using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly AMazDbContext _context;
        public FacultyRepository(AMazDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty> GetFacultyByIdAsync(string id)
        {
            return await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyId == Guid.Parse(id));
        }

        public async Task<bool> CreateFacultyAsync(Faculty faculty)
        {
            try
            {
                _context.Faculties.Add(faculty);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateFacultyAsync(Faculty faculty)
        {
            try
            {
                _context.Faculties.Update(faculty);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFacultyAsync(string id)
        {
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyId == Guid.Parse(id));
                _context.Faculties.Remove(faculty);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
