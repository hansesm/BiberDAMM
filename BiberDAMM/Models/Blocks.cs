using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    // To which time is a bed blocked in a stay
    public class Blocks
    {
        public DateTime Date { get; set; }

        public int StayId { get; set; }
        public virtual Stay Stay { get; set; }

        public int BedId { get; set; }
        public virtual Bed Bed { get; set; }

    }
}