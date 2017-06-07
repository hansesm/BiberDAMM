using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public enum StayType
    {
        ambulant,
        stationär
    }

    public class Stay
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Aufnahmezeitpunkt")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Entlassungszeitpunkt")]
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

        //Rowversion will be updated in the controller [HansesM]
        public int RowVersion { get; set; }

        public DateTime LastUpdated { get; set; }

        [Display(Name = "Patient")]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Display(Name = "Behandelnder Arzt")]
        public int ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Blocks> Blocks { get; set; }

        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}