using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Facebook.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;

        public SearchController(ILogger<HomeController> logger, FacebookDataContext _facebookDataContext, IUserData _userData) {
            userData = _userData;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }


        public IActionResult Index(string search)
        {
            var loggedUserData = userData.GetUser(HttpContext);

            List<User> searchUsrs = facebookDataContext.Users
                .Where(usr => (usr.FirstName.Contains(search) || usr.LastName.Contains(search)) && usr.Id != 1/*loggedUserData.Id*/).ToList();

            List<UserRelation> userRelations = (from usrRel in facebookDataContext.UserRelations
                                                where usrRel.InitiatorId == 1/*loggedUserData.Id*/ || usrRel.DesiderId == 1 /*loggedUserData.Id*/
                                                select usrRel).ToList();

            ViewData["usrsRelations"] = userRelations;

            return View(searchUsrs);
        }
    }
}