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

        public UserService(IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            Repo.IFacultyRepository facultyRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _emailService = emailService;
            _facultyRepository = facultyRepository;
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
        public async Task<string> GetCoordinatorEmailsByFaculty(string facultyId)
        {
            var facultyUsers = await _userManager.Users.Include(user => user.Faculty)
                .Where(user => user.Faculty.FacultyId.ToString() == facultyId)
                .ToListAsync();
            
            var coordinators = await _userManager.GetUsersInRoleAsync(Role.Coordinator.ToString());
            var coordinatorUsers = facultyUsers.Where(user => coordinators.Any(coordinator => coordinator.Id == user.Id));
            var coordinatorEmails = coordinatorUsers?.Select(u => u.Email).FirstOrDefault();
            
            return coordinatorEmails;
        }
    }
}
