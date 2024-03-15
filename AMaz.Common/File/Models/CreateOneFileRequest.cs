using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Models
{
    public class CreateOneFileRequest
    {
        public IFormFile? File { get; set; }
        public string? ContributionId { get; set; }
    }
}
