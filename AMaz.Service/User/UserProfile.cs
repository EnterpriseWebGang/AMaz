using AMaz.Common;
using AMaz.Entity;
using AutoMapper;

namespace AMaz.Service
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>().ForMember(u => u.IsActive, opt => opt.MapFrom(src => GetIsActiveStatus(src.IsActive)));
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

        private string GetIsActiveStatus(bool isActive)
        {
            return isActive ? "Yes" : "No";
        }
    }
}
