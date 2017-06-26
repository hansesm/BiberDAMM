using System.Web.Mvc;


//Author: ChristesR
//Class for generating 

namespace BiberDAMM.ViewModels
{
    public class TherapistIndexViewModel
    {
        public JsonResult ListTreatments { get; set; }
        public TherapistIndexViewModel(JsonResult ListTreatments)
        {

            this.ListTreatments = ListTreatments;
        }
    }
}