using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationContactType
    {
        [Required]
        [Display(Name = "Kontakttyp")]
        public string Name { get; set; }
    }
}