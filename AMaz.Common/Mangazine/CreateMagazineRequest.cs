using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class CreateMagazineRequest
    {
        public string Name { get; set; }

        public DateTime FirstClosureDate { get; set; }

        public DateTime FinalClosureDate { get; set; }
    }
}
