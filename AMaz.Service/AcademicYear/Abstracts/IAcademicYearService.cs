using AMaz.Common;
namespace AMaz.Service
{
    public interface IAcademicYearService
    {
        Task<(bool succeed, string errorMsg)> CreateAcademicYearAsync(CreateAcademicYearRequest request);
        Task<(bool succeed, string errorMsg)> DeleteAcademicYearAsync(string id);
        Task<AcademicYearViewModel> GetAcademicYearByIdAsync(string id);
        Task<IEnumerable<AcademicYearViewModel>> GetAllAcademicYearsAsync();
        Task<(bool succeed, string errorMsg)> UpdateAcademicYearAsync(UpdateAcademicYearRequest request);
        Task<AcademicYearViewModel> GetLatestAcademicYear();
    }
}