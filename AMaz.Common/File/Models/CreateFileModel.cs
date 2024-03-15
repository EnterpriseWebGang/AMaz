using Microsoft.AspNetCore.Http;

namespace AMaz.Models
{
    public class CreateFileModel
    {
        public IFormFile File { get; set; }
        public string? Name { get; set; }

        public FileType FileType { get; set; }

        public string? MIMEType { get; set; }

        public string? Path { get; set; }
        
    }

    public enum FileType
    {
        Undefined = 0,
        Image = 1,
        Docx = 2,
        Zip = 4,
        Csv = 8
    }
}
