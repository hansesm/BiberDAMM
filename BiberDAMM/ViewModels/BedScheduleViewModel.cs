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

        [Display(Name = "Modellbezeichnung")]
        public BedModels BedModel { get; set; }

        [Display(Name = "Raumnummer")]
        public string RoomNbr { get; set; }

        [Display(Name = "Vorname")]
        public string PatientSName { get; set; }

        [Display(Name = "Nachname")]
        public string PatientLName { get; set; }

        [Display(Name = "Belegungsbeginn")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Belegungsende")]
        public DateTime EndDate { get; set; }
    }
}