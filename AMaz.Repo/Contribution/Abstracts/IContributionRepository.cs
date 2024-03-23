using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public interface IContributionRepository
    {
        Task<IEnumerable<Contribution>> GetAllContributionsAsync();
        Task<Contribution> GetContributionByIdAsync(Guid id);
        Task<bool> CreateContributionAsync(Contribution contribution);
        Task<bool> DeleteContributionAsync(Guid id);
    }
}
