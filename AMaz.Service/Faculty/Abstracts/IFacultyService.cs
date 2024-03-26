using AMaz.Common;

namespace AMaz.Service
{
    public interface IFacultyService
    {
        Task<bool> CreateFacultyAsync(CreateFacultyRequest request);
        Task<bool> DeleteFacultyAsync(string id);
        Task<IEnumerable<FacultyViewModel>> GetAllFacultiesAsync();
        Task<FacultyViewModel> GetFacultyByIdAsync(string id);
        Task<bool> UpdateFacultyAsync(UpdateFacultyRequest request);
    }
}