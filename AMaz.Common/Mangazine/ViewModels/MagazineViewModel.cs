using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class MagazineViewModel
    {
        [Display(Name = "Id")]
        public string MagazineId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string FacultyName { get; set; }

        [Required]
        [Display(Name = "First Closure Date")]
        [DataType(DataType.Date)]
        public DateTime FirstClosureDate { get; set; }

        [Required]
        [Display(Name = "Final Closure Date")]
        [DataType(DataType.Date)]
        public DateTime FinalClosureDate { get; set; }

        public AcademicYearViewModel AcademicYear { get; set; }
    }
}
