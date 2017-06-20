using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    /* Jean-PierreK
    * Added an enum list of all available bed models
    */

    public enum Models
    {
        Inkubator,
        Intensivbett,
        Klinikbett,
        Rotationsbett,
        Säuglingsbett,
        Schwerlastbett
    }
    public class Bed
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Modell")]
        public Models Model { get; set; }

        [Required]
        [Display(Name = "Raum")]
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<Blocks> Blocks { get; set; }

    
    }
}