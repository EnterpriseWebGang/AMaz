using AMaz.Common;
using AMaz.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;
        private readonly IMapper _mapper;
        public FacultyController(IFacultyService facultyService, IMapper mapper)
        {
            _mapper = mapper;
            _facultyService = facultyService;
        }
        
        public async Task<IActionResult> Index()
        {
            var faculties = await _facultyService.GetAllFacultiesAsync();
            return View(faculties);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(CreateFacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<CreateFacultyRequest>(model);
                var result = await _facultyService.CreateFacultyAsync(request);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(string id)
        {
            var faculty = await _facultyService.GetFacultyByIdAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            ViewBag.FacultyId = id;
            var model = _mapper.Map<UpdateFacultyViewModel>(faculty);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(string id, UpdateFacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<UpdateFacultyRequest>(model);
                request.FacultyId = id;
                var result = await _facultyService.UpdateFacultyAsync(request);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.FacultyId = id;
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            var faculty = await _facultyService.GetFacultyByIdAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            
            await _facultyService.DeleteFacultyAsync(id);
            return RedirectToAction("Index");
        }


    }
}
