using AMaz.Common;
using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using System.Runtime.CompilerServices;

namespace AMaz.Service
{
    public class MagazineService : IMagazineService
    {
        private readonly IMagazineRepository _mangazineRepository;
        private readonly IAcademicYearReponsitory _academicYearReponsitory;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;
        public MagazineService(IMagazineRepository mangazineRepository, IMapper mapper, IAcademicYearReponsitory academicYearReponsitory, IFacultyRepository facultyRepository)
        {
            _mangazineRepository = mangazineRepository;
            _mapper = mapper;
            _academicYearReponsitory = academicYearReponsitory;
            _facultyRepository = facultyRepository;
        }

        public async Task<List<MagazineViewModel>> GetAllMagazines()
        {
            var data = await _mangazineRepository.GetAllMagazinesAsync();
            var model = _mapper.Map<List<MagazineViewModel>>(data);
            return model;
        }

        public async Task<List<MagazineViewModel>> GetAllMagazineByFacultyId(string facultyId)
        {
            var data = await _mangazineRepository.GetAllMagazineByFaculty(facultyId);
            return _mapper.Map<List<MagazineViewModel>>(data);
        }

        public async Task<MagazineDetailViewModel> GetMagazineByIdAsync(string magazineId)
        {
            var data = await _mangazineRepository.GetMagazineByIdAsync(magazineId);
            var model = _mapper.Map<MagazineDetailViewModel>(data);
            model.Contributions = _mapper.Map<List<ContributionViewModel>>(data.Contributions);
            return model;
        }

        public async Task<UpdateMagazineViewModel> GetUpdateMagazineViewModelAsync(string magazineId)
        {
            var data = await _mangazineRepository.GetMagazineByIdAsync(magazineId);
            return _mapper.Map<UpdateMagazineViewModel>(data);
        }

        public async Task<(bool succeed, string errorMsg)> CreateMagazineAsync(CreateMagazineRequest request)
        {
            var magazine = _mapper.Map<Magazine>(request);
            if(request.FacultyId == null)
            {
                return (false, "Faculty is required!");
            }

            if(request.FirstClosureDate > request.FinalClosureDate)
            {
                return (false, "Final closure date must be greater than first closure date!");
            }

            var latestAcademicYear = await _academicYearReponsitory.GetLatestAcademicYearAsync();
            if (latestAcademicYear == null)
            {
                return (false, "There is no academic year available!");
            }

            // Check if the closure date is within the academic year
            if (request.FirstClosureDate < latestAcademicYear.DateTimeFrom || request.FinalClosureDate > latestAcademicYear.DateTimeTo)
            {
                return (false, "Closure date must be within the academic year!");
            }
           
            var faculty = await _facultyRepository.GetFacultyByIdAsync(request.FacultyId);
            if (faculty == null)
            {
                return (false, "Faculty not found!");
            }
            magazine.AcademicYear = latestAcademicYear;
            magazine.Faculty = faculty;
            await _mangazineRepository.CreateMagazineAsync(magazine);
            return (true, string.Empty);
        }

        public async Task<(bool succeed, string errorMsg)> UpdateMagazineAsync(UpdateMagazineRequest request)
        {
            if (request.FacultyId == null)
            {
                return (false, "Faculty is required!");
            }

            if (request.AcademicYearId == null)
            {
                return (false, "Academic year is required!");
            }

            var magazine = await _mangazineRepository.GetMagazineByIdAsync(request.MagazineId);
            if (magazine == null)
            {
                return (false, "Magazine not found!");
            }

            // Check if the first closure date is greater than the final closure date
            if (request.FirstClosureDate > request.FinalClosureDate)
            {
                return (false, "Final closure date must be greater than first closure date!");
            }
            magazine.FirstClosureDate = request.FirstClosureDate;
            magazine.FinalClosureDate = request.FinalClosureDate;
            magazine.Name = request.Name;

            var academicYear = await _academicYearReponsitory.GetAcademicYearByIdAsync(request.AcademicYearId);
            if (academicYear == null)
            {
                return (false, "There is no academic year available!");
            }

            // Check if the closure date is within the academic year
            if (request.FirstClosureDate < academicYear.DateTimeFrom || request.FinalClosureDate > academicYear.DateTimeTo)
            {
                return (false, "Closure date must be within the academic year!");
            }

            var faculty = await _facultyRepository.GetFacultyByIdAsync(request.FacultyId);
            if (faculty == null)
            {
                return (false, "Faculty not found!");
            }

            magazine.AcademicYear = academicYear;
            magazine.Faculty = faculty;
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
