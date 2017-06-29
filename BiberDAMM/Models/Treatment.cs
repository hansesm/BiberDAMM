using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiberDAMM.Models
{
    public class Treatment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Behandlungsbeginn")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Display(Name = "Behandlungsende")]
        public DateTime EndDate { get; set; }


        public int StayId { get; set; }
        public virtual Stay Stay { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        public DateTime UpdateTimeStamp { get; set; }

        public int TreatmentTypeId { get; set; }
        public virtual TreatmentType TreatmentType { get; set; }

        public int? IdOfSeries { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}