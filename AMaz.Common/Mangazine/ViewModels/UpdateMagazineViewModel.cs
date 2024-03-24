using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class UpdateMagazineViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "First Closure Date")]
        [DataType(DataType.Date)]
        public DateTime FirstClosureDate { get; set; }

        [Required]
        [Display(Name = "Final Closure Date")]
        [DataType(DataType.Date)]
        public DateTime FinalClosureDate { get; set; }

        public List<AcademicYearViewModel> AcademicYears { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public string AcademicYearId { get; set; }
    }
}
