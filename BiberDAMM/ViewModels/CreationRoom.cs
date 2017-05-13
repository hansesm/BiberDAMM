using BiberDAMM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    public class CreationRoom
    {
        [Required]
        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Required]
        public virtual ICollection<RoomType> RoomTypes { get; set; }


    }
}