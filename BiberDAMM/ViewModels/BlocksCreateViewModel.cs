using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class BlocksCreateViewModel
    {
        //public Stay Stay { get; set; }
        public Blocks Blocks { get; set; }
        public List<SelectListItem> ListBedModels { get; set; }


        public BlocksCreateViewModel(Blocks blocks, List<SelectListItem> listBedModels)
        {
            this.Blocks = blocks;
            //this.Stay = stay;
            this.ListBedModels = listBedModels;
        }
    }
}