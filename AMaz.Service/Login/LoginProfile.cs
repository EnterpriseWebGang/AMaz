using AMaz.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, LoginRequest>().ReverseMap();
        }
    }
}
