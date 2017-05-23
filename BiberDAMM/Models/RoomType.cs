﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Raumtyp")]
        public string Name { get; set; }

        public virtual ICollection<Room> Room { get; set; }

        public virtual ICollection<TreatmentType> TreatmentType { get; set; }
    }
}