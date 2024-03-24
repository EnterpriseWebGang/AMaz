﻿using AMaz.Common;

namespace AMaz.Service
{
    public interface IMagazineService
    {
        Task<(bool succeed, string errorMsg)> CreateMagazineAsync(CreateMagazineRequest request);
        Task<(bool succeed, string errorMsg)> DeleteMagazineAsync(string magazineId);
        Task<List<MagazineViewModel>> GetAllMagazineByFacultyId(string facultyId);
        Task<List<MagazineViewModel>> GetAllMagazines();
        Task<MagazineDetailViewModel> GetMagazineByIdAsync(string magazineId);
        Task<(bool succeed, string errorMsg)> UpdateMagazineAsync(UpdateMagazineRequest request);
    }
}