using AMaz.Entity;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Repo
{
    public interface IMagazineRepository
    {
        Task<bool> CreateMagazineAsync(Magazine magazine);
        Task<bool> DeleteMagazineAsync(Magazine magazine);
        Task<List<Magazine>> GetAllMagazineByFaculty(string facultyId);
        Task<List<Magazine>> GetAllMagazinesAsync(Func<DbSet<Magazine>, IQueryable<Magazine>> filter = null);
        Task<Magazine> GetMagazineByIdAsync(string id);
        Task<bool> UpdateMagazineAsync(Magazine magazine);
    }
}