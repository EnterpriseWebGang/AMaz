﻿using System.ComponentModel.DataAnnotations;

namespace AMaz.Common
{
    public class CreateAcademicYearViewModel
    {
        [Required]
        [Display(Name = "Start date")]
        public DateTime DateTimeFrom { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime DateTimeTo { get; set; }
    }
}