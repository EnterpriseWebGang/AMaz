using AMaz.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AMaz.Common;
using AutoMapper;

namespace AMaz.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        public LoginController(ILoginService loginService, IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<LoginRequest>(model);
                var response = await _loginService.SignInUserAsync(request);
                if (response.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Cannot login!";
                return View();
            }

            ViewBag.Error = "Invalid Input!";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Sign out the user and redirect to home page
            _loginService.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
