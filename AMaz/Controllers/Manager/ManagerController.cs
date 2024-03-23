using AMaz.Common;
using AMaz.Entity;
using AMaz.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Web.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IContributionService _contributionService;
        private readonly IMapper _mapper;

        public ManagerController(IContributionService contributionService, IMapper mapper)
        {
            _contributionService = contributionService ?? throw new ArgumentNullException(nameof(contributionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index()
        {
            var contributions = await _contributionService.GetAllContributionsAsync();
            return View(_mapper.Map<IEnumerable<ContributionViewModel>>(contributions));
        }

        public IActionResult Create()
        {
            var model = new CreateContributionViewModel(); // Create an instance of CreateContributionViewModel
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContributionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<CreateContributionRequest>(model);
                var result = await _contributionService.CreateContributionAsync(request);

                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Failed to create contribution.";
                }
            }

            // If the model state is not valid or creation failed, return the same view with the model
            return View(model);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ContributionViewModel>(contribution));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contributionService.DeleteContributionAsync(id);

            if (result)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Failed to delete contribution.";
            return RedirectToAction("Index");
        }
    }
}
