using AMaz.Common;
using Microsoft.AspNetCore.Identity;

namespace AMaz.Service
{
    public interface ILoginService
    {
       // Task<AuthenticateResponse> AuthenticateUserAsync(AuthenticateRequest model);

        Task<SignInResult> SignInUserAsync(LoginRequest  email);

        Task LogOut();
    }
}
