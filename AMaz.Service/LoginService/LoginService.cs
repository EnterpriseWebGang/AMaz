using AMaz.Entity;
using AMaz.Repo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(ILoginRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _loginRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            User user = await _loginRepository.GetUserByEmailAsync(email);

            if (user != null && user.Password == password)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> SignInUserAsync(string email)
        {
            User user = await _loginRepository.GetUserByEmailAsync(email);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true; // Return true for a successful sign-in
            }

            return false; // Return false if the user is not found
        }
    }
}
