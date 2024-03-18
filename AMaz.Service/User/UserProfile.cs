using AMaz.Common;
using AMaz.Entity;
using AutoMapper;

namespace AMaz.Service
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
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
    }
}
