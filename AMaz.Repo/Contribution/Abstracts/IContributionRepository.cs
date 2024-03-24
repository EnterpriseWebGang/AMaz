using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Repo
{
    public interface IContributionRepository
    {
        Task<IEnumerable<Contribution>> GetAllContributionsAsync();
        Task<Contribution> GetContributionByIdAsync(string id);
        Task<bool> CreateContributionAsync(Contribution contribution);
        Task<bool> DeleteContributionAsync(string id);
        Task<bool> UpdateContributionAsync(Contribution contribution);
    }
}
