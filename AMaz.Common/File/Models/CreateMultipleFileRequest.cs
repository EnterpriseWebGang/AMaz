using Microsoft.AspNetCore.Http;

namespace AMaz.Models
{
    public class CreateMultipleFileRequest
    {
        public List<IFormFile>? Files { get; set; }
        public string? ContributionId {  get; set; }
    }
}
