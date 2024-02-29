using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Web.Models
{
    [Table("AcademicYear")]
    public class AcademicYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AcademicYearId { get; set; }

        [Required]
        public DateTime DateTimeFrom { get; set; }

        [Required]
        public DateTime DateTimeTo { get; set; }
    }
}
