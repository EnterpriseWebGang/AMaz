using AMaz.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AMaz.Entity;

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
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            try
            {
                // Authenticate user credentials
                var response = await _loginService.AuthenticateUserAsync(model);

                // Sign in the user after successful authentication
                await _loginService.SignInUserAsync(response.Email);

                // Redirect to home page after successful sign-in
                return RedirectToAction("Index", "Home");
            }
            catch (AppException ex)
            {
                // Display an error message for invalid email or password
                ViewBag.Error = ex.Message;
                return View();
            }
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
