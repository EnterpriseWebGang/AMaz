using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Entity
{
    [Table("File")]
    [Index(nameof(Name))]
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FileId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Path { get; set; }

        //Undefined = 0
        //Image = 1
        //Docx = 2
        public int FileType { get; set; }

        public string? MIMEType { get; set; }

        [ForeignKey("ContributionId")]
        public virtual Contribution? Contribution { get; set; }
    }
}
