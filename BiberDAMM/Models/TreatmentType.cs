using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    public class TreatmentType
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Behandlungstyp")]
        public string Name { get; set; }

        //        public Nullable<int> RoomType_Id { get; set; }
        [Display(Name = "Raumtyp-ID")]
        public int? RoomType_Id { get; set; }

        public virtual RoomType RoomType { get; set; }

//        public virtual ICollection<Treatment> Treatment { get; set; }
    }
}