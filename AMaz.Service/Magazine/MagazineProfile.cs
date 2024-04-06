using AutoMapper;
using AMaz.Entity;
using AMaz.Common;

namespace AMaz.Service
{
    public class MagazineProfile : Profile  
    {
        public MagazineProfile()
        {
            CreateMap<CreateMagazineRequest, Magazine>();
            CreateMap<CreateMagazineViewModel, CreateMagazineRequest>();
            CreateMap<Magazine, MagazineViewModel>();
            CreateMap<UpdateMagazineRequest, Magazine>()
                .ForMember(m => m.MagazineId, option => option.MapFrom(src => Guid.Parse(src.MagazineId)));
            CreateMap<UpdateMagazineViewModel, UpdateMagazineRequest>();
            CreateMap<MagazineViewModel, UpdateMagazineViewModel>();
            CreateMap<Magazine, UpdateMagazineViewModel>().
                ForMember(dest => dest.AcademicYearId, option => option.MapFrom(src => src.AcademicYear.AcademicYearId.ToString() ?? "")).
                ForMember(dest => dest.FacultyId, option => option.MapFrom(src => src.Faculty.FacultyId.ToString() ?? ""));
            CreateMap<Magazine, MagazineDetailViewModel>()
                .ForMember(m => m.FacultyName, option => option.MapFrom(src => src.Faculty.Name ?? ""))
                .ForMember(m => m.AcademicYear, option => option.MapFrom(src => GetAcademicYear(src.AcademicYear)))
                .ForMember(m => m.Contributions, option => option.Ignore());
        }

        private string GetAcademicYear(AcademicYear academicYear)
        {
            if (academicYear == null)
            {
                return "";
            }
            return $"{academicYear?.DateTimeFrom.ToString("yyyy/MM/dd")} - {academicYear?.DateTimeTo.ToString("yyyy/MM/dd")}";
        }
    }
}
