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

        // Asynchronously authenticates a user based on email and password
        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            // Retrieve user from the repository based on the provided email
            User user = await _loginRepository.GetUserByEmailAsync(email);

            // Check if the user exists and the provided password is correct
            if (user != null && user.Password == password)
            {
                return true; // Return true if authentication is successful
            }

            return false; // Return false if authentication fails
        }

        // Asynchronously signs in a user based on their email
        public async Task<bool> SignInUserAsync(string email)
        {
            // Retrieve user from the repository based on the provided email
            User user = await _loginRepository.GetUserByEmailAsync(email);

            // Check if the user exists
            if (user != null)
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                };

                // Create an identity and principal based on the claims
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user using the authentication scheme and principal
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true; // Return true for a successful sign-in
            }

            return false; // Return false if the user is not found
        }
    }
}
