using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Models.ViewModels;
using Facebook.Utilities;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using FacebookDbContext;
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

        public HomeController(ILogger<HomeController> logger, FacebookDataContext _facebookDataContext, IUserData _userData )
        {
            userData = _userData;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }

        //[AuthorizedAction]
        public IActionResult Index()
        {
            ViewData["Actions"] = userData.GetActions(HttpContext);
            
            TempData["Users"] = userData.GetUser(HttpContext).FirstName;
            return View();
        }

        //[AuthorizedAction]
        public IActionResult Privacy()
        {
            return View();
        }

        //[AuthorizedAction]
        [HttpPost]
        public IActionResult CreatePost(UserPostDTO userPost )
        {
            // userId, postId, createdAt, isCreator?????????????????????????????????

            User user = userData.GetUser(HttpContext);
            UsersPost usersPost = new UsersPost();

            userPost.CreatedAt = DateTime.Now;
            Post postToDB = new Post();

            postToDB.PostContent = userPost.PostContent;


            facebookDataContext.Posts.Add(postToDB);
            facebookDataContext.SaveChanges();

            usersPost.PostId = postToDB.Id; // post Id
            usersPost.CreatedAt = userPost.CreatedAt; // Post created time
            usersPost.UserId = user.Id; // user Id
            usersPost.IsCreator = true;


            facebookDataContext.UsersPosts.Add(usersPost);
            facebookDataContext.SaveChanges();

            return View("Index");
        }

       [HttpGet]
        public IActionResult GetPosts()
        {
            var postsResult = facebookDataContext.Posts.Include(p => p.UsersPosts)
                .ToList();
            
            List<PostsDTO> posts = new List<PostsDTO>();
            PostsDTO postToRetrieve;
            for (int i = 0; i < postsResult.Count;i++)
            {
                if (postsResult[i].IsDeleted == false)
                {
                    postToRetrieve = new PostsDTO();

                    foreach (var users in postsResult[i].UsersPosts)
                    {
                        postToRetrieve.Id = users.PostId;
                        var userP = facebookDataContext.Users.Find(users.UserId);

                        postToRetrieve.PostContent = postsResult[i].PostContent;
                        postToRetrieve.FirstName = userP.FirstName;
                        postToRetrieve.LastName = userP.LastName;
                        postToRetrieve.CreatedAt = users.CreatedAt;
                    }
                    posts.Add(postToRetrieve);
                }
            }
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = posts });
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

            return PartialView("EditPost", postView);
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
