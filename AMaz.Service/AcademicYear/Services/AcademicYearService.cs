using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using AMaz.Common;
using Azure.Core;

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

        public async Task<(bool succeed, string errorMsg)> CreateAcademicYearAsync(CreateAcademicYearRequest request)
        {
            if (!(request.DateTimeTo > request.DateTimeFrom)) // check if the end date is greater than the start date
            {
                return (false, "End date must be greater than start date!");
            }

            request.DateTimeTo = request.DateTimeTo.AddHours(23).AddMinutes(59).AddSeconds(59); // set the end date to the end of the day

            var existingAcademicYears = await _academicYearRepository.GetAllAcademicYearsAsync();
            foreach (var existingAcademicYear in existingAcademicYears)
            {
                if ((request.DateTimeFrom >= existingAcademicYear.DateTimeFrom && request.DateTimeFrom <= existingAcademicYear.DateTimeTo) ||
                    (request.DateTimeTo >= existingAcademicYear.DateTimeFrom && request.DateTimeTo <= existingAcademicYear.DateTimeTo))
                {
                    return (false, "Cannot create because it overlaped with other academic year!");
                }
            }

            var academicYear = _mapper.Map<AcademicYear>(request);
            await _academicYearRepository.CreateAcademicYearAsync(academicYear);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> UpdateAcademicYearAsync(UpdateAcademicYearRequest request)
        {
            var academicYear = await _academicYearRepository.GetAcademicYearByIdAsync(request.AcademicYearId);
            if (academicYear == null)
            {
                return (false, "Academic year not found!");
            }
            if (!(request.DateTimeTo > request.DateTimeFrom)) // check if the end date is greater than the start date
            {
                return (false, "End date must be greater than start date!");
            }

            request.DateTimeTo = request.DateTimeTo.AddHours(23).AddMinutes(59).AddSeconds(59); // set the end date to the end of the day

            var allAcademicYear = await _academicYearRepository.GetAllAcademicYearsAsync();
            var existingAcademicYears = allAcademicYear.Where(a => a.AcademicYearId.ToString() != request.AcademicYearId);
            foreach (var existingAcademicYear in existingAcademicYears)
            {
                if ((request.DateTimeFrom >= existingAcademicYear.DateTimeFrom && request.DateTimeFrom <= existingAcademicYear.DateTimeTo) ||
                    (request.DateTimeTo >= existingAcademicYear.DateTimeFrom && request.DateTimeTo <= existingAcademicYear.DateTimeTo))
                {
                    return (false, "Cannot update because it overlaped with other academic year!");
                }
            }
            academicYear.DateTimeFrom = request.DateTimeFrom;
            academicYear.DateTimeTo = request.DateTimeTo;
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
