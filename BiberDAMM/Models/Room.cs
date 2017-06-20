using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Required]
        [Display(Name = "Raumtyp")]
        public int RoomTypeId { get; set; }

        [Required]
        [Display(Name = "Max. Betten")]
        public int RoomMaxSize { get; set; }

        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<Bed> Bed { get; set; }

        public virtual ICollection<Treatment> Treatment { get; set; }
    }
}