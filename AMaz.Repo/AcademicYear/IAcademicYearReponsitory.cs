using AMaz.Entity;

namespace AMaz.Repo
{
    public interface IAcademicYearReponsitory
    {
        Task<AcademicYear> CreateAcademicYearAsync(AcademicYear model);
        Task DeleteAcademicYearAsync(string id);
        Task<AcademicYear> GetAcademicYearByIdAsync(string id);
        Task<IEnumerable<AcademicYear>> GetAllAcademicYearsAsync();
        Task UpdateAcademicYearAsync(AcademicYear model);
    }
}