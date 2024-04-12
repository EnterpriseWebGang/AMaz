using AMaz.Common;
using AMaz.DB;
using AMaz.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Service
{
    public class DashBoardService
    {
        private readonly AMazDbContext _db;

        public DashBoardService(AMazDbContext db)
        {
            _db = db;
        }

        public async Task<DashBoardViewModel> GetFacultyContributionCountByAcademicYear()
        {
            var query = from magazine in
                                    (from magazine in _db.Magazines.Include(m => m.AcademicYear).Include(m => m.Faculty).Include(m => m.Contributions)
                                     select new
                                     {
                                         Faculty = magazine.Faculty,
                                         AcademicYear = magazine.AcademicYear,
                                         ContributionCount = magazine.Contributions.Count(),
                                     })
                        group magazine by new { magazine.Faculty, magazine.AcademicYear } into data
                        select new
                        {
                            AcademicYear = data.Key.AcademicYear.ToString(),
                            Faculty = data.Key.AcademicYear.ToString(),
                            Count = data.ToList().Sum(a => a.ContributionCount),
                        };

            var data = await query.ToListAsync();

            var xCount = data.DistinctBy(d => d.AcademicYear).Count();
            var yCount = data.DistinctBy(d => d.Faculty).Count();

            var academicYears = new List<string>();
            var faculties = new List<string>();
            var dashBoardData = new int[xCount, yCount];

            var currentX = 0;
            var currentY = 0;
            
            foreach (var item in data.GroupBy(d => d.AcademicYear))
            {
                academicYears.Add(item.Key);
                foreach(var facultyCount in item)
                {
                    faculties.Add(item.Key);
                    dashBoardData[currentX, currentY] = facultyCount.Count;
                    currentY++;
                }

                currentX++;
            }



            return new DashBoardViewModel
            {
                FirstLabels = academicYears.ToArray(),
                SecondLabels = faculties.ToArray(),
                Data = dashBoardData,
            };

        }
    }
}
