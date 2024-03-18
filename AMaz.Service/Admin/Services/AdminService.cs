using AMaz.Common;
using AMaz.Entity;
using Microsoft.AspNetCore.Identity;

namespace AMaz.Service
{
    public partial class UserService
    {
        public async Task<IdentityResult> CreateAccount(CreateAccountRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Role);

            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Role does not exist"
                });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                var forDbCreate = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    Email = request.Email,
                };

                var createUserResult = await _userManager.CreateAsync(forDbCreate, request.Password);
                var addUserToRoleResult = IdentityResult.Failed();
                if (true)
                {
                   addUserToRoleResult = await _userManager.AddToRoleAsync(forDbCreate, request.Role);
                }

                if (createUserResult.Succeeded)
                {
                    if (addUserToRoleResult.Succeeded)
                    {
                        return addUserToRoleResult;
                    }

                    return IdentityResult.Failed(createUserResult.Errors.Concat(addUserToRoleResult.Errors).ToArray());
                }

                return IdentityResult.Failed(createUserResult.Errors.Concat(addUserToRoleResult.Errors).ToArray());
            }
            else
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User Already Exist"
                });
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User do not exist"
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, request.Password);
        }

    }

}
