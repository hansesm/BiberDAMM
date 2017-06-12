using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class StayDetailsViewModel
    {
        public Stay Stay { get; set; }
        public List<SelectListItem> ListDoctors { get; set; }
        public JsonResult ListTreatments { get; set; }

        public StayDetailsViewModel(Stay stay, List<SelectListItem> listDoctors, JsonResult ListTreatments)
        {
            this.Stay = stay;
            this.ListDoctors = listDoctors;
            this.ListTreatments = ListTreatments;
        }
    }
}