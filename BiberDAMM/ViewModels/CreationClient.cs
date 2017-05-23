using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public class CreationClient
    {
        // ===============================
        // AUTHOR     : ChristesR
        // ===============================

        [Required]
        //        [RegularExpression("/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$/u")]
        [Display(Name = "Vormame")]
        public string Surname { get; set; }

        [Required]
        //        [RegularExpression("/^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$/u")]
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

        [Display(Name = "Versicherung")]
        public virtual ICollection<HealthInsurance> HealthInsurances { get; set; }
    }
}