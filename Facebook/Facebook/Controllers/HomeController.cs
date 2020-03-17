using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Models.ViewModels;
using Facebook.Utilities;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        //[AuthorizedAction]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreatePost(UserPostDTO userPost)
        {
            Post postToDB = new Post();

            postToDB.PostContent = userPost.PostContent;

            facebookDataContext.Posts.Add(postToDB);
            facebookDataContext.SaveChanges();

            return View("Index");
        }
    }
}
