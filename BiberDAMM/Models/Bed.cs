using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    public class Bed
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Modell")]
        public string Model { get; set; }

        [Required]
        [Display(Name="Raum")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public virtual ICollection<Blocks> Blocks { get; set; }
    }
}