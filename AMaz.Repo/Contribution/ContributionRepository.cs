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
            return await _dbContext.Contributions.ToListAsync();
        }

        public async Task<Contribution> GetContributionByIdAsync(Guid id)
        {
            return await _dbContext.Contributions.FindAsync(id);
        }

        public async Task<bool> CreateContributionAsync(Contribution contribution)
        {
            try
            {
                _dbContext.Contributions.Add(contribution);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteContributionAsync(Guid id)
        {
            var contribution = await _dbContext.Contributions.FindAsync(id);
            if (contribution == null)
                return false;

            _dbContext.Contributions.Remove(contribution);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
