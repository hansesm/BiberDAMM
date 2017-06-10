using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class StayCreateViewModel
    {
        public int Id { get; set; }
        public Stay Stay { get; set; }
        //public Client Client { get; set; }
        public List<SelectListItem> ListDoctors { get; set; }

        public StayCreateViewModel(int id, Stay stay, List<SelectListItem> listDoctors)
        {
            this.Id = id;
            //this.Client = client;
            this.Stay = stay;
            this.ListDoctors = listDoctors;
        }

        public StayCreateViewModel()
        {
            
        }
    }
}