using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FacebookDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Facebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        public HomeController(ILogger<HomeController> logger, FacebookDataContext _facebookDataContext)
        {
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }

        public IActionResult Index()
        {
            var x = facebookDataContext.Users.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
