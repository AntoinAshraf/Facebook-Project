using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;

namespace Facebook.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private static string searchStr;

        public SearchController(ILogger<HomeController> logger, FacebookDataContext _facebookDataContext, IUserData _userData) {
            userData = _userData;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
            searchStr = "";
        }

        

        public IActionResult Index(string search) {

            if(search == "") {
                return RedirectToAction("/Home/Index");
            }
            searchStr = search;
            ViewData["Actions"] = userData.GetActions(HttpContext);
            var loggedUserData = userData.GetUser(HttpContext);

            List<User> searchUsrs = facebookDataContext.Users
                .Where(usr => (usr.FirstName.Contains(search) || usr.LastName.Contains(search)) && usr.Id != loggedUserData.Id).ToList();

            List<UserRelation> userRelations = (from usrRel in facebookDataContext.UserRelations
                                                where usrRel.InitiatorId == loggedUserData.Id || usrRel.DesiderId == loggedUserData.Id
                                                select usrRel).ToList();

            ViewData["usrsRelations"] = userRelations;
            ViewData["LoggedUser"] = loggedUserData.Id;


            return View(searchUsrs);
        }

        [HttpPut]
        public IActionResult ConfirmFriendAction(int InitiatorId, int DesiderId) {

            var userRelSelect = facebookDataContext.UserRelations.Where(usrRel => usrRel.InitiatorId == InitiatorId && usrRel.DesiderId == DesiderId).FirstOrDefault();
            if (userRelSelect == null)
                return Json(new { success = false });

            userRelSelect.SocialStatusId = (int)SocialStatuses.Friend;
            facebookDataContext.SaveChanges();
            //return RedirectToAction("index", searchStr);
            return Json(new { success = true });


        }
    }
}