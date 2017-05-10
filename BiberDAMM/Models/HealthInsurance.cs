using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    public enum InsuranceType
    {
        privat, gesetzlich
    }

    public class HealthInsurance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Versicherungsname")]
        public string Name { get; set; }

        [Display(Name = "Vericherungstyp")]
        public InsuranceType Type { get; set; }

        public virtual ICollection<Client> Client { get; set; }
    }
}