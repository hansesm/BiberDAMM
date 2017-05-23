using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationRoomType
    {
        [Required]
        [Display(Name = "Raumtyp")]
        public string Name { get; set; }
    }
}