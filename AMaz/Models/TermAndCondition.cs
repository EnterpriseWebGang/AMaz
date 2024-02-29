using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Web.Models
{
    [Table("TermAndCondition")]
    public class TermAndCondition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TermAndConditionId { get; set; }

        [Required]
        public string Content { get; set;}

        [Required]
        public decimal Version { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}
