using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Web.Models
{
    [Table("Magazine")]
    public class Magazine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MagazineId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType (DataType.Date)]
        public DateTime FirstClosureDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FinalClosureDate { get; set;}

        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear? AcademicYear { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }
    }
}
