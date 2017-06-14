﻿using BiberDAMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    // TODO [KrabsJ] required attributes?
    // viewModel for selecting a treatment type in the process of creating a new treatment [KrabsJ]
    public class CreationTreatmentSelectType
    {
        public int StayId { get; set; }
        public int TreatmentTypeId { get; set; }
        public List<SelectListItem> ListTreatmentTypes { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}