using AMaz.Common;
using AMaz.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public interface IContributionService
    {
        Task<IEnumerable<ContributionViewModel>> GetAllContributionsAsync();
        Task<ContributionDetailViewModel> GetContributionByIdAsync(string id);
        Task<bool> CreateContributionAsync(CreateContributionRequest request, Func<Contribution, UserViewModel, Task> sendingEmailCallBack);
        Task<bool> DeleteContributionAsync(string id);
        Task<bool> UpdateContributionAsync(UpdateContributionRequest request, Func<Contribution, UserViewModel, Task> sendingEmailCallBack);

    }
}
