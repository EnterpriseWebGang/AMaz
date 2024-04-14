using AMaz.Common;

namespace AMaz.Service
{
    public interface IMagazineService
    {
        Task<(bool succeed, string errorMsg)> CreateMagazineAsync(CreateMagazineRequest request);
        Task<(bool succeed, string errorMsg)> DeleteMagazineAsync(string magazineId);
        Task<List<MagazineViewModel>> GetAllMagazineByFacultyId(string facultyId);
        Task<List<MagazineViewModel>> GetAllMagazines();
        Task<MagazineDetailViewModel> GetMagazineByIdAsync(string magazineId, bool isAuthenticated = true);
        Task<UpdateMagazineViewModel> GetUpdateMagazineViewModelAsync(string magazineId);
        Task<(bool succeed, string errorMsg)> UpdateMagazineAsync(UpdateMagazineRequest request);
        Task<(bool succeed, byte[] fileBytes)> GetMagazineReport(string magazineId);
    }
}