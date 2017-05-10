using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.Models
{
    public class Treatment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Behandlungsbeginn")]
        public DateTime Begin { get; set; }

        [Required]
        [Display(Name = "Behandlungsende")]
        public DateTime End { get; set; }


        public int StayId { get; set; }
        public virtual Stay Stay { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
        
        public int ThreatmentTypeId { get; set; }
        public virtual TreatmentType TreatmentType { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}