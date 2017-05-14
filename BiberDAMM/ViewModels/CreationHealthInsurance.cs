using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
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