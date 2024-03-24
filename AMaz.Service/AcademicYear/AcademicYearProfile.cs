using AMaz.Common;
using AMaz.Entity;
using AutoMapper;

namespace AMaz.Service
{
    public class AcademicYearProfile : Profile
    {
        public AcademicYearProfile()
        {
            CreateMap<CreateAcademicYearRequest, AcademicYear>();
            CreateMap<CreateAcademicYearViewModel, CreateAcademicYearRequest>();
            CreateMap<AcademicYear, AcademicYearViewModel>();
            CreateMap<UpdateAcademicYearRequest, AcademicYear>()
                .ForMember(m => m.AcademicYearId, option => option.MapFrom(src => Guid.Parse(src.AcademicYearId)));
            CreateMap<UpdateAcademicYearViewModel, AcademicYear>()
                .ForMember(m => m.AcademicYearId, option => option.MapFrom(src => Guid.Parse(src.AcademicYearId)));
            CreateMap<UpdateAcademicYearViewModel, UpdateAcademicYearRequest>();
            CreateMap<AcademicYearViewModel, UpdateAcademicYearViewModel>();
        }
    }
}
