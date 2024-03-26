using AMaz.Common;
using AMaz.Entity;
using AutoMapper;

namespace AMaz.Service
{
    public class FacultyProfile : Profile
    {
        public FacultyProfile()
        {
            CreateMap<CreateFacultyRequest, Faculty>();
            CreateMap<UpdateFacultyRequest, Faculty>();
            CreateMap<Faculty, FacultyViewModel>();
            CreateMap<CreateFacultyViewModel, CreateFacultyRequest>();
            CreateMap<UpdateFacultyViewModel, UpdateFacultyRequest>();
            CreateMap<FacultyViewModel, UpdateFacultyViewModel>();
        }
    }
}
