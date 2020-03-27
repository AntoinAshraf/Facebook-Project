using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Mappers;
using Facebook.Models.ViewModels;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
////DESKTOP-J75213F\SQLEXPRESS
namespace Facebook.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProfileController(ILogger<ProfileController> logger, FacebookDataContext _facebookDataContext, IUserData _userData
              , IHostingEnvironment hostingEnvironment)
        {
            userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }

        public IActionResult Profile()
        {

            ViewData["Actions"] = userData.GetActions(HttpContext);
            TempData["Users"] = userData.GetUser(HttpContext);
            return View();
        }


        [HttpGet]
        /***
         * id --> this user Id
         */
        public IActionResult GetPosts(int?id)
        { // is DELTEDDDDDDDDDDDDDDDDDDDDDDDDD, posts comment??????????????????
            try
            {
                if(id==null) return Json(new { statusCode = ResponseStatus.NoDataFound });              
                User user = userData.GetUser(HttpContext);

                //to check who is user?
                bool flag = true;//to check Owner prop
                if(id!=user.Id)
                {
                    user = facebookDataContext.Users.SingleOrDefault(p => p.Id == id);
                    flag = false;
                }



                PostsMapper postsMapper = new PostsMapper(facebookDataContext);

                var postsResult = facebookDataContext.UsersPosts.Where(p => p.UserId == user.Id && p.Post.IsDeleted == false)
                    .Include(p => p.Post)
                    .ThenInclude(p => p.PostPhotos)
                    .Include(p => p.Post.Comments)
                    .ToList();

                if (postsResult == null || postsResult.Count < 0)
                    return Json(new { statusCode = ResponseStatus.NoDataFound });

                // Mapping
                List<PostsDTO> postsToRetrieve = postsMapper.Map(postsResult, user).ToList();
                postsToRetrieve.ForEach(o => o.Owner = flag);

                return Json(new { statusCode = ResponseStatus.Success, responseMessage = postsToRetrieve });
            }
            catch
            {
                return Json(new { statusCode = ResponseStatus.NoDataFound });
            }
        }

       
    }
}