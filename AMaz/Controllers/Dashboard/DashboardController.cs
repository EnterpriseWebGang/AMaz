using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController() { }


        public IActionResult Index()
        {
            return View();
        }

        

        public IActionResult GetFacultyContributionCountByAcademicYear()
        {

        }
    }
}
