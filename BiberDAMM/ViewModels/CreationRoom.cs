using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationRoom
    {
        [Required]
        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Required]
        public virtual ICollection<RoomType> RoomTypes { get; set; }
    }
}