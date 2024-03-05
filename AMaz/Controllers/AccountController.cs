using AMaz.Service;
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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            bool isLogin = await _loginService.AuthenticateUserAsync(email, password);

            if (isLogin)
            {
                return RedirectToAction("ViewTest", "Home"); // Redirect to home page after successful login
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }
    }
}
