using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
//    Initializing enum for genders
    public enum Sex
    {
        männlich, weiblich, unbekannt
    }

    public class Client
    {

        public int Id { get; set; }
         // TODO Check if working and names like Ügüglülü can be saved

        [Required]
        [RegularExpression("/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$/u")]
        [Display(Name = "Vormame")]
        public string Surname { get; set; }

        [Required]
        [RegularExpression("/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$/u")]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        public Sex Sex { get; set; }

        [Required]
        [Display(Name = "Geburtsdatum")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Versicherungsnummer")]
        public int? InsuranceNumber { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        public DateTime Captured { get; set; }

        public DateTime LastUpdated { get; set; }

        public int RowVersion { get; set; }

        [Display(Name = "Versicherung")]
        public int? HealthInsuranceId { get; set; }
        public virtual HealthInsurance HealthInsurance { get; set; }

        public virtual ICollection<ContactData> ContactData { get; set; }

        public virtual ICollection<Stay> Stay { get; set; }
    }
}