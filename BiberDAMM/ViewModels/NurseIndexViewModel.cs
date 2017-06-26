using System.Web.Mvc;


//Author: ChristesR
//Class for generating 

namespace BiberDAMM.ViewModels
{
    public class NurseIndexViewModel
    {
        public JsonResult ListTreatments { get; set; }
        public NurseIndexViewModel(JsonResult ListTreatments)
        {

            this.ListTreatments = ListTreatments;
        }
    }
}