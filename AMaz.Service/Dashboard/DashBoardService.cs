using AMaz.Common;
using AMaz.DB;
using AMaz.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var leftQuery = from Faculty in _db.Faculties
                            join magazine in _db.Magazines.Include(m => m.AcademicYear).Include(m => m.Faculty) on Faculty.FacultyId equals magazine.Faculty.FacultyId into magazineFaculty
                            from mf in magazineFaculty.DefaultIfEmpty() //left join
                            join contribution in _db.Contributions on mf.MagazineId equals contribution.Magazine.MagazineId into facultyContribution
                            join academicYear in _db.AcademicYears on mf.AcademicYear.AcademicYearId equals academicYear.AcademicYearId into magazineAcademicYear
                            from ma in magazineAcademicYear.DefaultIfEmpty() //left join
                            from cf in facultyContribution.DefaultIfEmpty() //left join
                            select new
                            {
                                Faculty = Faculty,
                                AcademicYear = ma,
                                Magazine = mf,
                                Contribution = cf
                            };

            var leftData = await leftQuery.ToListAsync();

            var rightQuery = from academicYear in _db.AcademicYears
                             join magazine in _db.Magazines.Include(m => m.AcademicYear).Include(m => m.Faculty) on academicYear.AcademicYearId equals magazine.AcademicYear.AcademicYearId into magazineAcademicYear
                             from ma in magazineAcademicYear.DefaultIfEmpty() //left join
                             join contribution in _db.Contributions on ma.MagazineId equals contribution.Magazine.MagazineId into facultyContribution
                             join Faculty in _db.Faculties on ma.Faculty.FacultyId equals Faculty.FacultyId into magazineFaculty
                             from mf in magazineFaculty.DefaultIfEmpty() //left join
                             from cf in facultyContribution.DefaultIfEmpty() //left join
                             orderby academicYear.DateTimeFrom
                             select new
                             {
                                 Faculty = mf,
                                 AcademicYear = academicYear,
                                 Magazine = ma,
                                 Contribution = cf
                             };
            var rightData = await rightQuery.ToListAsync();

            var dbData = leftData.Union(rightData).ToList();
            var actualData = dbData.GroupBy(d => new { d.Faculty, d.AcademicYear }).Select(d => new
            {
                Faculty = d.Key.Faculty?.Name,
                AcademicYear = d.Key.AcademicYear?.ToString(),
                ContributionCount = d.Where(c => c.Contribution != null)?.Count() ?? 0
            }).OrderBy(d => d.AcademicYear).ToList();

            Dictionary<string, int> academicYearPositionMap = new Dictionary<string, int>();
            Dictionary<string, int> facultyPositionMap = new Dictionary<string, int>();

            var academicYears = actualData.DistinctBy(d => d.AcademicYear).Where(d => d.AcademicYear != null).Select(d => d.AcademicYear.ToString()).ToList();

            for (int i = 0; i < academicYears.Count(); i++)
            {
                academicYearPositionMap.Add(academicYears[i], i);
            }

            var faculties = actualData.DistinctBy(d => d.Faculty).Where(d => d.Faculty != null).OrderBy(d => d.Faculty).Select(d => d.Faculty).ToList();

            for (int i = 0; i < faculties.Count(); i++)
            {
                facultyPositionMap.Add(faculties[i], i);
            }

            var xCount = academicYears.Count();
            var yCount = faculties.Count();

            var dashBoardData = new int[yCount, xCount];

            var lastAcademicYearPosition = 0;
            var lastFacultyPosition = 0;

            foreach (var item in actualData.GroupBy(d => d.Faculty))
            {
                foreach (var academicYearAndCount in item)
                {
                    if (item.Key.IsNullOrEmpty() || academicYearAndCount.AcademicYear == null)
                    {
                        continue;
                    }
                    dashBoardData[facultyPositionMap[item.Key], academicYearPositionMap[academicYearAndCount.AcademicYear.ToString()]] = academicYearAndCount.ContributionCount;
                }
            }

            return new DashBoardViewModel
            {
                FirstLabels = academicYears.ToArray(),
                SecondLabels = faculties.ToArray(),
                Data = dashBoardData,
            };

        }

        public async Task<DashBoardViewModel> GetContributorPercentageByFacultyForAnAcademicYear(string academicYearId)
        {
            var query = from faculty in _db.Faculties
                        join u in
                            (from user in _db.Users.Include(u => u.Faculty)
                             join contribution in _db.Contributions.Include(c => c.User) on user.Id equals contribution.User.Id into uc //get users with contributions
                             from c in uc
                             join magazine in _db.Magazines.Include(m => m.AcademicYear) on c.Magazine.MagazineId equals magazine.MagazineId into mc
                             from m in mc.DefaultIfEmpty() //left join
                             join AcademicYear in _db.AcademicYears on m.AcademicYear.AcademicYearId equals AcademicYear.AcademicYearId into ma
                             from a in ma.DefaultIfEmpty()//left join
                             where a.AcademicYearId.ToString() == academicYearId
                             select new
                             {
                                 FacultyId = user.Faculty.FacultyId,
                                 User = user,
                             })
                        on faculty.FacultyId equals u.FacultyId into fu
                        from f in fu.DefaultIfEmpty() //left join
                        select new
                        {
                            Faculty = faculty,
                            Student = f.User,
                        };

            var dbData = await query.ToListAsync();

            var actualData = dbData.DistinctBy(d => d.Student?.Id).GroupBy(d => d.Faculty).Select(d => new
            {
                Faculty = d.Key,
                StudentCount = d.Count(c => c.Student != null),
            }).ToList();
            var total = actualData.Sum(d => d.StudentCount);

            var xCount = dbData.DistinctBy(d => d.Faculty).Count();
            var yCount = 1;

            var faculties = dbData.DistinctBy(d => d.Faculty.Name).Select(d => d.Faculty).ToList();
            var dashBoardData = new int[yCount, xCount];

            var currentX = 0;
            var currentY = 0;

            foreach (var item in actualData)
            {
                dashBoardData[currentY, currentX] = (int)Math.Round((double)item.StudentCount / total * 100);
                currentX++;
            }

            return new DashBoardViewModel
            {
                FirstLabels = faculties.Select(f => f.Name).ToArray(),
                Data = dashBoardData,
            };
        }

        public async Task<DashBoardViewModel> GetContributorCountByFacultyAndAcademicYear()
        {
            var leftJoinQuery = from academicYear in _db.AcademicYears
                                join u in
                                    (from user in _db.Users.Include(u => u.Faculty)
                                     join contribution in _db.Contributions.Include(c => c.User) on user.Id equals contribution.User.Id into uc //get users with contributions
                                     from c in uc.DefaultIfEmpty() //left join
                                     join magazine in _db.Magazines.Include(m => m.AcademicYear) on c.Magazine.MagazineId equals magazine.MagazineId into mc
                                     from m in mc.DefaultIfEmpty() //left join
                                     join Faculty in _db.Faculties on user.Faculty.FacultyId equals Faculty.FacultyId into uf
                                     from f in uf.DefaultIfEmpty() //left join
                                     select new
                                     {
                                         AcademicYearId = m.AcademicYear.AcademicYearId,
                                         Magazine = m,
                                         Faculty = f,
                                         User = user,
                                     })
                                on academicYear.AcademicYearId equals u.AcademicYearId into au
                                from a in au.DefaultIfEmpty() //left join
                                select new
                                {
                                    Faculty = a.Faculty,
                                    User = a.User,
                                    AcademicYear = academicYear,
                                };
            var leftJoinData = await leftJoinQuery.ToListAsync();

            var rightJoinQuery = from faculty in _db.Faculties
                                 join au in
                                     (from user in _db.Users.Include(u => u.Faculty)
                                      join contribution in _db.Contributions.Include(c => c.User) on user.Id equals contribution.User.Id into uc //get users with contributions
                                      from c in uc.DefaultIfEmpty() //left join
                                      join magazine in _db.Magazines.Include(m => m.AcademicYear) on c.Magazine.MagazineId equals magazine.MagazineId into mc
                                      from m in mc.DefaultIfEmpty() //left join
                                      join academicYear in _db.AcademicYears on m.AcademicYear equals academicYear into ma
                                      from a in ma
                                      select new
                                      {
                                          AcademicYear = a,
                                          Magazine = m,
                                          Faculty = user.Faculty,
                                          User = user,
                                      })
                            on faculty equals au.Faculty into fu
                                 from u in fu.DefaultIfEmpty() //left join
                                 orderby u.AcademicYear.DateTimeTo
                                 select new
                                 {
                                     Faculty = faculty,
                                     User = u.User,
                                     AcademicYear = u.AcademicYear,
                                 };


            var rightJoinData = await rightJoinQuery.ToListAsync();
            var fullJoinData = leftJoinData.Union(rightJoinData).ToList();     // Count = 5
            var actualData = fullJoinData.GroupBy(d => new { d.Faculty, d.AcademicYear }).Select(d => new
            {
                Faculty = d.Key.Faculty?.Name,
                AcademicYear = d.Key.AcademicYear?.ToString(),
                UserCount = d.Where(c => c.User != null)?.Count() ?? 0
            }).ToList();

            Dictionary<string, int> academicYearPositionMap = new Dictionary<string, int>();
            Dictionary<string, int> facultyPositionMap = new Dictionary<string, int>();

            var academicYears = actualData.Where(d => d.AcademicYear != null).Select(d => d.AcademicYear).ToList();
            for (int i = 0; i < academicYears.Count(); i++)
            {
                academicYearPositionMap.Add(academicYears[i], i);
            }

            var faculties = actualData.Where(d => d.Faculty != null).Select(d => d.Faculty).ToList();
            for (int i = 0; i < faculties.Count(); i++)
            {
                facultyPositionMap.Add(faculties[i], i);
            }

            var xCount = academicYears.Count();
            var yCount = faculties.Count();

            var dashBoardData = new int[yCount, xCount];

            var currentX = 0;
            var currentY = 0;

            foreach (var item in actualData.GroupBy(d => d.Faculty))
            {
                if (item.Key != null)
                {
                    foreach (var academicYearAndCount in item)
                    {
                        if (item.Key.IsNullOrEmpty() || academicYearAndCount.AcademicYear == null)
                        {
                            continue;
                        }
                        dashBoardData[facultyPositionMap[item.Key], academicYearPositionMap[academicYearAndCount.AcademicYear.ToString()]] = academicYearAndCount.UserCount;
                        currentX++;
                    }
                }
                currentY++;
                currentX = 0;
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