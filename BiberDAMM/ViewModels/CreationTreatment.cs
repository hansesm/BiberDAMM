using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiberDAMM.Models;
using System.Web.Mvc;

namespace BiberDAMM.ViewModels
{
    public enum Series
    {
        noSeries,
        day,
        week,
        twoWeeks,
        month,
    }

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
        public DateTime? BeginDate { get; set; }

        [Required]
        [Display(Name = "Behandlungsende")]
        public DateTime? EndDate { get; set; }

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

        // attributes for staff selection
        public IList<Staff> Staff { get; set; }

        public IList<Staff> SelectedStaff { get; set; }

        // attributes for calendar part
        public IList<AppointmentOfSelectedRessource> AppointmentsOfSelectedRessources { get; set; }

        public JsonResult JsonAppointmentsOfSelectedRessources { get; set; }

        // attributes for showing conflicting appointments
        public IList<AppointmentOfSelectedRessource> ConflictingAppointmentsList { get; set; }

        // attribute for displaying the right day in calendar
        public string ShowCalendarDay { get; set; }

        // attribute for planning a cleaning event
        public CleaningDuration CleaningDuration { get; set; }

        // attributes for planning a series of treatments
        public Series Series { get; set; }
        [Display(Name ="Anzahl Wiederholungen")]
        [Range(0, int.MaxValue, ErrorMessage = "Der eingegebene Wert befindet sich ausserhalb des zulässigen Wertebereichs")]
        public int SeriesCounter { get; set; }
    }

    //this class only implements the attributes of the class "room" that are necessary for creating a new treatment [KrabsJ]
    public class SelectionRoom
    {
        public int Id { get; set; }

        [Display(Name = "Raumnummer")]
        public string RoomNumber { get; set; }

        [Display(Name = "Raumtyp")]
        public string RoomTypeName { get; set; }
    }

    // [KrabsJ]
    // this class implements an appointment (less attributes than a normal treatment)
    // it is used for showing the appointments of the selected Room, client and Users when creating a new treatment
    public class AppointmentOfSelectedRessource
    {
        [Display(Name = "Betroffene Ressource")]
        public string Ressource { get; set; }

        [Display(Name = "Anfang")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Ende")]
        public DateTime EndDate { get; set; }

        public string EventColor { get; set; }
    }

    // [KrabsJ]
    // this class implements staffmembers (less attributes than a normal ApplicationUser)
    // it is used for selecting users during the process of creating a new treatment
    public class Staff
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Typ")]
        public UserType StaffType { get; set; }

        [Display(Name = "Auswahl")]
        public bool Selected { get; set; }
    }
}