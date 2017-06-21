﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    /* Jean-PierreK
    * Added an enum list of all available bed models
    * Changed enum name to something more error prone
    */

    public enum BedModels
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
        public BedModels BedModels { get; set; }

        [Required]
        [Display(Name = "Raum")]
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<Blocks> Blocks { get; set; }

    
    }
}