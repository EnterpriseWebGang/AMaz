using AMaz.Common;
using AMaz.DB;
using AMaz.Entity;
using AMaz.Repo;
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
        private readonly AMazDbContext _dbContext;

        public UserService(IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IFacultyRepository facultyRepository,
            AMazDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _emailService = emailService;
            _facultyRepository = facultyRepository;
            _dbContext = dbContext;
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

        public async Task<UserViewModel> GetUserDetailByEmailAsync(string email)
        {
            var entity = await _userManager.Users.FirstOrDefaultAsync(c => c.Email == email);
            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<UserViewModel>(entity);
        }

        public async Task<string> GetUserFacultyId(string userId)
        {
            var user = await _userManager.Users.Include(u => u.Faculty).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            return user.Faculty.FacultyId.ToString();
        }

        public async Task<UserViewModel>GetCoordinatorEmailByFaculty(string facultyId)
        {
            var coordinatorQuery =  from users in _dbContext.Users.Include(u => u.Faculty)
                                    join faculties in _dbContext.Faculties 
                                        on users.Faculty.FacultyId equals faculties.FacultyId
                                    join userRoles in _dbContext.UserRoles 
                                        on users.Id equals userRoles.UserId
                                    join roles in _dbContext.Roles 
                                        on userRoles.RoleId equals roles.Id
                                    where roles.Name == "Coordinator" && faculties.FacultyId.ToString() == facultyId
                                    select users;

            var coordinators = await coordinatorQuery.FirstOrDefaultAsync();
            if (coordinators == null)
            {
                return null;
            }

            var models = _mapper.Map<UserViewModel>(coordinators);
            return models;
        }

        public async Task<bool> ValidateIfUserIsInFaculty(string userId, string magazineId)
        {
            var magazine = await _dbContext.Magazines.Include(m => m.Faculty).FirstOrDefaultAsync(m => m.MagazineId.ToString() == magazineId);
            var query = from users in _dbContext.Users.Include(u => u.Faculty)
                        join faculties in _dbContext.Faculties
                            on users.Faculty.FacultyId equals faculties.FacultyId
                        join Magazine in _dbContext.Magazines
                            on faculties.FacultyId equals Magazine.Faculty.FacultyId
                        where users.Id == userId && faculties.FacultyId.ToString() == magazine.Faculty.FacultyId.ToString()
                        select users;

            var user = await query.CountAsync();
            if (user == 0)
            {
                return false;
            }

            return true;
        }
    }
}
