using AMaz.Common;
using AMaz.Entity;
using AMaz.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AMaz.Web.Controllers
{
    public class ContributionController : Controller
    {
        private readonly IContributionService _contributionService;
        private readonly IMagazineService _magazineService;
        private readonly FileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ContributionController(IContributionService contributionService, 
            FileService fileService ,
            IMapper mapper, 
            IMagazineService magazineService,
            IEmailService emailService)
        {
            _contributionService = contributionService;
            _fileService = fileService;
            _mapper = mapper;
            _magazineService = magazineService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var contributions = await _contributionService.GetAllContributionsAsync();
            return View(_mapper.Map<IEnumerable<ContributionViewModel>>(contributions));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create()
        {
            var magazines = await _magazineService.GetAllMagazines();
            var model = new CreateContributionViewModel();
            model.Magazines = magazines;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContributionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<CreateContributionRequest>(model);
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.SubmissionDate = DateTime.Now;
                var result = await _contributionService.CreateContributionAsync(request, async (contribution, coordinator) =>
                {
                    if (contribution != null && coordinator != null)
                    {
                        await _emailService.SendCreateContributionEmail(contribution, coordinator, Url.ActionLink("Details", "Contribution", new { id = contribution.ContributionId.ToString() }) ?? "No Link To Action");
                    }
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Failed to create contribution.";
                }
            }

            // If the model state is not valid or creation failed, return the same view with the model
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _contributionService.DeleteContributionAsync(id);

            if (result)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Failed to delete contribution.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Update(string id)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UpdateContributionViewModel>(contribution);
            ViewBag.Id = id;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id,UpdateContributionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<UpdateContributionRequest>(model);
                request.ContributionId = id;

                var result = await _contributionService.UpdateContributionAsync(request, async (contribution, coordinator) =>
                {
                    if (contribution != null && coordinator != null)
                    {
                        await _emailService.SendUpdateContributionEmail(contribution, coordinator, Url.ActionLink("Details", "Contribution", new { id = contribution.ContributionId.ToString() }) ?? "No Link To Action");
                    }
                });
                if (!result)
                {
                    ViewBag.Error = "Failed to update contribution.";
                }
            }
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetContributionFile(string fileId, string contributionId)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(contributionId);
            if (contribution == null)
            {
                return NotFound();
            }

            var file = await _fileService.GetFileById(fileId);
            if (file.filestream == null)
            {
                return NotFound();
            }

            return File(file.filestream, file.contentType, file.fileName);
        }

    }
}
