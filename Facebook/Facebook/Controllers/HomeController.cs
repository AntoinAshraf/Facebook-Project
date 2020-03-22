using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.ExtentionClass;
using Facebook.Models.ViewModels;
using Facebook.Utilities;
using Facebook.Utilities.Enums;
using Facebook.Validators;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

namespace Facebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, FacebookDataContext _facebookDataContext, IUserData _userData 
            ,IHostingEnvironment hostingEnvironment)
        {
            userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }

        //[AuthorizedAction]
        public IActionResult Index()
        {
            ViewData["Actions"] = userData.GetActions(HttpContext); 
            //TempData["Users"] = userData.GetUser(HttpContext).FirstName;
            return View();
        }

        //[AuthorizedAction]
        public IActionResult Privacy()
        {
            return View();
        }

        public string test()
        {
        
           return Path.Combine(hostingEnvironment.WebRootPath + "\\images");
        }

        //[AuthorizedAction]
        [HttpPost]
        public IActionResult CreatePost(UserPostDTO userPost )
        {
            try
            {
                User user = userData.GetUser(HttpContext);

                Post postToDB = new Post();
                postToDB.PostContent = userPost.PostContent;


                PostValidator validator = new PostValidator();
                var result = validator.Validate(postToDB);
                if (!result.IsValid)
                {
                    return RedirectToAction("Index");
                    // ModelState.AddModelError("Data", "Canot Save Data");
                }

                //save post
                facebookDataContext.Posts.Add(postToDB);
                facebookDataContext.SaveChanges();

                //to create copy from Image

                string uniqueFileName = null;
                if (userPost.image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath + "\\images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + userPost.image.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    userPost.image.CopyTo(new FileStream(filePath, FileMode.Create));

                    //create photo and save into db
                    PostPhoto postPhotoToDb = new PostPhoto() { Url = uniqueFileName, CreatedAt = DateTime.Now, PostId = postToDB.Id };
                    postToDB.PostPhotos.Add(postPhotoToDb);
                }

                UsersPost usersPost = new UsersPost();
                userPost.CreatedAt = DateTime.Now;
                usersPost.PostId = postToDB.Id; // post Id
                usersPost.CreatedAt = userPost.CreatedAt; // Post created time
                usersPost.UserId = user.Id; // user Id
                usersPost.IsCreator = true;
                facebookDataContext.UsersPosts.Add(usersPost);



                facebookDataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            catch
            {
                //ModelState.AddModelError("Data", "Canot Save Data");
                return RedirectToAction("Index");
            }
         
        }

       [HttpGet]
        public IActionResult GetPosts()
        {

            try {
                User user = userData.GetUser(HttpContext);
                bool flag = true;

                //Get All Friend
                var allFriends = facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.InitiatorId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.DesiderId)
                    .Union(facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.DesiderId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.InitiatorId));

                // NOT WORKING? WHY??!
                //var allFriends = facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.InitiatorId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.DesiderId);
                //allFriends.Union(facebookDataContext.UserRelations.Include(p => p.SocialStatus).Where(p => p.DesiderId == user.Id && p.SocialStatus.Id == (int)SocialStatuses.Friend).Select(p => p.InitiatorId));


                var postsResult = facebookDataContext.Posts.Include(p => p.UsersPosts).Include(p => p.PostPhotos)
              .ToList();
                if(postsResult==null) 
                    return Json(new { statusCode = ResponseStatus.NoDataFound });
                
                List<PostsDTO> posts = new List<PostsDTO>();
                PostsDTO postToRetrieve;
                for (int i = 0; i < postsResult.Count; i++)
                {
                    if (postsResult[i].IsDeleted == false)
                    {
                        foreach (var users in postsResult[i].UsersPosts)
                        {
                            postToRetrieve = new PostsDTO();
                            postToRetrieve.Id = users.PostId;
                            var userP = facebookDataContext.Users?.Find(users.UserId);

                           
                            postToRetrieve.PostContent = postsResult[i].PostContent;
                            postToRetrieve.FirstName = userP.FirstName;
                            postToRetrieve.LastName = userP.LastName;
                            postToRetrieve.CreatedAt =users.CreatedAt;
                         
                            //for Edit Option
                            if (userP.Id == user.Id)
                                postToRetrieve.Owner = true;
                            if (postToRetrieve.Owner == false)
                            {
                                //out if Not Friend
                                if (!allFriends.Contains(userP.Id))
                                {
                                     break;
                                }
                            }
                            if (postsResult[i].PostPhotos.Count>0)
                            {
                                foreach(var postPhoto in postsResult[i].PostPhotos)
                                {
                                    postToRetrieve.UrlPost.Add(postPhoto.Url); 
                                }
                            }

                            posts.Add(postToRetrieve);
                        }
                        
                    }
                }
              
                return Json(new { statusCode = ResponseStatus.Success, responseMessage = posts });
            }
            catch
            {
                return Json(new { statusCode = ResponseStatus.NoDataFound });
            }
            
            //return PartialView("Posts", posts);

        }


        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            if (id == null)
            {
            }
            
            var postModel = facebookDataContext.Posts.Find(id);

            PostsDTO postView = new PostsDTO();

            postView.Id = postModel.Id;
            postView.PostContent = postModel.PostContent;

            // return Json(new { statusCode = ResponseStatus.Success, responseMessage = postView });
            return View("EditPost",postView);
        }

        [HttpPost]
        public IActionResult EditPost(Post post)
        {
            if(ModelState.IsValid)
            {
                facebookDataContext.Entry(post).State = EntityState.Modified;
                facebookDataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("Posts", post);

        }


        public IActionResult DeletePost(int? id)
        {
            if(id == null)
            {

            }
            
            Post postToDelete = facebookDataContext.Posts.Find(id);
            postToDelete.IsDeleted = true;
            facebookDataContext.SaveChanges();

            return View("Index");
        }
    }
}
