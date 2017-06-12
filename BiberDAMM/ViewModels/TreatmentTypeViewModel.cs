using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    //[JEL]
    public class TreatmentTypeViewModel
    {   
        public TreatmentType treatmentType { get; set; }
        public List<SelectListItem> listRoomTypes { get; set; }

        public TreatmentTypeViewModel(TreatmentType treatmentType, List<SelectListItem> listRoomTypes)
        {
            this.treatmentType = treatmentType;
            this.listRoomTypes = listRoomTypes;
        }


    }

}