using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class UpdateFacultyViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
