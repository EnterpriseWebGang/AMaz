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
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public ContributionController(IContributionService contributionService, FileService fileService ,IMapper mapper)
        {
            _contributionService = contributionService;
            _fileService = fileService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var contributions = await _contributionService.GetAllContributionsAsync();
            return View(_mapper.Map<IEnumerable<ContributionViewModel>>(contributions));
        }

        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            var model = new CreateContributionViewModel();
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
                var result = await _contributionService.CreateContributionAsync(request);

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

            return View(_mapper.Map<ContributionViewModel>(contribution));
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

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateContributionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new UpdateContributionRequest
                {
                    Title = model.Title,
                    Content = model.Content,
                    Files = model.Files
                };

                var result = await _contributionService.UpdateContributionAsync(request);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Failed to update contribution.";
                }
            }
            return View(model);
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
