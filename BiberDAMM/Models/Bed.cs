using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public class Bed
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Modell")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Raum")]
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<Blocks> Blocks { get; set; }

        //TODO Sinn-klären [HansesM]
        internal static ApplicationUser FindById(int? id)
        {
            throw new NotImplementedException();
        }

        internal static object Delete(ApplicationUser deleteBed)
        {
            throw new NotImplementedException();
        }
    }
}