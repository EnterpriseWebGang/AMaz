using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("AcademicYear")]
    public class AcademicYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AcademicYearId { get; set; }

        [Required]
        public DateTime DateTimeFrom { get; set; }

        [Required]
        public DateTime DateTimeTo { get; set; }

        [ForeignKey("MagazineId")]
        public ICollection<Magazine> Magazines { get; set; }

        public override string ToString()
        {
            return DateTimeFrom.ToString("MM/dd/yyyy") + " - " + DateTimeTo.ToString("MM/dd/yyyy");
        }
    }
}
