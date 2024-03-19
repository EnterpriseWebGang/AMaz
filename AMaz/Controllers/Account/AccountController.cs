using Microsoft.AspNetCore.Mvc;
using AMaz.Service;
using AMaz.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AMaz.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public AccountController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [Authorize()]
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

                ViewBag.Error = result.ToString();
                return View(model);
            }

            ViewBag.Error = "Invalid Input!";
            return View(model);
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
        public IActionResult ResetPassword(string userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Error = TempData["ErrorMessage"];
            return View();
        }

        // POST: /Account/ResetPassword/{userId}
        [HttpPost]
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
    }
}
