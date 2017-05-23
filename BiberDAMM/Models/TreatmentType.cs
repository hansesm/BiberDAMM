﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public class TreatmentType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Behandlungstyp")]
        public string Name { get; set; }


        [Display(Name = "Raumtyp-ID")]
        public int? RoomTypeId { get; set; }

        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}