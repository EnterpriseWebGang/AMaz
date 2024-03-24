﻿using AMaz.Common;
using AMaz.Entity;
using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Service
{
    public partial class UserService
    {
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment evironment;
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var users = await _userManager.Users.Where(u => u.Id != currentUser.Id).ToListAsync();
            var result = users.Select(c =>
            {
                var model = _mapper.Map<UserViewModel>(c);
                model.Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray());
                return model;

            }).ToList();

            return result;
        }

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
                   
                    //TODO: Send Email to User

                    if (addUserToRoleResult.Succeeded)
                    {
                        await _emailService.SendCreateAccountEmail(request);
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
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (result.Succeeded)
            {
                //TODO: Send Email to user
            }

            return result;
        }

        public async Task<(bool result, string errorMessage)> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "user does not exist");
            }

            if (!user.IsActive)
            {
                return (false, "user is already deactivated");
            }

            user.IsActive = false;

            await _userManager.UpdateAsync(user);

            return (true, "");
        }

        public async Task<(bool result, string errorMessage)> ActivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "user does not exist");
            }

            if (user.IsActive)
            {
                return (false, "user is already activated");
            }

            user.IsActive = true;

            await _userManager.UpdateAsync(user);

            return (true, "");
        }

        public async Task<(bool result, string errorMessage)> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "user does not exist");
            }

            await _userManager.DeleteAsync(user);

            return (true, "");
        }

    }

}
