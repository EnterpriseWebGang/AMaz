﻿using AMaz.Models;
using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class ContributionDetailViewModel
    {
        [Display(Name = "Id")]
        public string ContributionId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Submission Date")]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Display(Name = "Accepted Date")]
        [DataType(DataType.Date)]
        public DateTime? AcceptedDate { get; set; }

        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        public string CoordinatorComment { get; set; }

        [Display(Name = "Is Accepted")]
        public bool IsAccepted { get; set; }
        public FileViewModel[] Files { get; set; }
        public MagazineViewModel Magazine { get; set; }
        public UserViewModel User { get; set; }
    }
}
