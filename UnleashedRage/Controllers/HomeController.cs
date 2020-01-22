using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnleashedRage.Database;
using UnleashedRage.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UnleashedRage.Controllers {
    public class HomeController : Controller {
        private readonly URContext _context;

        public HomeController (URContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            // get latest issue from database
            ComicPage latestIssue = ComicPageDB.GetOnePage(_context, 9);

            // send image to Home.Index view
            ViewBag.CurrentPage = latestIssue;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
