using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================

    public class CreationTreatmentType
    {
        [Required]
        [Display(Name = "Behandlungstyp")]
        public string Name { get; set; }


        [Display(Name = "Raumtyp")]
        public virtual ICollection<RoomType> RoomType { get; set; }
    }
}