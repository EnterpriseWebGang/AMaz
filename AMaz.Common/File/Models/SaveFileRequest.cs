using Microsoft.AspNetCore.Http;

namespace AMaz.Models
{
    public class SaveFileRequest
    {
        public IFormFile File { get; set; }

        public string Path { get; set; }
    }

}
