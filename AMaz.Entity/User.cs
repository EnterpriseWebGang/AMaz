using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Index(nameof(Email))]
    public class User : IdentityUser
    {
        [PersonalData]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [PersonalData]
        [StringLength(255)]
        public string? LastName { get; set; }

        [PersonalData]
        public bool IsActive {  get; set; }

        [PersonalData]
        public DateTime? LastDeactivatedDate { get; set; }

        [PersonalData]
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }

    }
}
