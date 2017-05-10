using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    // To which time is a bed blocked in a stay
    //In this class there is a combined primary key, that prevents beds to be blocked multiple times at once
    public class Blocks
    {
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

    }
}