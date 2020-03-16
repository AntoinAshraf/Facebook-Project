using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Utilities;
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

        [AuthorizedAction]
        public IActionResult Index()
        {
            ViewData["Actions"] = userData.GetActions(HttpContext);
            return View();
        }

        [AuthorizedAction]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
