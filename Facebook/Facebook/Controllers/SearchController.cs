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
            ViewData["Users"] = userData.GetUser(HttpContext);
            var loggedUserData = userData.GetUser(HttpContext);

            List<User> searchUsrs = facebookDataContext.Users
                .Where(usr => (usr.FirstName.ToUpper().Contains(search.ToUpper()) || usr.LastName.ToUpper().Contains(search.ToUpper())) && usr.Id != loggedUserData.Id).ToList();

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

        [HttpDelete]
        public IActionResult UnFriendAction(int InitiatorId, int DesiderId) {
            var userRelSelect = facebookDataContext.UserRelations.Where(usrRel => (usrRel.InitiatorId == InitiatorId || usrRel.InitiatorId == DesiderId) && (usrRel.InitiatorId == DesiderId || usrRel.DesiderId == DesiderId)).FirstOrDefault();
            if (userRelSelect == null)
                return Json(new { success = false});

            facebookDataContext.UserRelations.Remove(userRelSelect);
            facebookDataContext.SaveChanges();

            return Json(new { success = true});
        }

        [HttpPost]
        public IActionResult RequestFriendAction(int InitiatorId, int DesiderId) {
            UserRelation newUserRel = new UserRelation() {
                    CreatedAt = DateTime.Now,
                    DesiderId = DesiderId,
                    InitiatorId = InitiatorId,
                    IsDeleted = false,
                    SocialStatusId = (int)SocialStatuses.Request
                };
            facebookDataContext.UserRelations.Add(newUserRel);
            facebookDataContext.SaveChanges();
            return Json(new { success = true});
        }
    }
}