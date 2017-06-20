using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    // This is a ViewModel used to create a new treatment [KrabsJ]
    public class CreationTreatment
    {
        // general attributes
        public int StayId { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int TreatmentTypeId { get; set; }

        [Display(Name = "Behandlungstyp")]
        public string TreatmentTypeName { get; set; }

        // treatment details
        [Required]
        [Display(Name = "Behandlungsbeginn")]
        public DateTime? Begin { get; set; }

        [Required]
        [Display(Name = "Behandlungsende")]
        public DateTime? End { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        // attributes for room selection
        public IList<SelectionRoom> Rooms { get; set; }

        [Required]
        public int SelectedRoomId { get; set; }

        [Required]
        [Display(Name = "Raum")]
        public string SelectedRoomNumber { get; set; }

        // attributes for calendar part
        public IList<AppointmentOfSelectedRessource> AppointmentsOfSelectedRessources { get; set; }

        public JsonResult JsonAppointmentsOfSelectedRessources { get; set; }
    }

    //this class only implements the attributes of the class "room" that are necessary for creating a new treatment [KrabsJ]
    public class SelectionRoom
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Display(Name = "Raumtyp")]
        public string RoomTypeName { get; set; }
    }

    // [KrabsJ]
    // this class implements an appointment (less attributes than an normal treatment)
    // it is used for showing the appointments of the selected Room, client and Users when creating a new treatment
    public class AppointmentOfSelectedRessource
    {
        public int Id { get; set; }

        public string Ressource { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}