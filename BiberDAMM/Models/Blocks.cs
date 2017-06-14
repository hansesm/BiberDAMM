using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Foolproof;

namespace BiberDAMM.Models
{
    // To which time is a bed blocked in a stay
    //In this class there is a combined primary key, that prevents beds to be blocked multiple times at once

    public enum ClientRoomType
    {
        Einzelzimmer,
        Doppelzimmer,
        Mehrbettzimmer
    }
    
    public class Blocks
    {
        /*

        [Key]
        [Column(Order = 0)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 1)]
        public int StayId { get; set; }

        public virtual Stay Stay { get; set; }

        [Key]
        [Column(Order = 2)]
        public int BedId { get; set; }

        public virtual Bed Bed { get; set; }

        */

        //Changed the Datamodel - Because the initial concept was shit. [HansesM]

        [Display(Name= "Bettennummer")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Begindatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd.MM.yyyy}")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Display(Name = "Enddatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd.MM.yyyy}")]
        [GreaterThan("BeginDate")]
        public DateTime EndDate { get; set; }

        [Required]
        public int? StayId { get; set; }

        public virtual Stay Stay { get; set; }

        [Required]
        public int? BedId { get; set; }

        public virtual Bed Bed { get; set; }

        [Required]
        public ClientRoomType ClientRoomType { get; set; }


    }
}