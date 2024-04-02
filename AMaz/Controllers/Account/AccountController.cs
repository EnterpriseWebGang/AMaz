﻿using Microsoft.AspNetCore.Mvc;
using AMaz.Service;
using AMaz.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using AMaz.Entity;

namespace AMaz.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IFacultyService _facultyService;
        private readonly IMapper _mapper;

        public AccountController(UserService userService, IMapper mapper, IFacultyService facultyService)
        {
            _userService = userService;
            _mapper = mapper;
            _facultyService = facultyService;
        }

        // GET: AccountController
        [Authorize()]
        public async Task<ActionResult> Index()
        {
            var models = await _userService.GetAllUsersAsync();
            return View(models);
        }

        //// GET: AccountController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: AccountController/Create
        [Authorize()]
        public ActionResult Create()
        {
            var faculties = _facultyService.GetAllFacultiesAsync().Result;
            var model = new CreateAccountViewModel
            {
                Faculties = faculties.ToList()
            };
            return View(model);
        }

        // POST: AccountController/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<CreateAccountRequest>(model);
                var result = await _userService.CreateAccount(request);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                // Check if the error description contains "Email already exists"
                if (result.Errors.Any(e => e.Description.Contains("Email already exists")))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }
                else
                {
                    ViewBag.Error = result.ToString();
                }

                return View(model);
            }

            ViewBag.Error = "Invalid Input!";
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> CheckEmailExists(string email)
        {
            var isEmailExist = await _userService.IsEmailExistAsync(email);
            return Json(new { exists = isEmailExist });
        }
        public async Task<ActionResult> Deactivate(string userId)
        {
            var result = await _userService.DeactivateUser(userId);
            if (result.result)
            {
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = result.errorMessage;
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Activate(string userId)
        {
            var result = await _userService.ActivateUser(userId);
            if (result.result)
            {
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = result.errorMessage;
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.result)
            {
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = result.errorMessage;
            return RedirectToAction("Index");
        }

        // GET: /Account/ResetPassword/{userId}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ResetPassword(string userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Error = TempData["ErrorMessage"];
            return View();
        }

        // POST: /Account/ResetPassword/{userId}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string userId, ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Wrong Input!";
                return RedirectToAction("ResetPassword", new {userId = userId});
            }

            var request = _mapper.Map<ResetPasswordRequest>(model);
            request.UserId = userId;
            var result = await _userService.ResetPassword(request);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = string.Join("\n", result.Errors.Select(e => e.Description));
                return RedirectToAction("ResetPassword", new { userId = userId }); ;
            }

            return RedirectToAction("Index");
        }

        // GET: ChangeUserRole
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRoleAndFaculty(string userId)
        {
            var model = await _userService.GetUserRoleViewModelAsync(userId);
            var faculties = await _facultyService.GetAllFacultiesAsync();
            model.Faculties = faculties.ToList();
            ViewBag.UserId = userId;
            ViewBag.Error = TempData["ErrorMessage"];
            return View(model);
        }

        // POST: ChangeUserRole
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRoleAndFaculty(string userId, ChangeUserRoleAndFacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<ChangeUserRoleAndFacultyRequest>(model);
                request.UserId = userId;

                var result = await _userService.ChangeUserRole(request);
                if (result.result)
                {
                    return RedirectToAction("Index");
                }

                else
                {
                    TempData["ErrorMessage"] = result.error;
                    return RedirectToAction("ChangeUserRole", new { userId = userId }); ;
                }
            }
            ViewBag.UserId = userId; 
            return RedirectToAction("ChangeUserRole", new {userId = userId});
        }
    }
}
