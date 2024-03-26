using System.ComponentModel.DataAnnotations;
namespace AMaz.Common
{
    public class CreateFacultyViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
