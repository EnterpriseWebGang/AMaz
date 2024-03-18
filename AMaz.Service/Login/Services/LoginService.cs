using AMaz.Common;
using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace AMaz.Service
{
    public partial class LoginService : ILoginService
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> SignInUserAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            if (!user.IsActive)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
            return result;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }


    }
}
