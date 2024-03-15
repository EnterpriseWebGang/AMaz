using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = AMaz.Entity.File;

namespace AMaz.Repo
{
    public class FileRepository : IFileRepository
    {
        public AMazDbContext _context;
        public FileRepository(AMazDbContext context)
        {
            _context = context;
        }

        public async Task<List<File>> GetAllFilesAsync()
        {
            var result = await _context.Files.ToListAsync();
            return result;
        }

        public async Task<File> GetFileByIdAsync(string id)
        {
            var result = await _context.Files.Where(f => f.FileId.ToString() == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<File>> GetAllFilesByContributionId(string contributionId)
        {
            var result = await _context.Files.Where(f => f.Contribution.ContributionId.ToString() == contributionId).ToListAsync();
            return result;
        }

        public async Task<bool> SaveMutipleFiles(List<File> files)
        {
            try
            {
                _context.Files.AddRange(files);
                await _context.SaveChangesAsync();
            }

            catch (Exception)
            {
                return false;
            }

            return true;

        }

        public async Task<bool> SaveFile(File file)
        {
            try
            {
                _context.Files.Add(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<List<File>> GetFilesByNameAsync(string name)
        {
            var query = _context.Files.Where(file => file.Name == name);
            return await query.ToListAsync();
        }

        public async Task<File> GetFileById(string id)
        {
            return await _context.Files.Where(file => file.FileId.ToString() == id).FirstOrDefaultAsync();
        }
    }
}
