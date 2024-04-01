using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class CreateContributionViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Upload Files")]
        public List<IFormFile> Files { get; set; }

        [Required]
        [Display(Name = "MagazineId")]
        public string MagazineId { get; set; }

        public IEnumerable<MagazineViewModel>? Magazines { get; set; }
    }
}
