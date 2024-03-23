using System;
using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class ContributionViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Submission Date")]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Display(Name = "Seen By Coordinator")]
        public bool IsSeenByOrdinator { get; set; }

        [Display(Name = "Accepted Terms")]
        public bool IsAcceptedTerms { get; set; }

        [Display(Name = "Accepted Date")]
        [DataType(DataType.Date)]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

    }
}
