using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Models
{
    public class LocalFileStorageConfiguration
    {
        public string? ImageStorePath { get; set; }
        public string? DocxStorePath { get; set; }
        public string? OtherFileTypeStorePath { get; set; }
    }
}
