using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationBed
    {
        [Required]
        [Display(Name = "Modell")]
        public string Model { get; set; }
    }
}