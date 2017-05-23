using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public enum InsuranceType
    {
        privat,
        gesetzlich
    }

    public class HealthInsurance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Versicherungsname")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Versicherungsnummer")]
        public string Number { get; set; }

        [Display(Name = "Vericherungstyp")]
        public InsuranceType Type { get; set; }

        public virtual ICollection<Client> Client { get; set; }
    }
}