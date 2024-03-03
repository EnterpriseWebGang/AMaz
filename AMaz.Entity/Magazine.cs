using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("Magazine")]
    [Index(nameof(Name))]
    public class Magazine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MagazineId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType (DataType.Date)]
        public DateTime FirstClosureDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinalClosureDate { get; set;}

        [ForeignKey("ContributionId")]
        public virtual ICollection<Contribution> Contributions { get; set; }

        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear? AcademicYear { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }
    }
}
