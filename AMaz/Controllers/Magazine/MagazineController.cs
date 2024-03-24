using AMaz.Common;
using AMaz.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class MagazineController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMagazineService _magazineService;
        private readonly IAcademicYearService _academicYearService;
        public MagazineController(IMagazineService magazineService, IMapper mapper, IAcademicYearService academicYearService)
        {
            _academicYearService = academicYearService;
            _magazineService = magazineService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string facultyId = null)
        {
            if(facultyId != null)
            {
                var byFaculty = await _magazineService.GetAllMagazineByFacultyId(facultyId);
                return View(byFaculty);
            }
            ViewBag.Error = TempData["Error"];
            var model = await _magazineService.GetAllMagazines();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMagazineViewModel model)
        {
            try
            {
                var request = _mapper.Map<CreateMagazineRequest>(model);
                var response = await _magazineService.CreateMagazineAsync(request);
                if (!response.succeed)
                {
                    TempData["Error"] = response.errorMsg;
                }

                return RedirectToAction("Index");
            }
            catch (AppException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Update(string id)
        {
            var magazine = await _magazineService.GetMagazineByIdAsync(id);
            var academicYears = await _academicYearService.GetAllAcademicYearsAsync();
            var model = _mapper.Map<UpdateMagazineViewModel>(magazine);
            model.AcademicYears = academicYears.ToList();
            ViewBag.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateMagazineViewModel model)
        {
            try
            {
                var request = _mapper.Map<UpdateMagazineRequest>(model);
                request.MagazineId = id;
                var response = await _magazineService.UpdateMagazineAsync(request);
                if (!response.succeed)
                {
                    TempData["Error"] = response.errorMsg;
                }

                return RedirectToAction("Index");
            }
            catch (AppException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = await _magazineService.DeleteMagazineAsync(id);
            if (!response.succeed)
            {
                TempData["Error"] = response.errorMsg;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var magazine = await _magazineService.GetMagazineByIdAsync(id);
            var model = _mapper.Map<MagazineDetailViewModel>(magazine);
            return View(model);
        }
    }
}
