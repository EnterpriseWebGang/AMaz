using AMaz.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AMaz.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Authenticate user credentials
            bool isLogin = await _loginService.AuthenticateUserAsync(email, password);

            if (isLogin)
            {
                // Sign in the user after successful authentication
                bool signInResult = await _loginService.SignInUserAsync(email);

                if (signInResult)
                {
                    // Redirect to home page after successful sign-in
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Display an error message if there's an issue during the sign-in process
                    ViewBag.Error = "Error during sign-in process";
                    return View();
                }
            }

            // Display error message for invalid email or password
            ViewBag.Error = "Invalid email or password";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Sign out the user and redirect to home page
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
