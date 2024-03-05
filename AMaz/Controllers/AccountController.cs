using AMaz.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AMaz.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            bool isLogin = await _loginService.AuthenticateUserAsync(email, password);

            if (isLogin)
            {
                bool signInResult = await _loginService.SignInUserAsync(email);

                if (signInResult)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Error during sign-in process";
                    return View();
                }
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

    }
}
