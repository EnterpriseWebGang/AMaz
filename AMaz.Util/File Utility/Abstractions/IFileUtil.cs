using Microsoft.AspNetCore.Http;

namespace AMaz.Util
{
    public interface IFileUtil
    {
        public Task<FileOperationResult> SaveFileAsync(IFormFile file, string path);
        public Task<FileOperationResult> DeleteFileAsync(string path);
        public Task<(MemoryStream, FileOperationResult)> GetFileAsync(string path);
    }
}
