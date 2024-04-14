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
using Microsoft.IdentityModel.Tokens;

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

        //[Authorize(Roles = "Manager")]
        //public async Task<IActionResult> Index()
        //{
        //    var contributions = await _contributionService.GetAllContributionsAsync();
        //    return View(_mapper.Map<IEnumerable<ContributionViewModel>>(contributions));
        //}

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(string magazineId)
        {
            //var magazines = await _magazineService.GetAllMagazines();
            if (magazineId.IsNullOrEmpty())
            {
                return NotFound();
            }
            var model = new CreateContributionViewModel();
            model.MagazineId = magazineId;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContributionViewModel model, string magazineId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = _mapper.Map<CreateContributionRequest>(model);
                    request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    request.SubmissionDate = DateTime.Now;
                    request.MagazineId = magazineId;
                    var result = await _contributionService.CreateContributionAsync(request, async (contribution, coordinator) =>
                    {
                        if (contribution != null && coordinator != null)
                        {
                            await _emailService.SendCreateContributionEmail(contribution, coordinator, Url.ActionLink("Details", "Contribution", new { id = contribution.ContributionId.ToString() }) ?? "No Link To Action");
                        }
                    });

                    if (result)
                    {
                        return RedirectToAction("Details", "Magazine", new { id = magazineId });
                    }
                    else
                    {
                        ViewBag.Error = "Failed to create contribution.";
                    }
                }
                catch (Exception e)
                {

                    ViewBag.Error = $"Failed to create contribution. Error: {e}"; ;
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
        public async Task<IActionResult> Delete(string id, string magazineId)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != contribution.User.Id)
            {
                return Forbid();
            }

            var result = await _contributionService.DeleteContributionAsync(id);

            if (result)
            {
                return RedirectToAction("Details", new { id });
            }

            ViewBag.Error = "Failed to delete contribution.";
            return RedirectToAction("Details", "Magazine", new { id = magazineId });
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Update(string id)
        {
            var contribution = await _contributionService.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }
            if(User.FindFirstValue(ClaimTypes.NameIdentifier) != contribution.User.Id)
            {
                return Forbid();
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
                var contribution = await _contributionService.GetContributionByIdAsync(id);
                if (contribution == null)
                {
                    return NotFound();
                }
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != contribution.User.Id)
                {
                    return Forbid();
                }

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
        [Authorize(Roles = "Coordinator")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCoordinatorComment(ContributionCommentDtos commentDto)
        {
            try
            {
                // Check if the comment DTO is valid
                if (!ModelState.IsValid)
                {
                    // If not, return a bad request response with the model state errors
                    return BadRequest(ModelState);
                }

                // Attempt to add the coordinator comment using the service
                var success = await _contributionService.AddCommentAsync(commentDto);

                // Check if adding the comment was successful
                if (success)
                {
                    // If successful, return a 200 OK response
                   
                    return RedirectToAction("Details", new { id = commentDto.ContributionId });
                }
                else
                {
                    // If not successful, return a 500 internal server error response
                    return StatusCode(500, "Failed to add coordinator comment.");
                }
            }
            catch (ArgumentException ex)
            {
                // Handle the case where an invalid argument exception is thrown (e.g., invalid contribution ID)
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Coordinator")] // Ensure only coordinators can approve contributions
        public async Task<IActionResult> ApproveContribution(string contributionId)
        {
            try
            {
                // Call the service method to approve the contribution
                var result = await _contributionService.ApproveContributionAsync(contributionId);

                if (result)
                {
                    // Return a success message if the contribution is approved
                    return RedirectToAction("Details", new {id= contributionId });
                }
                else
                {
                    // Handle the case where the contribution cannot be approved (e.g., not found)
                    return NotFound("Contribution not found or failed to approve.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the approval process
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coordinator")] // Ensure only coordinators can reject contributions
        public async Task<IActionResult> RejectContribution(string contributionId)
        {
            try
            {
                // Call the service method to reject the contribution
                var result = await _contributionService.RejectContributionAsync(contributionId);

                if (result)
                {
                    // Redirect to a success page or show a success message
                    TempData["SuccessMessage"] = "Contribution rejected successfully.";
                    RedirectToAction("Details", new { id = contributionId });
                }
                else
                {
                    // If the rejection fails, redirect back with an error message
                    TempData["ErrorMessage"] = "Failed to reject contribution.";
                }
            }
            catch (ArgumentException ex)
            {
                // Handle the case where an invalid argument exception is thrown
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect back to the details page or any other appropriate page
            return RedirectToAction("Details", new { id = contributionId });
        }


    }
}
