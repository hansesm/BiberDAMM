using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    public class CreationBed
    {
        [Required]
        [Display(Name = "Modell")]
        public string Model { get; set; }
    }
}