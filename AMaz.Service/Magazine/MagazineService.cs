using AMaz.Common;
using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;

namespace AMaz.Service
{
    public class MagazineService : IMagazineService
    {
        private readonly IMagazineRepository _mangazineRepository;
        private readonly IAcademicYearReponsitory _academicYearReponsitory;
        private readonly IMapper _mapper;
        public MagazineService(IMagazineRepository mangazineRepository, IMapper mapper, IAcademicYearReponsitory academicYearReponsitory)
        {
            _mangazineRepository = mangazineRepository;
            _mapper = mapper;
            _academicYearReponsitory = academicYearReponsitory;
        }

        public async Task<List<MagazineViewModel>> GetAllMagazines()
        {
            var data = await _mangazineRepository.GetAllMagazinesAsync();
            return _mapper.Map<List<MagazineViewModel>>(data);
        }

        public async Task<List<MagazineViewModel>> GetAllMagazineByFacultyId(string facultyId)
        {
            var data = await _mangazineRepository.GetAllMagazineByFaculty(facultyId);
            return _mapper.Map<List<MagazineViewModel>>(data);
        }

        public async Task<MagazineDetailViewModel> GetMagazineByIdAsync(string magazineId)
        {
            var data = await _mangazineRepository.GetMagazineByIdAsync(magazineId);
            return _mapper.Map<MagazineDetailViewModel>(data);
        }

        public async Task<(bool succeed, string errorMsg)> CreateMagazineAsync(CreateMagazineRequest request)
        {
            var magazine = _mapper.Map<Magazine>(request);
            if(request.FirstClosureDate > request.FinalClosureDate)
            {
                return (false, "Final closure date must be greater than first closure date!");
            }

            var latestAcademicYear = await _academicYearReponsitory.GetLatestAcademicYearAsync();
            if (latestAcademicYear == null)
            {
                return (false, "There is no academic year available!");
            }
            magazine.AcademicYear = latestAcademicYear;
            await _mangazineRepository.CreateMagazineAsync(magazine);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> UpdateMagazineAsync(UpdateMagazineRequest request)
        {
            var magazine = await _mangazineRepository.GetMagazineByIdAsync(request.MagazineId);
            if (magazine == null)
            {
                return (false, "Magazine not found!");
            }
            if (request.FirstClosureDate > request.FinalClosureDate)
            {
                return (false, "Final closure date must be greater than first closure date!");
            }
            magazine.FirstClosureDate = request.FirstClosureDate;
            magazine.FinalClosureDate = request.FinalClosureDate;
            magazine.Name = request.Name;
            magazine.AcademicYear = await _academicYearReponsitory.GetAcademicYearByIdAsync(request.AcademicYearId);
            await _mangazineRepository.UpdateMagazineAsync(magazine);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> DeleteMagazineAsync(string magazineId)
        {
            var magazine = await _mangazineRepository.GetMagazineByIdAsync(magazineId);
            if (magazine == null)
            {
                return (false, "Magazine not found!");
            }
            await _mangazineRepository.DeleteMagazineAsync(magazine);
            return (true, string.Empty);
        }
    }
}
