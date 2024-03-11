using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public partial class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";


        public LoginService(ILoginRepository loginRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // Asynchronously authenticates a user based on email and password
        public async Task<AuthenticateResponse> AuthenticateUserAsync(AuthenticateRequest model)
        {
            // Retrieve user from the repository based on the provided email
            User user = await _loginRepository.GetUserByEmailAsync(model.Email);

            // Check if the user exists and the provided password is correct
            if (user == null || !Encrypt(model.Password, user.Password))
            {
                throw new AppException("Email or password is incorrect");
            }


            // Check if the provided password matches the stored password hash
            var response = _mapper.Map<AuthenticateResponse>(user);
            return response;


        }
        // Asynchronously signs in a user based on their email
        public async Task SignInUserAsync(string email)
        {
            /*  // Retrieve user from the repository based on the provided email
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

              return false; // Return false if the user is not found*/
            User user = await _loginRepository.GetUserByEmailAsync(email);

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
            }
            else
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

        }
        private static bool Encrypt(string providedPassword, string storedPasswordHash)
        {
            /*using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using (var transform = tdes.CreateEncryptor())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            neu muon dung Bcrpt thi dung thang nay man doan code tren di */


            return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
        }





    }
}
