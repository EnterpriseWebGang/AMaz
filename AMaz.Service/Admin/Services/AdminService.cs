using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMaz.DB;
using AutoMapper;
using System.Security.Cryptography;
using AutoMapper.Execution;
using AMaz.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AMaz.Service
{
    public partial class AdminService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AdminService(
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAccount(CreateAccountRequest request)
        {
            var roleName = GetUserRole(request.Role);
            var role = await _roleManager.FindByNameAsync(roleName);

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

                string userPWD = request.Password;
                var createUserResult = await _userManager.CreateAsync(forDbCreate, userPWD);
                var addToRoleResult = await _userManager.AddToRoleAsync(forDbCreate, roleName);

                if (createUserResult.Succeeded)
                {
                    if (addToRoleResult.Succeeded)
                    {
                        return addToRoleResult;
                    }

                    return IdentityResult.Failed(createUserResult.Errors.Concat(addToRoleResult.Errors).ToArray());
                }

                return IdentityResult.Failed(createUserResult.Errors.Concat(addToRoleResult.Errors).ToArray());
            }
            else
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User Already Exist"
                });
            }
        }

        private string GetUserRole(int role)
        {
            return role switch
            {
                (int)Role.Admin => "Admin",
                (int)Role.Student => "Student",
                (int)Role.Coordinator => "Coordinator",
                (int)Role.Manager => "Manager",
                _ => ""
            };
        }


        //public bool AdminCheck()
        //{

        //}

        //public AuthenticateResponse Create(CreateRequest model)
        //{

        //}


        //public void DeleteAcount(Guid id)
        //{

        //}
    }

}
