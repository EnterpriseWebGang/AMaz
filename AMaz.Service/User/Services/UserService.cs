using AMaz.Common;
using AMaz.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public partial class UserService
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IMapper mapper, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var users = await _userManager.Users.Where(u => u.Id != currentUser.Id)
                .Select(c => new UserViewModel
                {
                    Id = c.Id,
                    Email = c.Email,
                    IsActive = GetIsActiveStatus(c.IsActive),
                    Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
                }).ToListAsync();

            return users;
        }

        public async Task<UserViewModel> GetUserDetailByIdAsync(string id)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var entity = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == id && c.Id != currentUser.Id);
            if (entity == null || entity.Id == currentUser.Id)
            {
                return null;
            }

            return _mapper.Map<UserViewModel>(entity);
        }

        private string GetIsActiveStatus(bool isActive)
        {
            return isActive ? "Yes" : "No";
        }
    }
}
