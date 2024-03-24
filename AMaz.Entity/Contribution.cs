using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMaz.Entity
{
    [Table("Contribution")]
    [Index(nameof(Title))]
    public class Contribution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContributionId { get; set; }

        public string? Title { get; set;}

        public string? Content { get; set;}

        [Required]
        public int Status { get; set;}

        [Required]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Required]
        public bool IsSeenByOrdinator { get; set; }

        [Required]
        public bool IsAcceptedTerms {  get; set; }

        [DataType(DataType.Date)]
        public DateTime? AcceptedDate { get; set; }

        public string AuthorName {  get; set; }

        [Required]
        [ForeignKey("MagazineId")]
        public virtual Magazine Magazine { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("FileId")]
        public virtual ICollection<File>? Files { get; set; }
    }
}
