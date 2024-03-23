using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using AMaz.Common;

namespace AMaz.Service
{
    public partial class AcademicYearService : IAcademicYearService
    {
        private readonly IAcademicYearReponsitory _academicYearRepository;
        private readonly IMapper _mapper;

        public AcademicYearService(IAcademicYearReponsitory academicYearRepository, IMapper mapper)
        {
            _academicYearRepository = academicYearRepository;
            _mapper = mapper;
        }

        public async Task<(bool succeed, string errorMsg)> CreateAcademicYearAsync(CreateAcademicYearRequest model)
        {
            var existingAcademicYears = await _academicYearRepository.GetAllAcademicYearsAsync();
            foreach (var existingAcademicYear in existingAcademicYears)
            {
                if ((model.DateTimeFrom >= existingAcademicYear.DateTimeFrom && model.DateTimeFrom <= existingAcademicYear.DateTimeTo) ||
                    (model.DateTimeTo >= existingAcademicYear.DateTimeFrom && model.DateTimeTo <= existingAcademicYear.DateTimeTo))
                {
                    return (false, "No");
                }
            }

            var academicYear = _mapper.Map<AcademicYear>(model);
            var result = await _academicYearRepository.CreateAcademicYearAsync(academicYear);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> UpdateAcademicYearAsync(UpdateAcademicYearRequest model)
        {
            var academicYear = _mapper.Map<AcademicYear>(model);
            await _academicYearRepository.UpdateAcademicYearAsync(academicYear);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> DeleteAcademicYearAsync(string id)
        {
            await _academicYearRepository.DeleteAcademicYearAsync(id);
            return (true, string.Empty);
        }

        public async Task<IEnumerable<AcademicYearViewModel>> GetAllAcademicYearsAsync()
        {
            var result = await _academicYearRepository.GetAllAcademicYearsAsync();
            return _mapper.Map<IEnumerable<AcademicYearViewModel>>(result);
        }

        public async Task<AcademicYearViewModel> GetAcademicYearByIdAsync(string id)
        {
            var result = await _academicYearRepository.GetAcademicYearByIdAsync(id);
            return _mapper.Map<AcademicYearViewModel>(result);
        }
    }
}
