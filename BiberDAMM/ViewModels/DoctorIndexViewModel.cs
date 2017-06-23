using System.Web.Mvc;


//Author: ChristesR
//Class for generating 

namespace BiberDAMM.ViewModels
{
    public class DoctorIndexViewModel
    {
        public JsonResult ListTreatments { get; set; }
        public DoctorIndexViewModel(JsonResult ListTreatments)
        {
            
            this.ListTreatments = ListTreatments;
        }
    }
}