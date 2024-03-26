using AMaz.Entity;

namespace AMaz.Repo
{
    public interface IFacultyRepository
    {
        Task<bool> CreateFacultyAsync(Faculty faculty);
        Task<bool> DeleteFacultyAsync(string id);
        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();
        Task<Faculty> GetFacultyByIdAsync(string id);
        Task<bool> UpdateFacultyAsync(Faculty faculty);
    }
}