﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationTreatment
    {
        [Required]
        [Display(Name = "Behandlungsbeginn")]
        public DateTime Begin { get; set; }

        [Required]
        [Display(Name = "Behandlungsende")]
        public DateTime End { get; set; }

        public int StayId { get; set; }

        [Display(Name = "Räume")]
        public virtual ICollection<Room> Rooms { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "BehandlungsTyp")]
        public virtual ICollection<TreatmentType> TreatmentTypes { get; set; }
    }
}