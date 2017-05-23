using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public enum InsuranceType
    {
        privat,
        gesetzlich
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