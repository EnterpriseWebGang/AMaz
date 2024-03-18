using AMaz.Common;
using AutoMapper;

namespace AMaz.Service
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<CreateAccountViewModel, CreateAccountRequest>();
        }
    }
}
