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
        //private readonly ILogger<LoginModel> _logger;

        //private static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";


        public LoginService(SignInManager<User> signInManager)
        {
           _signInManager = signInManager;
        }

        // Asynchronously authenticates a user based on email and password
        //public async Task<AuthenticateResponse> AuthenticateUserAsync(AuthenticateRequest model)
        //{
        //    // Retrieve user from the repository based on the provided email
        //    User user = await _loginRepository.GetUserByEmailAsync(model.Email);

        //    // Check if the user exists and the provided password is correct
        //    if (user == null || !Encrypt(model.Password, user.Password))
        //    {
        //        throw new AppException("Email or password is incorrect");
        //    }


        //    // Check if the provided password matches the stored password hash
        //    var response = _mapper.Map<AuthenticateResponse>(user);
        //    return response;


        //}

        // Asynchronously signs in a user based on their email
        public async Task<SignInResult> SignInUserAsync(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
            return result;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        //private static bool Encrypt(string providedPassword, string storedPasswordHash)
        //{
        //    /*using var md5 = new MD5CryptoServiceProvider();
        //    using var tdes = new TripleDESCryptoServiceProvider();
        //    tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
        //    tdes.Mode = CipherMode.ECB;
        //    tdes.Padding = PaddingMode.PKCS7;

        //    using (var transform = tdes.CreateEncryptor())
        //    {
        //        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        //        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
        //        return Convert.ToBase64String(bytes, 0, bytes.Length);
        //    }
        //    neu muon dung Bcrpt thi dung thang nay man doan code tren di */


        //    return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
        //}





    }
}
