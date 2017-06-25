/*
 * Class for room cleaning and cleaning personal related things
 * Jean-PierreK 
 */

using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public enum CleaningDuration
    {
        noCleaning,
        tenMinutes,
        twentyMinutes,
        thirtyMinutes,
    }

    public class Cleaner
    {
        public int Id { get; set; }

        [Display(Name = "Raum")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Display(Name = "Reinigungsbeginn")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Reinigungsende")]
        [GreaterThan("BeginDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Reinigung erfolgt")]
        public bool CleaningDone { get; set; }

        public CleaningDuration CleaningDuration { get; set; }

        public int TreatmentId { get; set; }
        public virtual Treatment Treatment { get; set; }

    }
}