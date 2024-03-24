using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Repo
{
    public class MagazineRepository : IMagazineRepository
    {
        private readonly AMazDbContext _dbContext;
        public MagazineRepository(AMazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Magazine>> GetAllMagazinesAsync(Func<DbSet<Magazine>, IQueryable<Magazine>> filter = null)
        {
            if (filter != null)
            {
                return await filter(_dbContext.Magazines).ToListAsync();
            }
            return await _dbContext.Magazines.ToListAsync();
        }

        public async Task<List<Magazine>> GetAllMagazineByFaculty(string facultyId)
        {
            return await _dbContext.Magazines.Include(m => m.Faculty).Where(m => m.Faculty.FacultyId == Guid.Parse(facultyId)).ToListAsync();
        }

        public async Task<Magazine> GetMagazineByIdAsync(string id)
        {
            return await _dbContext.Magazines.Include(m => m.Faculty).
                Include(m => m.AcademicYear).
                Include(m => m.Contributions).
                ThenInclude(c => c.User).
                FirstOrDefaultAsync(m => m.MagazineId == Guid.Parse(id));
        }

        public async Task<bool> CreateMagazineAsync(Magazine magazine)
        {
            _dbContext.Magazines.Add(magazine);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMagazineAsync(Magazine magazine)
        {
            _dbContext.Magazines.Update(magazine);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMagazineAsync(Magazine magazine)
        {
            _dbContext.Magazines.Remove(magazine);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
