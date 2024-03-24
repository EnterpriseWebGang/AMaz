using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;

namespace AMaz.Repo
{
    public class AcademicYearReponsitory : IAcademicYearReponsitory
    {
        private readonly AMazDbContext _db;

        public AcademicYearReponsitory(AMazDbContext db)
        {
            _db = db;
        }

        public async Task CreateAcademicYearAsync(AcademicYear model)
        {
            await _db.AcademicYears.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAcademicYearAsync(AcademicYear model)
        {
            _db.AcademicYears.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAcademicYearAsync(string id)
        {
            AcademicYear academicYear = await _db.AcademicYears.Where(a => a.AcademicYearId == Guid.Parse(id)).FirstOrDefaultAsync();
            _db.AcademicYears.Remove(academicYear);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync() 
        { 
            return await _db.AcademicYears.ToListAsync(); 
        }

        public async Task<AcademicYear> GetAcademicYearByIdAsync(string id)
        {
            return await _db.AcademicYears.Where(a => a.AcademicYearId == Guid.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<AcademicYear> GetLatestAcademicYearAsync()
        {
            return await _db.AcademicYears.OrderByDescending(a => a.DateTimeFrom).FirstOrDefaultAsync();
        }
    }
}
