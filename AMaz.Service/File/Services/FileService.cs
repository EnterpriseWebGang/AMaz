using AMaz.Common;
using AMaz.Repo;
using AMaz.Util;
using AMaz.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using File = AMaz.Entity.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace AMaz.Service
{
    public class FileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IOptions<LocalFileStorageConfiguration> _fileStorageConfiguration;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;


        int _batchSize = 100;
        public FileService(
            IFileRepository fileRepository,
            IOptions<LocalFileStorageConfiguration> fileStorageConfiguration,
            IMapper mapper,
            IHostingEnvironment environment)
        {
            _fileRepository = fileRepository;
            _fileStorageConfiguration = fileStorageConfiguration;
            _mapper = mapper;
            _environment = environment;
        }

        // public async Task GetFiles()
        public async Task<(Stream filestream, string fileName, FileOperationResult result, string contentType)> GetFileById(string id)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);
            var (stream, result) = await FileUtility.GetFileAsync(file.Path);
            stream.Position = 0; //reset the memory stream to return to User
            return (stream, file.Name, result, file.MIMEType);
        }

        public async Task DeleteFiles(List<string> id)
        {
            var files = await _fileRepository.GetAllFileByIdAsync(id);
            var tasks = new List<Task>();
            foreach (var file in files)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await DeleteFileFromDisk(file.Path);
                }));
            }

            await Task.WhenAll(tasks);
        }

        public async Task DeleteFileFromDisk(string path){
            await FileUtility.DeleteFileAsync(path);
        }

        public async Task<List<FileViewModel>> GetAllFilesAsync(List<string> ids = null)
        {
            if (ids.IsNullOrEmpty())
            {
                var files = await _fileRepository.GetAllFilesAsync();
                var result = _mapper.Map<List<FileViewModel>>(files);
                return result;
            }
            
            var filesById = await _fileRepository.GetAllFileByIdAsync(ids);
            var resultById = _mapper.Map<List<FileViewModel>>(filesById);
            return resultById;
        }

        public async Task<List<FileViewModel>> GetAllFilesByContribution(string contributionId)
        {
            var files = await _fileRepository.GetAllFilesByContributionId(contributionId);
            var result = _mapper.Map<List<FileViewModel>>(files);
            return result;
        }

        public async Task SaveFileToDiskAsync(SaveFileRequest request)
        {
            await FileUtility.SaveFileAsync(request.File, request.Path);
        }

        public async Task<IEnumerable<string>> SaveMultipleFilesAsync(CreateMultipleFileRequest uploadMultipleFile)
        {
            var result = new List<string>();
            await ListProcessUtility<IFormFile>.ProcessListByBatchAsync(uploadMultipleFile.Files.ToList(), _batchSize, async files =>
                  {
                      var fileModels = _mapper.Map<List<CreateFileModel>>(files);

                      for (var i = 0; i < fileModels.Count; i++)
                      {
                          fileModels[i].Path = GetPathForFile(fileModels[i]);
                      }

                      var insertInDb = _mapper.Map<List<File>>(fileModels);
                      await _fileRepository.SaveMutipleFiles(insertInDb);
                      result.AddRange(insertInDb.Select(f => f.FileId.ToString()));

                      var saveFileToDiskRequests = _mapper.Map<List<SaveFileRequest>>(fileModels);
                      var tasks = new List<Task>();
                      foreach (var request in saveFileToDiskRequests)
                      {
                          tasks.Add(Task.Run(async () =>
                          {
                              await SaveFileToDiskAsync(request);
                          }));
                      }

                      await Task.WhenAll(tasks);
                  });

            return result;
        }

        public async Task SaveOneFileAsync(CreateOneFileRequest request)
        {
            var fileModel = _mapper.Map<CreateFileModel>(request.File);
            GetPathForFile(fileModel);

            var insertInDb = _mapper.Map<File>(fileModel);
            await _fileRepository.SaveFile(insertInDb);

            var saveFileToDiskRequest = _mapper.Map<SaveFileRequest>(fileModel);
            await SaveFileToDiskAsync(saveFileToDiskRequest);
        }

        private string GetPathForFile(CreateFileModel fileModels)
        {

            var fileType = fileModels.FileType;
            var path = fileType switch
            {
                FileType.Image => Path.Combine(_environment.WebRootPath, _fileStorageConfiguration.Value.ImageStorePath, fileModels.Name),
                FileType.Docx => Path.Combine(_environment.WebRootPath, _fileStorageConfiguration.Value.DocxStorePath, fileModels.Name),
                FileType.Undefined => Path.Combine(_environment.WebRootPath, _fileStorageConfiguration.Value.OtherFileTypeStorePath, fileModels.Name),
                _ => Path.Combine(_environment.WebRootPath, "Files", fileModels.Name)
            };

            return path;
        }

        //public async

    }
}
