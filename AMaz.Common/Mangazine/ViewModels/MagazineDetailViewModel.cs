using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common 
{ 
    public class MagazineDetailViewModel : MagazineViewModel
    {
        public string FacultyName { get; set; }
        public string AcademicYear { get; set; }
        public IEnumerable<ContributionViewModel> Contributions { get; set; }
    }
}
