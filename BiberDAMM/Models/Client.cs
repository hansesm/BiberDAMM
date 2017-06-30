using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
//    Initializing enum for genders
    public enum Sex
    {
        männlich,
        weiblich,
        unbekannt
    }

    public class Client
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Vorname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        public Sex Sex { get; set; }

        [Required]
        [Display(Name = "Geburtsdatum")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Versichertennummer")]
        public int? InsuranceNumber { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Erfassung")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Captured { get; set; }

        [Display(Name = "Letzte Aktualisierung")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; }
        
        [Display(Name = "Datensatzversion")]
        public int RowVersion { get; set; }

        [Display(Name = "Versicherung")]
        public int? HealthInsuranceId { get; set; }

        public virtual HealthInsurance HealthInsurance { get; set; }

        public virtual ICollection<ContactData> ContactData { get; set; }

        public virtual ICollection<Stay> Stay { get; set; }
    }
}