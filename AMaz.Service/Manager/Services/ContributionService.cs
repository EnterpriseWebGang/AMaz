using AMaz.Common;
using AMaz.Entity;
using AMaz.Repo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public class ContributionService : IContributionService
    {
        private readonly IContributionRepository _contributionRepository;
        private readonly IMapper _mapper;

        public ContributionService(IContributionRepository contributionRepository, IMapper mapper)
        {
            _contributionRepository = contributionRepository ?? throw new ArgumentNullException(nameof(contributionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ContributionViewModel>> GetAllContributionsAsync()
        {
            var contributions = await _contributionRepository.GetAllContributionsAsync();
            return _mapper.Map<IEnumerable<ContributionViewModel>>(contributions);
        }

        public async Task<ContributionViewModel> GetContributionByIdAsync(Guid id)
        {
            var contribution = await _contributionRepository.GetContributionByIdAsync(id);
            return _mapper.Map<ContributionViewModel>(contribution);
        }

        public async Task<bool> CreateContributionAsync(CreateContributionRequest request)
        {
            // Check if the request is null
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Contribution request cannot be null.");
            }

            // Perform additional validation on the request properties if needed
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(request.Title));
            }



            var contribution = _mapper.Map<Contribution>(request);
            return await _contributionRepository.CreateContributionAsync(contribution);
        }

        public async Task<bool> DeleteContributionAsync(Guid id)
        {
            // Check if the id is valid
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid contribution id.", nameof(id));
            }
            return await _contributionRepository.DeleteContributionAsync(id);
        }

    }
}
