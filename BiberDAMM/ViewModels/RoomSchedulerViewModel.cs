using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    public class RoomSchedulerViewModel
    {
        public List<SelectListItem> ListRooms { get; set; }

        public RoomSchedulerViewModel(List<SelectListItem> listRooms)
        {
            this.ListRooms = listRooms;
        }
    }
}