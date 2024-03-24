using AMaz.Common;
using AMaz.Entity;
using AMaz.Models;
using AMaz.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using File = AMaz.Entity.File;

namespace AMaz.Service
{
    public class ContributionService : IContributionService
    {
        private readonly UserManager<User> _userManager;
        private readonly IFileRepository _fileRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public ContributionService(IContributionRepository contributionRepository, IMapper mapper, UserManager<User> userManager, FileService fileService, IFileRepository fileRepository)
        {
            _contributionRepository = contributionRepository;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<ContributionViewModel>> GetAllContributionsAsync()
        {
            var contributions = await _contributionRepository.GetAllContributionsAsync();
            return _mapper.Map<IEnumerable<ContributionViewModel>>(contributions);
        }

        public async Task<ContributionViewModel> GetContributionByIdAsync(string id)
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

            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
            {
                throw new ArgumentException("Invalid user id.", nameof(request.UserId));
            }

            var contribution = _mapper.Map<Contribution>(request);
            contribution.User = user;
            contribution.Status = (int)ContributionStatus.Pending;
            contribution.AuthorName = user.UserName;
            contribution.IsAcceptedTerms = true;
            
            bool createContributionResult = await _contributionRepository.CreateContributionAsync(contribution);

            
            if (!createContributionResult)
            {
                throw new Exception("Failed to create contribution.");
            }
            
            var createMutipleFileRequest = new CreateMultipleFileRequest
            {
                ContributionId = contribution.ContributionId.ToString(),
                Files = request.Files
            };

            var fileIds = await _fileService.SaveMultipleFilesAsync(createMutipleFileRequest);
            var files = await _fileRepository.GetAllFileByIdAsync(fileIds.ToList());
            contribution.Files = files;

            await _contributionRepository.UpdateContributionAsync(contribution);
            return true;
        }

        public async Task<bool> DeleteContributionAsync(string id)
        {
            var contribution = await _contributionRepository.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return false;
            }
            await _fileService.DeleteFiles(contribution.Files.Select(f => f.FileId.ToString()).ToList());
            // Check if the id is valid
            if (id == string.Empty)
            {
                throw new ArgumentException("Invalid contribution id.", nameof(id));
            }
            return await _contributionRepository.DeleteContributionAsync(id);
        }

        public async Task<bool> UpdateContributionAsync(UpdateContributionRequest request)
        {
            try
            {
                var contribution = _mapper.Map<Contribution>(request);
                return await _contributionRepository.UpdateContributionAsync(contribution);
            }
            catch
            {
                return false;
            }
        }
    }
}
