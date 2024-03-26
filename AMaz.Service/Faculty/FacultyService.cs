using AMaz.Common;
using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;

namespace AMaz.Service
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;
        public FacultyService(IFacultyRepository facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<FacultyViewModel>> GetAllFacultiesAsync()
        {
            var faculties = await _facultyRepository.GetAllFacultiesAsync();
            return faculties.Select(f => new FacultyViewModel
            {
                FacultyId = f.FacultyId.ToString(),
                Name = f.Name
            });
        }

        public async Task<FacultyViewModel> GetFacultyByIdAsync(string id)
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(id);
            return new FacultyViewModel
            {
                FacultyId = faculty.FacultyId.ToString(),
                Name = faculty.Name
            };
        }

        public async Task<bool> CreateFacultyAsync(CreateFacultyRequest request)
        {
            var faculty = _mapper.Map<Faculty>(request);
            return await _facultyRepository.CreateFacultyAsync(faculty);
        }

        public async Task<bool> UpdateFacultyAsync(UpdateFacultyRequest request)
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(request.FacultyId);
            if (faculty == null)
            {
                return false;
            }
            
            faculty.Name = request.Name;
            return await _facultyRepository.UpdateFacultyAsync(faculty);
        }

        public async Task<bool> DeleteFacultyAsync(string id)
        {
            return await _facultyRepository.DeleteFacultyAsync(id);
        }
    }
}
