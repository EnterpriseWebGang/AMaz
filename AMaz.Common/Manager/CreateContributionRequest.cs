using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class CreateContributionRequest
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }

        public DateTime SubmissionDate { get; set; }

    }
}
