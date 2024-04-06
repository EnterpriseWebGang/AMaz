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
            CreateMap<AcademicYear, AcademicYearViewModel>()
                .ForMember(dest => dest.DateTimeTo, opt => opt.MapFrom(src => src.DateTimeTo.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.DateTimeFrom, opt => opt.MapFrom(src => src.DateTimeFrom.ToString("MM/dd/yyyy")));
            CreateMap<UpdateAcademicYearRequest, AcademicYear>()
                .ForMember(m => m.AcademicYearId, option => option.MapFrom(src => Guid.Parse(src.AcademicYearId)));
            CreateMap<UpdateAcademicYearViewModel, UpdateAcademicYearRequest>();
            CreateMap<AcademicYearViewModel, UpdateAcademicYearViewModel>();
        }
    }
}
