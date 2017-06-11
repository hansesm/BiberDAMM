using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class BlocksCreateViewModel
    {
        public Stay Stay { get; set; }
        public Blocks Blocks { get; set; }
        public List<SelectListItem> ListBedModels { get; set; }


        public BlocksCreateViewModel(Blocks blocks, Stay stay, List<SelectListItem> ListBedModels)
        {
            this.Stay = stay;
            this.Blocks = Blocks;
            this.ListBedModels = ListBedModels;
        }

        public BlocksCreateViewModel()
        {

        }
    }
}