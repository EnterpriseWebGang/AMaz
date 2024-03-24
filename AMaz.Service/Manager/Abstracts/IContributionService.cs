using AMaz.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public interface IContributionService
    {
        Task<IEnumerable<ContributionViewModel>> GetAllContributionsAsync();
        Task<ContributionViewModel> GetContributionByIdAsync(string id);
        Task<bool> CreateContributionAsync(CreateContributionRequest request);
        Task<bool> DeleteContributionAsync(string id);
        Task<bool> UpdateContributionAsync(UpdateContributionRequest request);

    }
}
