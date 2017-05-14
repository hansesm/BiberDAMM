using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class CreationRoomType
    {
        [Required]
        [Display(Name = "Raumtyp")]
        public string Name { get; set; }
    }
}