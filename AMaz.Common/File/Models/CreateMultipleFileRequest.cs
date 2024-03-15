using Microsoft.AspNetCore.Http;

namespace AMaz.Models
{
    public class CreateMultipleFileRequest
    {
        public IFormFileCollection? Files { get; set; }
        public string? ContributionId {  get; set; }
    }
}
