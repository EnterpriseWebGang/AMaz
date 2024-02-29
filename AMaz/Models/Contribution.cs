using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Web.Models
{
    [Table("Contribution")]
    public class Contribution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ContributionId { get; set; }

        [Required]
        public string Title { get; set;}

        [Required]
        public string Text { get; set;}

        [Required]
        public string? FileLinks { get; set;}

        [Required]
        public string Status { get; set;}

        [Required]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AcceptedDate { get; set; }

        [ForeignKey("MagazineId")]
        public virtual Magazine? Magazine { get; set; }

        [ForeignKey("TermAndConditionId")]
        public virtual TermAndCondition? TermAndCondition { get; set; }
    }
}
