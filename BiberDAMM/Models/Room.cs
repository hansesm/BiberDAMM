using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace BiberDAMM.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Raumnummer")]
        public string RoomNumber { get; set; }

        [Required]
        public int RoomTypeId { get; set; }
        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<Bed> Bed { get; set; }

//        public virtual ICollection<Treatment> Treatment { get; set; }
    }
}