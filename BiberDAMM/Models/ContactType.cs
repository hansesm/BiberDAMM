using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    public class ContactType
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Kontakttyp")]
        public string Name { get; set; }

        public virtual ICollection<ContactData> ContactData { get; set; }
    }
}