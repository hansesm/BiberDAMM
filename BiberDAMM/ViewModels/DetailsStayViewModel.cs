﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class DetailsStayViewModel
    {
        public Stay Stay { get; set; }
        public List<SelectListItem> ListDoctors { get; set; }

        public DetailsStayViewModel(Stay stay, List<SelectListItem> listDoctors)
        {
            this.Stay = stay;
            this.ListDoctors = listDoctors;
        }
    }
}