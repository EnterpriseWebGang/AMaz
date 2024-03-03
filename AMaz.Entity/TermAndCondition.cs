using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("TermAndCondition")]
    public class TermAndCondition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TermAndConditionId { get; set; }

        public string Content { get; set;}

        [Required]
        public decimal Version { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime UpdatedDate { get; set;}

        [ForeignKey("ContributionId")]
        public virtual ICollection<Contribution> Contributions { get; set; }
    }
}
