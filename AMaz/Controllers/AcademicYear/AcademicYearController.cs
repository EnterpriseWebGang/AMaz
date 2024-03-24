using AMaz.Service;
using Microsoft.AspNetCore.Mvc;
using AMaz.Common;
using AutoMapper;


namespace AMaz.Web.Controllers
{
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearService _academicYearService;
        private readonly IMapper _mapper;

        public AcademicYearController(IAcademicYearService academicYearService, IMapper mapper)
        {
            _academicYearService = academicYearService;
            _mapper = mapper;
        }

        // GET: /AcademicYear/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /AcademicYear/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateAcademicYearViewModel model)
        {
            try
            {
                // Create a new academic year
                var request = _mapper.Map<CreateAcademicYearRequest>(model);
                var response = await _academicYearService.CreateAcademicYearAsync(request);

                // Redirect to the list of academic years after successful creation
                return RedirectToAction("Index");
            }
            catch (AppException ex)
            {
                // Display an error message for invalid academic year creation
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: /AcademicYear/Udpate
        public async Task<IActionResult> UpdateAsync(string id)
        {
            // Get the academic year by id
            var academicYear = await _academicYearService.GetAcademicYearByIdAsync(id);
            var model = _mapper.Map<UpdateAcademicYearViewModel>(academicYear);
            model.AcademicYearId = id;

            // Display the academic year details for update
            return View(model);
        }

        // POST: /AcademicYear/Update
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAcademicYearViewModel model)
        {
            try
            {
                // Update the academic year
                var request = _mapper.Map<UpdateAcademicYearRequest>(model);
                request.AcademicYearId = model.AcademicYearId;
                var response = await _academicYearService.UpdateAcademicYearAsync(request);

                // Redirect to the list of academic years after successful update
                return RedirectToAction("Index");
            }
            catch (AppException ex)
            {
                // Display an error message for invalid academic year update
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: /AcademicYear/Delete
        public async Task<IActionResult> Delete(string id)
        {
            // Get the academic year by id
            var response = await _academicYearService.DeleteAcademicYearAsync(id);

            // Display the academic year details for delete
            return RedirectToAction("Index");
        }

        // GET: /AcademicYear/Index
        public async Task<IActionResult> Index()
        {
            // Get all academic years
            var response = await _academicYearService.GetAllAcademicYearsAsync();

            // Display the list of academic years
            return View(response);
        }

        // GET: /AcademicYear/Details
        public async Task<IActionResult> Details(string id)
        {
            // Get the academic year by id
            var response = await _academicYearService.GetAcademicYearByIdAsync(id);

            // Display the academic year details
            return View(response);
        }
    }
}
