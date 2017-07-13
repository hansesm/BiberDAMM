/* 
 * ViewModel for BedSchedule
 * Author: Jean-PierreK
 */

using System;
using System.Collections.Generic;
using System.Linq;
using BiberDAMM.Models;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.ViewModels
{
    public class BedScheduleViewModel
    {
        public Bed bed { get; set; }

        [Display(Name = "Id")]
        public int BedNbr { get; set; }

        [Display(Name = "Modell")]
        public BedModels BedModel { get; set; }

        [Display(Name = "Raumnummber")]
        public string RoomNbr { get; set; }

        [Display(Name = "Vorname")]
        public string PatientSName { get; set; }

        [Display(Name = "Nachname")]
        public string PatientLName { get; set; }

        [Display(Name = "Beginndatum")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Endatum")]
        public DateTime EndDate { get; set; }
    }
}