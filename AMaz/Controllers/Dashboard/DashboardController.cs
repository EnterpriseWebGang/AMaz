using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
