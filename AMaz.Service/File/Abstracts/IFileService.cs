using AMaz.Models;

namespace AMaz.Service
{
    public interface IFileService
    {
        Task<List<FileViewModel>> GetAllFiles();
        Task<List<FileViewModel>> GetAllFilesByIds(List<string> ids);
        Task<List<FileViewModel>> GetAllFilesByContribution(string contributionId);
        Task<FileViewModel> GetFileById(string id);
        Task SaveFileToDiskAsync(SaveFileRequest request);
        Task SaveMultipleFilesAsync(CreateMultipleFileRequest uploadMultipleFile);
        Task SaveOneFileAsync(CreateOneFileRequest request);
    }
}