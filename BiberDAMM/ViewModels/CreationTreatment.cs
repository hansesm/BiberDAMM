using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    // This is a ViewModel used to create a new treatment
    public class CreationTreatment
    {
        //[Required]
        [Display(Name = "Behandlungsbeginn")]
        public DateTime Begin { get; set; }

        //[Required]
        [Display(Name = "Behandlungsende")]
        public DateTime End { get; set; }

        [Required]
        public int StayId { get; set; }

        public IList<SelectionRoom> Rooms { get; set; }

        [Required]
        public int SelectedRoomId { get; set; }

        [Required]
        [Display(Name = "Raum")]
        public string SelectedRoomNumber { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Behandlungstyp")]
        public string TreatmentTypeName { get; set; }

        [Required]
        public int TreatmentTypeId { get; set; }
    }

    //this class only implements the attributes of the class "room" that are necessary for creating a new treatment
    public class SelectionRoom
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Display(Name = "Raumtyp")]
        public string RoomTypeName { get; set; }
    }
}