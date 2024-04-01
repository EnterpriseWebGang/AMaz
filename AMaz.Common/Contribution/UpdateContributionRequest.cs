using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class UpdateContributionRequest
    {
        public string ContributionId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
