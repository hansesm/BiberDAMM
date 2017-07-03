using BiberDAMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//public JEL
namespace BiberDAMM.ViewModels
{
    public class BedsInRoomViewModel
    {
        public Room room { get; set; }
        
        public List<Blocks> currentBedBlocks { get; set; }
        public List<Bed> ListEmptyBeds { get; set; }

        public BedsInRoomViewModel(Room room, List<Blocks> currentBedBlocks, List<Bed> listBeds)
        {
            this.room = room;
            this.currentBedBlocks = currentBedBlocks;
            this.ListEmptyBeds = listBeds;
        }
    }
}