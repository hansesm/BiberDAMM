using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    public enum InsuranceType
    {
        privat, gesetzlich
    }

    public class CreationHealthInsurance
    {
        [Required]
        [Display(Name = "Versicherungsname")]
        public string Name { get; set; }

        [Display(Name = "Vericherungstyp")]
        public InsuranceType Type { get; set; }
    }
}