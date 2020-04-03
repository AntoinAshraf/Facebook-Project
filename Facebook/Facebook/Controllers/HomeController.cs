using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Facebook.Contracts;
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

        public HomeController(FacebookDataContext _facebookDataContext, IUserData _userData, IWebHostEnvironment hostingEnvironment)
        {
            this.userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            this.facebookDataContext = _facebookDataContext;
        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            ViewData["LayoutData"] = userData.GetLayoutData(HttpContext);
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
        [AuthorizedAction]
        public IActionResult DeletePost([FromQuery]int postId)
        {
            if (postId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            User user = userData.GetUser(HttpContext);
            Post post = facebookDataContext.Posts.Where(x => x.Id == postId).FirstOrDefault();
            UsersPost usersPost = facebookDataContext.UsersPosts.Where(x => x.PostId == post.Id && x.IsCreator).FirstOrDefault();
            if (post == null || usersPost.UserId != user.Id)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            post.IsDeleted = true;
            facebookDataContext.Posts.Update(post);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            return Json(new { statusCode = ResponseStatus.Success });
        }


        [HttpGet]
        public IActionResult GetPostById([FromQuery]int postId)
        {
            if (postId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            Post post = facebookDataContext.UsersPosts.Include(x => x.Post).FirstOrDefault(x => x.PostId == postId).Post;
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = new { postContent = post.PostContent, postId = post.Id } });
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
        [AuthorizedAction]
        public IActionResult CreatePost(IFormFile postImage, string postText)
        {
            string fileName = "";
            DateTime dateTimeNow = DateTime.Now;
            if (postText == null || postText == "")
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.EmptyPostText });

            if (postImage != null)
            {
                if (postImage.ContentType.ToLower() != "image/jpeg" && postImage.ContentType.ToLower() != "image/png" && postImage.ContentType.ToLower() != "image/jpg")
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
    }
}
