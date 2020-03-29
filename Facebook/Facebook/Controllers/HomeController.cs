using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.ExtentionClass;
using Facebook.Mappers;
using Facebook.Models.ViewModels;
using Facebook.Recources;
using Facebook.Utilities;
using Facebook.Utilities.Enums;
using Facebook.Validators;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace Facebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(FacebookDataContext _facebookDataContext, IUserData _userData , IWebHostEnvironment hostingEnvironment)
        {
            this.userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            this.facebookDataContext = _facebookDataContext;
        }

        //[AuthorizedAction]
        public IActionResult Index()
        {
            ViewData["Actions"] = userData.GetActions(HttpContext);
            ViewData["Users"] = userData.GetUser(HttpContext);
            int userId = userData.GetUser(HttpContext).Id;
            User userFullData = facebookDataContext.Users.Where(x => x.Id == userId)
                .Include("UserRelationsDesider.Initiator.UsersPosts.Post.Comments.User.ProfilePhotos")
                .Include("UserRelationsDesider.Initiator.UsersPosts.Post.Likes.User.ProfilePhotos")
                .Include("UserRelationsDesider.Initiator.UsersPosts.Post.PostPhotos")
                .Include("UserRelationsInitiator.Desider.UsersPosts.Post.Comments.User.ProfilePhotos")
                .Include("UserRelationsInitiator.Desider.UsersPosts.Post.Likes.User.ProfilePhotos")
                .Include("UserRelationsInitiator.Desider.UsersPosts.Post.PostPhotos")
                .Include("UsersPosts.Post.Comments.User.ProfilePhotos")
                .Include("UsersPosts.Post.Likes.User.ProfilePhotos")
                .Include("UsersPosts.Post.PostPhotos")
                .Include("ProfilePhotos")
                .FirstOrDefault();
            HomePageDto homePageDto = HomePageDtoMapper.Map(userFullData, hostingEnvironment);
            return View(homePageDto);
        }

        [HttpGet]
        public IActionResult DeletePost([FromQuery]int postId)
        {
            if (postId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            User user = userData.GetUser(HttpContext);
            Post post = facebookDataContext.Posts.Where(x => x.Id == postId).FirstOrDefault();
            UsersPost usersPost = facebookDataContext.UsersPosts.Where(x => x.PostId == post.Id && x.IsCreator).FirstOrDefault();
            if(post == null || usersPost.UserId != user.Id)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            post.IsDeleted = true;
            facebookDataContext.Posts.Update(post);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            return Json(new { statusCode = ResponseStatus.Success});
        }


        [HttpGet]
        public IActionResult GetPostById([FromQuery]int postId)
        {
            if (postId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            Post post = facebookDataContext.UsersPosts.Include(x=>x.Post).FirstOrDefault(x => x.PostId == postId).Post;
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = new { postContent = post.PostContent, postId = post.Id} });
        }

        [HttpPost]
        public IActionResult EditPost([FromBody]EditPostDto editPostDto)
        {
            if (editPostDto == null || editPostDto.PostId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            Post post = facebookDataContext.UsersPosts.Include(x => x.Post).FirstOrDefault(x => x.PostId == editPostDto.PostId).Post;
            post.PostContent = editPostDto.PostContent;
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            return Json(new { statusCode = ResponseStatus.Success });
        }

        [HttpPost]
        public IActionResult CreatePost(IFormFile postImage, string postText)
        {
            string fileName = "";
            DateTime dateTimeNow = DateTime.Now;
            if (postText == null || postText == "")
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.EmptyPostText });

            if (postImage != null)
            {
                if (postImage.ContentType != "image/jpeg" && postImage.ContentType != "image/png")
                    return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.WrongFormat });

                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "PostPics");
                fileName = postImage.FileName.Split(".")[0] + "_" + DateTime.Now.ToFileTime() + "." + postImage.FileName.Split(".")[1];
                var filePath = Path.Combine(uploads, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    postImage.CopyTo(fileStream);
                }
            }

            User user = userData.GetUser(HttpContext);
            string userProfilePic = facebookDataContext.ProfilePhotos.FirstOrDefault(x => x.UserId == user.Id).Url;

            Post post = new Post() { IsDeleted = false, PostContent = postText };
            facebookDataContext.Posts.Add(post);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }

            UsersPost usersPost = new UsersPost() { PostId = post.Id, IsCreator = true, CreatedAt = dateTimeNow, UserId = user.Id };
            facebookDataContext.UsersPosts.Add(usersPost);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }

            if (postImage != null)
            {
                PostPhoto postPhoto = new PostPhoto() { IsDeleted = false, PostId = post.Id, CreatedAt = dateTimeNow, Url = fileName };
                facebookDataContext.PostPhotos.Add(postPhoto);
                try { facebookDataContext.SaveChanges(); }
                catch { return Json(new { statusCode = ResponseStatus.Error }); }
            }

            return Json(new { statusCode = ResponseStatus.Success });
        }

        // public string test()
        // {
        //    return Path.Combine(hostingEnvironment.WebRootPath + "\\images");
        // }

        // //[AuthorizedAction]
        // [HttpPost]
        // public IActionResult CreatePost(UserPostDTO userPost )
        // {
        //     try
        //     {
        //         User user = userData.GetUser(HttpContext);

        //         Post postToDB = new Post();
        //         postToDB.PostContent = userPost.PostContent;


        //         PostValidator validator = new PostValidator();
        //         var result = validator.Validate(postToDB);
        //         if (!result.IsValid)
        //         {
        //             return RedirectToAction("Index");
        //             // ModelState.AddModelError("Data", "Canot Save Data");
        //         }

        //         //save post
        //         facebookDataContext.Posts.Add(postToDB);
        //         facebookDataContext.SaveChanges();

        //         //to create copy from Image

        //         string uniqueFileName = null;
        //         if (userPost.image != null)
        //         {
        //             string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\images");
        //             uniqueFileName = Guid.NewGuid().ToString() + "_" + userPost.image.FileName;

        //             string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //             userPost.image.CopyTo(new FileStream(filePath, FileMode.Create));

        //             //create photo and save into db
        //             PostPhoto postPhotoToDb = new PostPhoto() { Url = uniqueFileName, CreatedAt = DateTime.Now, PostId = postToDB.Id };
        //             postToDB.PostPhotos.Add(postPhotoToDb);
        //         }

        //         UsersPost usersPost = new UsersPost();
        //         userPost.CreatedAt = DateTime.Now;
        //         usersPost.PostId = postToDB.Id; // post Id
        //         usersPost.CreatedAt = userPost.CreatedAt; // Post created time
        //         usersPost.UserId = user.Id; // user Id
        //         usersPost.IsCreator = true;
        //         facebookDataContext.UsersPosts.Add(usersPost);



        //         facebookDataContext.SaveChanges();

        //         return RedirectToAction("Index");
        //     }

        //     catch
        //     {
        //         //ModelState.AddModelError("Data", "Canot Save Data");
        //         return RedirectToAction("Index");
        //     }

        // }

        //[HttpGet]
        // public IActionResult GetPosts()
        // {

        //     try {
        //         User user = userData.GetUser(HttpContext);
        //         bool flag = true;

        //         //Get All Friend
        //         var allFriends = facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.InitiatorId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.DesiderId)
        //             .Union(facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.DesiderId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.InitiatorId));

        //         // NOT WORKING? WHY??!
        //         //var allFriends = facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.InitiatorId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.DesiderId);
        //         //allFriends.Union(facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.DesiderId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.InitiatorId));


        //         var postsResult = facebookDataContext.Posts.Include(p => p.UsersPosts).Include(p => p.PostPhotos)
        //       .ToList();
        //         if(postsResult==null) 
        //             return Json(new { statusCode = ResponseStatus.NoDataFound });

        //         List<PostsDTO> posts = new List<PostsDTO>();
        //         PostsDTO postToRetrieve;
        //         for (int i = 0; i < postsResult.Count; i++)
        //         {
        //             if (postsResult[i].IsDeleted == false)
        //             {
        //                 foreach (var users in postsResult[i].UsersPosts)
        //                 {
        //                     postToRetrieve = new PostsDTO();
        //                     postToRetrieve.Id = users.PostId;
        //                     var userP = facebookDataContext.Users?.Find(users.UserId);


        //                     postToRetrieve.PostContent = postsResult[i].PostContent;
        //                     postToRetrieve.FirstName = userP.FirstName;
        //                     postToRetrieve.LastName = userP.LastName;
        //                     postToRetrieve.CreatedAt =users.CreatedAt;

        //                     //for Edit Option
        //                     if (userP.Id == user.Id)
        //                         postToRetrieve.Owner = true;
        //                     if (postToRetrieve.Owner == false)
        //                     {
        //                         //out if Not Friend
        //                         if (!allFriends.Contains(userP.Id))
        //                         {
        //                              break;
        //                         }
        //                     }
        //                     if (postsResult[i].PostPhotos.Count>0)
        //                     {
        //                         foreach(var postPhoto in postsResult[i].PostPhotos)
        //                         {
        //                             postToRetrieve.UrlPost.Add(postPhoto.Url); 
        //                         }
        //                     }

        //                     posts.Add(postToRetrieve);
        //                 }

        //             }
        //         }

        //         return Json(new { statusCode = ResponseStatus.Success, responseMessage = posts });
        //     }
        //     catch
        //     {
        //         return Json(new { statusCode = ResponseStatus.NoDataFound });
        //     }

        //     //return PartialView("Posts", posts);

        // }


        // [HttpGet]
        // public IActionResult EditPost(int? id)
        // {
        //     if (id == null)
        //     {
        //     }

        //     var postModel = facebookDataContext.Posts.Find(id);

        //     PostsDTO postView = new PostsDTO();

        //     postView.Id = postModel.Id;
        //     postView.PostContent = postModel.PostContent;

        //     // return Json(new { statusCode = ResponseStatus.Success, responseMessage = postView });
        //     return View("EditPost",postView);
        // }

        // [HttpPost]
        // public IActionResult EditPost(Post post)
        // {
        //     if(ModelState.IsValid)
        //     {
        //         facebookDataContext.Entry(post).State = EntityState.Modified;
        //         facebookDataContext.SaveChanges();
        //         return RedirectToAction("Index");
        //     }
        //     return PartialView("Posts", post);

        // }


        // public IActionResult DeletePost(int? id)
        // {
        //     if(id == null)
        //     {

        //     }

        //     Post postToDelete = facebookDataContext.Posts.Find(id);
        //     postToDelete.IsDeleted = true;
        //     facebookDataContext.SaveChanges();

        //     return View("Index");
        // }
    }
}
