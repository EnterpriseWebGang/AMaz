using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class CreateAccountViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int Role { get; set; }

        public List<FacultyViewModel>? Faculties { get; set; }

        [Display(Name = "Faculty")]
        public string? FacultyId { get; set; }
    }
}
