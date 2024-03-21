using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class ChangeUserRoleViewModel
    {
        [Required]
        public int Role { get; set; }
    }
}
