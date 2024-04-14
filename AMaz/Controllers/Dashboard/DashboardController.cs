using AMaz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DashboardController : Controller
    {
        private readonly DashBoardService _dashBoardService;
        private readonly IAcademicYearService _academicYearService;

        public DashboardController(DashBoardService dashBoardService, IAcademicYearService academicYearService) 
        {
            _dashBoardService = dashBoardService;
            _academicYearService = academicYearService;
        
        }
        
        public IActionResult Index()
        {
            ViewBag.AcademicYears = _academicYearService.GetAllAcademicYearsAsync();
            return View();
        }

        [HttpGet]        
        public async Task<IActionResult> GetContributionCount()
        {
            var model = await _dashBoardService.GetFacultyContributionCountByAcademicYear();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return Ok(json);
        }

        [HttpGet]
        public async Task<IActionResult> GetFacultyContributorPercentage(string academicYearId)
        {
            var model = await _dashBoardService.GetContributorPercentageByFacultyForAnAcademicYear(academicYearId);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return Ok(json);
        }

        [HttpGet]
        public async Task<IActionResult> GetContributorCount()
        {
            var model = await _dashBoardService.GetContributorCountByFacultyAndAcademicYear();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            return Ok(json);
        }
    }
}
