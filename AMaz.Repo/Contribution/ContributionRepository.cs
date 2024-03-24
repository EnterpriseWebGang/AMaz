using AMaz.DB;
using AMaz.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly AMazDbContext _dbContext;

        public ContributionRepository(AMazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Contribution>> GetAllContributionsAsync()
        {
            return await _dbContext.Contributions.Include(c => c.Files).ToListAsync();
        }

        public async Task<Contribution> GetContributionByIdAsync(string id)
        {
            return await _dbContext.Contributions.Include(c => c.Files).FirstOrDefaultAsync(c => c.ContributionId == Guid.Parse(id));
        }

        public async Task<bool> CreateContributionAsync(Contribution contribution)
        {
            try
            {
                contribution.Magazine = _dbContext.Magazines.Find(Guid.Parse("7c266e6f-7830-4feb-bc87-35d609d06801"));
                _dbContext.Contributions.Add(contribution);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateContributionAsync(Contribution contribution)
        {
            try
            {
                _dbContext.Contributions.Update(contribution);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public async Task<bool> DeleteContributionAsync(string id)
        {
            var contribution = await _dbContext.Contributions.FindAsync(Guid.Parse(id));
            if (contribution == null)
                return false;

            _dbContext.Contributions.Remove(contribution);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
