using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class UpdateAcademicYearViewModel
    {
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime DateTimeFrom { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        public DateTime DateTimeTo { get; set; }
    }
}
