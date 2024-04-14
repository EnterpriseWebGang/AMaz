using AMaz.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AMaz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin");
            }
            else if (User.IsInRole("Manager"))
            {
                return RedirectToAction("Manager");
            }
            else if (User.IsInRole("Coordinator"))
            {
                return RedirectToAction("Coordinator");
            }
            ViewData["ActivePage"] = "Home";
            return View();
        }

        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet("Manager")]
        public IActionResult Manager()
        {
            return View();
        }

        [HttpGet("Coordinator")]
        public IActionResult Coordinator()
        {
            return View();
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied(string? returnUrl)
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
