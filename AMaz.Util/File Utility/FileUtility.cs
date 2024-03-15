using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AMaz.Util
{
    public static class FileUtility
    {
        public static FileOperationResult DeleteFileAsync(string path)
        {
            var result = new FileOperationResult
            {
                FileOperationAction = FileOperationAction.Delete,
            };
            try
            {
                // Check if file exists with its full path
                if (File.Exists(path))
                {
                    // If file found, delete it
                    File.Delete(path);
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Error = $"The file donot exist: {path}";
                };
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = $"Unexpected Error: {e.Message}";
            }

            return result;
        }

        public static async Task<(MemoryStream, FileOperationResult)> GetFileAsync(string path)
        {
            MemoryStream stream = new MemoryStream();
            var result = new FileOperationResult
            {
                FileOperationAction = FileOperationAction.Get,
            };

            var directoryPath = Path.GetDirectoryName(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (string.IsNullOrEmpty(path) || !directoryInfo.Exists) //if not exist return empty stream
            {
                result.Success = false;
                result.Error = $"No such directory: {path}";
                return (stream, result);
            }

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    await fileStream.CopyToAsync(stream);
                }

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = $"Unexpect Error: {e.Message}";

            }      

            return (stream, result);
        }

        public static async Task<FileOperationResult> SaveFileAsync(IFormFile file, string path)
        {
            var result = new FileOperationResult
            {
                FileOperationAction = FileOperationAction.Save,
            };
            if (string.IsNullOrEmpty(path))
            {
                result.Success = false;
                result.Error = $"No such directory: {path}";
                return result;
            }
            
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(outputFileStream);
                };          
            }

            catch (Exception e)
            {
                result.Success = false;
                result.Error = $"Unexpect Error: {e.Message}";

                return(result);
            }

            result.Success = true;

            return result;
        }
    }

    public class FileOperationResult
    {
        public bool Success { get; set; }
        public FileOperationAction FileOperationAction { get; set; }
        public string? Error { get; set; }
    }

    public enum FileOperationAction
    {
        Save = 0,
        Get = 1,
        Delete = 2,
    }
}
