using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class StayIndexViewModel
    {
        public IEnumerable<Stay> Stays { get; set; }
        public DateTime RequestedDate { get; set; }

        public StayIndexViewModel(IEnumerable<Stay> stays, DateTime date)
        {
            this.Stays = stays;
            this.RequestedDate = date;
        }
    }
}