using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace musicdb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Artists()
        {
            return View();
        }

        public IActionResult Albums()
        {
            return View();
        }

        public IActionResult Tracks()
        {
            return View();
        }

        public IActionResult Genres()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numTimes)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
