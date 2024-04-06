using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class AcademicYearViewModel
    {
        [Display(Name = "Id")]
        public string AcademicYearId { get; set; }

        [Display(Name = "Start date")]
        public string DateTimeFrom { get; set; }

        [Display(Name = "End date")]
        public string DateTimeTo { get; set; }
    }
}
