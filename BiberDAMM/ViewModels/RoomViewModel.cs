using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    //[JEL]
    public class RoomViewModel
    {
        public Room room { get; set; }       
        public List<SelectListItem> listRoomTypes { get; set; }

        public RoomViewModel (Room room, List<SelectListItem> listRoomTypes)
        {
            this.room = room;
            this.listRoomTypes = listRoomTypes;
        }


    }
    
}