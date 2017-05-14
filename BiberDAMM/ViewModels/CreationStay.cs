using BiberDAMM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public enum StayType
    {
        ambulant, stationär
    }
    public class CreationStay
    {
        [Required]
        [Display(Name = "Aufnahmedatum")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Entlassungsdatum")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Diagnosekennzahl(ICD10)")]
        public string ICD10 { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Ergebnis")]
        public string Result { get; set; }

        [Required]
        [Display(Name = "Behandlungstyp")]
        public StayType StayType { get; set; }

        [Display(Name = "Patient")]
        public int ClientId { get; set; }

        [Display(Name = "Behandelnder Arzt")]
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }

    }
}