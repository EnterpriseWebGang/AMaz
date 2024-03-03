using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("Faculty")]
    [Index(nameof(Name))]
    public class Faculty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FacultyId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
