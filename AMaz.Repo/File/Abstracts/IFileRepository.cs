namespace AMaz.Repo
{
    public interface IFileRepository
    {
        Task<Entity.File> GetFileByIdAsync(string id);
        Task<List<Entity.File>> GetAllFileByIdAsync(List<string> ids);
        Task<List<Entity.File>> GetAllFilesAsync();
        Task<List<Entity.File>> GetFilesByNameAsync(string name);
        Task<List<Entity.File>> GetAllFilesByContributionId(string contributionId);
        Task<bool> SaveFile(Entity.File file);
        Task<bool> SaveMutipleFiles(List<Entity.File> files);
        Task<bool> DeleteMultipleFiles(IEnumerable<string> ids);
    }
}