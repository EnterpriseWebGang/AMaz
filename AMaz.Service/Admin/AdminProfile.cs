using AMaz.Common;
using AutoMapper;

namespace AMaz.Service
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<CreateAccountViewModel, CreateAccountRequest>().
                ForMember(r => r.Role, option => option.MapFrom(m => GetUserRole(m.Role)));
            CreateMap<ResetPasswordViewModel, ResetPasswordRequest>();
            CreateMap<ChangeUserRoleViewModel, ChangeUserRoleRequest>().
                ForMember(r => r.Role, option => option.MapFrom(m => GetUserRole(m.Role)));
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
