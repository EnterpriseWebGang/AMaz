using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class ChangeUserRoleAndFacultyViewModel
    {
        [Required]
        public int Role { get; set; }

        public string? Email { get; set; }

        public List<FacultyViewModel>? Faculties { get; set; }

        [Display(Name = "Faculty")]
        public string? FacultyId { get; set; }
    }
}
