using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("Users")]
    [Index(nameof(Email))]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public int Role { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsActive {  get; set; }

        public DateTime? LastDeactivatedDate { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }

    }
}
