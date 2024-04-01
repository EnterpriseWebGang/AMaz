using AMaz.Common;
using AMaz.Entity;
using AMaz.Models;
using AMaz.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMagazineRepository _magazineRepository;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public ContributionService(IContributionRepository contributionRepository, 
            IMapper mapper, 
            UserManager<User> userManager, 
            FileService fileService, 
            IFileRepository fileRepository,
            IMagazineRepository magazineRepository,
            UserService userService)
        {
            _contributionRepository = contributionRepository;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
            _fileRepository = fileRepository;
            _magazineRepository = magazineRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<ContributionViewModel>> GetAllContributionsAsync()
        {
            var contributions = await _contributionRepository.GetAllContributionsAsync();
            return _mapper.Map<IEnumerable<ContributionViewModel>>(contributions);
        }

        public async Task<ContributionDetailViewModel> GetContributionByIdAsync(string id)
        {
            var contribution = await _contributionRepository.GetContributionByIdAsync(id);
            return _mapper.Map<ContributionDetailViewModel>(contribution);
        }

        public async Task<bool> CreateContributionAsync(CreateContributionRequest request, Func<Contribution, UserViewModel, Task> sendingEmailCallBack)
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

            var magazine = await _magazineRepository.GetMagazineByIdAsync(request.MagazineId);
            if (magazine == null)
            {
                throw new ArgumentException("Invalid magazine id.", nameof(request.MagazineId));
            }

            var contribution = _mapper.Map<Contribution>(request);
            contribution.User = user;
            contribution.Status = (int)ContributionStatus.Pending;
            contribution.AuthorName = user.UserName;
            contribution.IsAcceptedTerms = true;
            contribution.Magazine = magazine;
            
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
            var coordinator = await _userService.GetCoordinatorEmailByFaculty(magazine.Faculty.FacultyId.ToString());
            await sendingEmailCallBack(contribution, coordinator);
            
            return true;
        }

        public async Task<bool> DeleteContributionAsync(string id)
        {
            var contribution = await _contributionRepository.GetContributionByIdAsync(id);
            if (contribution == null)
            {
                return false;
            }
            await _fileService.DeleteFiles(contribution.Files.Select(f => f.FileId.ToString()).ToList(), true);
            // Check if the id is valid
            if (id == string.Empty)
            {
                throw new ArgumentException("Invalid contribution id.", nameof(id));
            }
            return await _contributionRepository.DeleteContributionAsync(id);
        }

        public async Task<bool> UpdateContributionAsync(UpdateContributionRequest request, Func<Contribution, UserViewModel, Task> sendingEmailCallBack)
        {
            try
            {
                var contribution = await _contributionRepository.GetContributionByIdAsync(request.ContributionId);
                if (contribution == null)
                {
                    throw new ArgumentException("Invalid contribution id.", nameof(request.ContributionId));
                }


                //reset the status when it updates
                contribution.Title = request.Title;
                contribution.Content = request.Content;
                contribution.IsSeenByOrdinator = false;
                contribution.Status = (int)ContributionStatus.Pending;
                contribution.AcceptedDate = null;

                if (!request.Files.IsNullOrEmpty()) //update the whole file list of the contribution
                {
                    await _fileService.DeleteFiles(contribution.Files.Select(f => f.FileId.ToString()).ToList());
                    var createMutipleFileRequest = new CreateMultipleFileRequest
                    {
                        ContributionId = contribution.ContributionId.ToString(),
                        Files = request.Files
                    };

                    var fileIds = await _fileService.SaveMultipleFilesAsync(createMutipleFileRequest);
                    var files = await _fileRepository.GetAllFileByIdAsync(fileIds.ToList());
                    contribution.Files = files;
                }

                var result = await _contributionRepository.UpdateContributionAsync(contribution);
                if (contribution.Magazine.Faculty != null)
                {
                    var coordinator = await _userService.GetCoordinatorEmailByFaculty(contribution.Magazine.Faculty.FacultyId.ToString());
                    await sendingEmailCallBack(contribution, coordinator);
                }

                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
