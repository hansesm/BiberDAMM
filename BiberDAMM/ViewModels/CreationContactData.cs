using BiberDAMM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    public class CreationContactData
    {
        [Display(Name = "Kontaktbeschreibung")]
        public string Description { get; set; }

        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Telefonnummer")]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Mobiltelefonnummer")]
        [Phone]
        public string Mobile { get; set; }

        [Display(Name = "Straße und Hausnummer")]
        public string Street { get; set; }

        [Display(Name = "Postleitzahl")]
        public string Postcode { get; set; }

        [Display(Name = "Stadt")]
        public string City { get; set; }


        public int ClientId { get; set; }

        public virtual ICollection<ContactType> ContactType { get; set; }
    }
}