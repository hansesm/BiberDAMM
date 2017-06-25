using BiberDAMM.DAL;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using BiberDAMM.ViewModels;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using System.Collections.ObjectModel;
using System;
using System.Data.Entity.Migrations;

namespace BiberDAMM.Controllers
{
    public class TreatmentController : Controller
    {
        //The Database-Context [KrabsJ]
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET Treatment/SelectTreatmentType/id [KrabsJ]
        // first step of creating a new treatment is to select a treatment type
        // this method returns the view for this step
        // expected parameter: id of a stay
        // return: view("SelectTreatmentType", CreationTreatmentSelectType viewModel)
        public ActionResult SelectTreatmentType(int id)
        {
            // check if the stay with the given id exists
            int stayId;
            if (_db.Stays.Any(s => s.Id == id))
            {
                stayId = id;
            }
            else
            {
                // if there is no stay with the given id, show home index page and failure alert
                TempData["UnexpectedFailure"] = " Es konnte kein Aufenthalt mit der übergebenen Id gefunden werden.";
                return RedirectToAction("Index", "Home");
            }

            //get all treatment types from db and put them into a selectList
            var listTreatmentTypes = _db.TreatmentTypes.ToList();
            var selectionListTreatmentTypes = new List<SelectListItem>();
            foreach (var t in listTreatmentTypes)
            {
                selectionListTreatmentTypes.Add(new SelectListItem { Text = (t.Name), Value = (t.Id.ToString()) });
            }

            // create the viewModel for the view
            var treatmentCreationSelectTypeModel = new CreationTreatmentSelectType();
            treatmentCreationSelectTypeModel.StayId = stayId;
            treatmentCreationSelectTypeModel.ListTreatmentTypes = selectionListTreatmentTypes;
            treatmentCreationSelectTypeModel.ClientId = _db.Stays.Where(s => s.Id == stayId).FirstOrDefault().ClientId;
            Client client = _db.Clients.Where(c => c.Id == treatmentCreationSelectTypeModel.ClientId).FirstOrDefault();
            treatmentCreationSelectTypeModel.ClientName = client.Surname + " " + client.Lastname;

            // return view
            return View(treatmentCreationSelectTypeModel);
        }

        //POST: Treatment/SelectTreatmentType [KrabsJ]
        // this method transfers to the create method after selecting a treatment type
        // expected parameter: CreationTreatmentSelectType viewModel1, string command)
        // return: view("Create", CreationTreatmentSelectType viewModel1)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectTreatmentType(CreationTreatmentSelectType treatmentCreationSelectTypeModel, string command)
        {
            // if the abort button was clicked return to the stay details page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Stay", new { id = treatmentCreationSelectTypeModel.StayId });

            // else go to the create method
            return RedirectToAction("Create", "Treatment", treatmentCreationSelectTypeModel);

        }

        // GET: Treatment/Create [KrabsJ]
        // this method return the view "create" that enables the user to create a new treatment
        // the method loads the data that is necessary for creation
        // expected parameter: CreationTreatmentSelectType viewModel1
        // return: view("create", CreationTreatment viewModel2)
        public ActionResult Create(CreationTreatmentSelectType treatmentCreationSelectTypeModel)
        {

            // load data from the CreationTreatmentSelectType-viewmodel to a CreationTreatment-viewmodel
            CreationTreatment treatmentCreationModel = new CreationTreatment();
            treatmentCreationModel.StayId = treatmentCreationSelectTypeModel.StayId;
            treatmentCreationModel.TreatmentTypeId = treatmentCreationSelectTypeModel.TreatmentTypeId;
            treatmentCreationModel.ClientId = treatmentCreationSelectTypeModel.ClientId;
            treatmentCreationModel.ClientName = treatmentCreationSelectTypeModel.ClientName;

            // set defaultDate for calendar in view
            treatmentCreationModel.ShowCalendarDay = treatmentCreationModel.BeginDate.GetValueOrDefault(DateTime.Now).Date.ToString("s");

            //load selectedTreatmentType from db and set attribute TreatmentTypeName of viewModel
            TreatmentType selectedTreatmentType = _db.TreatmentTypes.Where(t => t.Id == treatmentCreationModel.TreatmentTypeId).FirstOrDefault();
            treatmentCreationModel.TreatmentTypeName = selectedTreatmentType.Name;

            //load the rooms that are available for the selectedTreatmentType
            ICollection<Room> rooms = new Collection<Room>();
            if (selectedTreatmentType.RoomTypeId == null)
            {
                rooms = _db.Rooms.ToList();
            }
            else
            {
                rooms = _db.Rooms.Where(r => r.RoomTypeId == selectedTreatmentType.RoomTypeId).ToList();
            }

            //convert the list of rooms to a list of SelectionRooms (this class only contains the attributes that are necessary for creating a new treatment)
            treatmentCreationModel.Rooms = new List<SelectionRoom>();
            foreach (var item in rooms)
            {
                SelectionRoom selectionRoom = new SelectionRoom();
                selectionRoom.Id = item.Id;
                selectionRoom.RoomNumber = item.RoomNumber;
                selectionRoom.RoomTypeName = item.RoomType.Name;
                treatmentCreationModel.Rooms.Add(selectionRoom);
            }

            // get all users (besides cleaners & adminstrators) from the db
            ICollection<ApplicationUser> userList = new Collection<ApplicationUser>();
            userList = _db.Users.Where(u => u.UserType.ToString() != ConstVariables.RoleCleaner && u.UserType.ToString() != ConstVariables.RoleAdministrator).ToList();

            // convert the list of users into a list of staffmembers
            treatmentCreationModel.Staff = new List<Staff>();
            foreach (var item in userList)
            {
                Staff staffMember = new Staff();
                staffMember.Id = item.Id;
                if (item.Title == null)
                {
                    staffMember.DisplayName = item.Surname + " " + item.Lastname;
                }
                else
                {
                    staffMember.DisplayName = item.Title + " "  + item.Surname + " " + item.Lastname;
                }
                staffMember.Selected = false;
                staffMember.StaffType = item.UserType;
                treatmentCreationModel.Staff.Add(staffMember);
            }

            // get all client appointments and store them in the viewModel
            treatmentCreationModel.AppointmentsOfSelectedRessources = GetClientAppointments(treatmentCreationModel.ClientId);

            // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
            if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
            {
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }
            treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

            //return view
            return View(treatmentCreationModel);
        }

        // POST: Treatment/Create [KrabsJ]
        // This method activates the update of the viewModel if a new room or new staff was selected and it stores new treatments in the db
        // expected parameter: CreationTreatment viewModel, string command
        // return: View(CreationTreatment viewModel) or RedirectToAction("Details", "Stay", new { id = treatmentCreationModel.StayId })
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreationTreatment treatmentCreationModel, string command)
        {
            // if abortButton was clicked, go back to details page of stay
            if (command.Equals(ConstVariables.AbortButton))
            {
                return RedirectToAction("Details", "Stay", new { id = treatmentCreationModel.StayId });
            }

            // set defaultDate for calendar in view
            treatmentCreationModel.ShowCalendarDay = treatmentCreationModel.BeginDate.GetValueOrDefault(DateTime.Now).Date.ToString("s");

            // if button "Aktualisieren" was clicked, update the appointments of selected ressources and update the planned treatment to show it in the calendar
            if (command.Equals("Aktualisieren"))
            {
                // update the data for appointments of the client
                // remove the appointments of the client that was found before
                if (treatmentCreationModel.AppointmentsOfSelectedRessources != null)
                {
                    foreach (var appointment in treatmentCreationModel.AppointmentsOfSelectedRessources.ToArray())
                    {
                        if (appointment.Ressource == ConstVariables.AppointmentOfClient)
                        {
                            treatmentCreationModel.AppointmentsOfSelectedRessources.Remove(appointment);
                        }
                    }
                }
                else
                {
                    // for adding new client appointments (see the steps below) it is necessary that the list is not null
                    treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                }
                List<AppointmentOfSelectedRessource> clientAppointments = GetClientAppointments(treatmentCreationModel.ClientId);
                foreach (var appointment in clientAppointments)
                {
                    treatmentCreationModel.AppointmentsOfSelectedRessources.Add(appointment);
                }

                // update data for plannedTreatment, appointments of selected room and appointments of selected staff
                CreationTreatment updatedTreatmentCreationModel = UpdateViewModelByPlannedTreatment(treatmentCreationModel);
                updatedTreatmentCreationModel = UpdateViewModelByRoomSelection(updatedTreatmentCreationModel);
                updatedTreatmentCreationModel = UpdateViewModelByStaffSelection(updatedTreatmentCreationModel);

                // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                if (updatedTreatmentCreationModel.AppointmentsOfSelectedRessources == null)
                {
                    updatedTreatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                }
                updatedTreatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(updatedTreatmentCreationModel.AppointmentsOfSelectedRessources);

                //clear ModeState, so that values are loaded from the updated model
                ModelState.Clear();

                return View(updatedTreatmentCreationModel);
            }

            //if a new room was selected, update the viewModel and return the View again
            if (command.Equals("Raum verwenden"))
            {
                CreationTreatment updatedTreatmentCreationModel = UpdateViewModelByRoomSelection(treatmentCreationModel);
                updatedTreatmentCreationModel = UpdateViewModelByPlannedTreatment(updatedTreatmentCreationModel);

                // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                if (updatedTreatmentCreationModel.AppointmentsOfSelectedRessources == null)
                {
                    updatedTreatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                }
                updatedTreatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(updatedTreatmentCreationModel.AppointmentsOfSelectedRessources);

                //clear ModeState, so that values are loaded from the updated model
                ModelState.Clear();

                return View(updatedTreatmentCreationModel);
            }

            //if new staff was selected, update the viewModel and return the View again
            if (command.Equals("Mitarbeiter einplanen"))
            {
                CreationTreatment updatedTreatmentCreationModel = UpdateViewModelByStaffSelection(treatmentCreationModel);
                updatedTreatmentCreationModel = UpdateViewModelByPlannedTreatment(updatedTreatmentCreationModel);

                // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                if (updatedTreatmentCreationModel.AppointmentsOfSelectedRessources == null)
                {
                    updatedTreatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                }
                updatedTreatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(updatedTreatmentCreationModel.AppointmentsOfSelectedRessources);

                //clear ModeState, so that values are loaded from the updated model
                ModelState.Clear();

                return View(updatedTreatmentCreationModel);
            }

            // if the user want to save the new treatment check if the model is valid
            if (ModelState.IsValid)
            {
                // ensure that BeginDate is before EndDate
                if (treatmentCreationModel.BeginDate > treatmentCreationModel.EndDate)
                {
                    // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                    if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
                    {
                        treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                    }
                    treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

                    // error-message for alert-statement [KrabsJ]
                    TempData["BeginDateEndDateError"] = " Der Behandlungsbeginn darf nicht nach dem Behandlungsende liegen.";

                    // return view
                    return View(treatmentCreationModel);
                }

                // ensure that the treatment is whithin the timeperiod of the stay
                var stay = _db.Stays.Where(s => s.Id == treatmentCreationModel.StayId).FirstOrDefault();
                if (stay.EndDate != null && stay.EndDate < treatmentCreationModel.EndDate)
                {
                    // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                    if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
                    {
                        treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                    }
                    treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

                    // error-message for alert-statement [KrabsJ]
                    TempData["StayClosedError"] = " Der zugehörige Aufenthalt des Patienten wurde am " + stay.EndDate + " beendet. Der Behandlungstermin befindet sich nicht im Zeitraum des Aufenthalts.";

                    // return view
                    return View(treatmentCreationModel);
                }

                // if the new treatment is planed in the future it is necessary to check if there are conflicting appointments
                if (treatmentCreationModel.EndDate > DateTime.Now)
                {
                    // the following steps ensure that there are no conflicts with other appointments
                    // therefore the program checks the dates from db again, because the dates stored in treatmentCreationModel.AppointmentsOfSelectedRessources could be outworn

                    //helper list for unordered conflicts
                    List<AppointmentOfSelectedRessource> unorderedConflicts = new List<AppointmentOfSelectedRessource>();

                    //initialize list of conflicting appointments
                    treatmentCreationModel.ConflictingAppointmentsList = new List<AppointmentOfSelectedRessource>();

                    // check if there are conflicts with other appointments of the client
                    var conflictingClientAppointments = _db.Treatments.Where(t => t.EndDate > DateTime.Now && t.Stay.ClientId == treatmentCreationModel.ClientId && t.BeginDate < treatmentCreationModel.EndDate && treatmentCreationModel.BeginDate < t.EndDate).ToList();
                    if (conflictingClientAppointments.Count > 0)
                    {
                        foreach (var appointment in conflictingClientAppointments)
                        {
                            AppointmentOfSelectedRessource newConflict = new AppointmentOfSelectedRessource();
                            newConflict.Id = appointment.Id;
                            newConflict.BeginDate = appointment.BeginDate;
                            newConflict.EndDate = appointment.EndDate;
                            newConflict.Ressource = "Patient";
                            unorderedConflicts.Add(newConflict);
                        }
                    }

                    // check if there are conflicts with other appointments of the selected room
                    var conflictingRoomAppointments = _db.Treatments.Where(t => t.EndDate > DateTime.Now && t.RoomId == treatmentCreationModel.SelectedRoomId && t.BeginDate < treatmentCreationModel.EndDate && treatmentCreationModel.BeginDate < t.EndDate).ToList();
                    if (conflictingRoomAppointments.Count > 0)
                    {
                        foreach (var appointment in conflictingRoomAppointments)
                        {
                            AppointmentOfSelectedRessource newConflict = new AppointmentOfSelectedRessource();
                            newConflict.Id = appointment.Id;
                            newConflict.BeginDate = appointment.BeginDate;
                            newConflict.EndDate = appointment.EndDate;
                            newConflict.Ressource = "Raum: " + treatmentCreationModel.SelectedRoomNumber;
                            unorderedConflicts.Add(newConflict);
                        }
                    }

                    // check if there are conflicts with other appointments of the selected staffmembers
                    if (treatmentCreationModel.SelectedStaff != null)
                    {
                        foreach (var staffMember in treatmentCreationModel.SelectedStaff)
                        {
                            var conflictingStaffAppointments = _db.Treatments.Where(t => t.EndDate > DateTime.Now && t.ApplicationUsers.Any(a => a.Id == staffMember.Id) && t.BeginDate < treatmentCreationModel.EndDate && treatmentCreationModel.BeginDate < t.EndDate).ToList();
                            if (conflictingStaffAppointments.Count > 0)
                            {
                                foreach (var appointment in conflictingStaffAppointments)
                                {
                                    AppointmentOfSelectedRessource newConflict = new AppointmentOfSelectedRessource();
                                    newConflict.Id = appointment.Id;
                                    newConflict.BeginDate = appointment.BeginDate;
                                    newConflict.EndDate = appointment.EndDate;
                                    newConflict.Ressource = staffMember.DisplayName;
                                    unorderedConflicts.Add(newConflict);
                                }
                            }
                        }
                    }

                    // store an ordered list of the conflicts in the viewModel
                    treatmentCreationModel.ConflictingAppointmentsList = unorderedConflicts.OrderBy(c => c.BeginDate).ToList();

                    // if there are any conflicts the treatment is not stored in the db
                    if (treatmentCreationModel.ConflictingAppointmentsList.Count > 0)
                    {
                        // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
                        if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
                        {
                            treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                        }
                        treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

                        // error-message for alert-statement [KrabsJ]
                        TempData["ConflictingAppointments"] = " Es wurden Konflikte mit anderen Terminen gefunden. ";

                        // return view
                        return View(treatmentCreationModel);
                    }
                }

                // this point is reached if there are no conflicting appointments or if the treatment is stored retroactive
                // a treatment that is stored retroactive (in the past) doesn't have to be checked on conflicts

                List<ApplicationUser> userList = new List<ApplicationUser>();
                foreach (var staffMember in treatmentCreationModel.SelectedStaff)
                {
                    var user = _db.Users.Single(u => u.Id == staffMember.Id);
                    userList.Add(user);
                }

                // create the new treatment and store or update it in the db
                var newTreatment = new Treatment
                {
                    BeginDate = treatmentCreationModel.BeginDate.GetValueOrDefault(DateTime.Now),
                    EndDate = treatmentCreationModel.EndDate.GetValueOrDefault(DateTime.Now),
                    StayId = treatmentCreationModel.StayId,
                    RoomId = treatmentCreationModel.SelectedRoomId,
                    Description = treatmentCreationModel.Description,
                    TreatmentTypeId = treatmentCreationModel.TreatmentTypeId,
                    ApplicationUsers = userList,
                };

                _db.Treatments.AddOrUpdate(newTreatment);
                _db.SaveChanges();

                // success-message for alert-statement
                TempData["NewTreatmentSuccess"] = " Die neue Behandlung wurde gespeichert.";

                // go back to stays details page
                return RedirectToAction("Details", "Stay", new { id = treatmentCreationModel.StayId });
            }

            // this point is only reached if the modal was not valid when trying to save the new treatment
            // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
            if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
            {
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }
            treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

            // return view
            return View(treatmentCreationModel);
        }

        //[KrabsJ]
        //this method updates the viewModel data depending on the selected room
        //expected parameter: CreationTreatment viewModel
        //return: CreationTreatment viewModel
        private CreationTreatment UpdateViewModelByRoomSelection(CreationTreatment treatmentCreationModel)
        {
            // if there was no room selected the selectedRoomId is 0 [db-IDs start with 1]
            // in this case no data has to be updated
            if (treatmentCreationModel.SelectedRoomId != 0)
            {
                // load the selectedRoomNumber from db and set the ViewModel-attribute
                string selectedRoomNumber = _db.Rooms.Where(r => r.Id == treatmentCreationModel.SelectedRoomId).FirstOrDefault().RoomNumber;
                treatmentCreationModel.SelectedRoomNumber = selectedRoomNumber;

                // remove the appointments of the room that was selected before
                if (treatmentCreationModel.AppointmentsOfSelectedRessources != null)
                {
                    foreach (var appointment in treatmentCreationModel.AppointmentsOfSelectedRessources.ToArray())
                    {
                        if (appointment.Ressource == ConstVariables.AppointmentOfRoom)
                        {
                            treatmentCreationModel.AppointmentsOfSelectedRessources.Remove(appointment);
                        }
                    }
                }
                else
                {
                    // for adding new appointments (see the steps below) it is necessary that the list is not null
                    treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
                }

                // load the appointments of the selected room from db
                var newRoomAppointments = _db.Treatments.Where(t => t.EndDate > DateTime.Now && t.RoomId == treatmentCreationModel.SelectedRoomId).ToList();

                // convert these appointments (class treatment) into objects of AppointmentOfSelectedRessource and add them to treatmentCreationModel.AppointmentsOfSelectedRessources
                foreach (var appointment in newRoomAppointments)
                {
                    AppointmentOfSelectedRessource appointmentOfSelectedRoom = new AppointmentOfSelectedRessource();
                    appointmentOfSelectedRoom.Id = appointment.Id;
                    appointmentOfSelectedRoom.BeginDate = appointment.BeginDate;
                    appointmentOfSelectedRoom.EndDate = appointment.EndDate;
                    appointmentOfSelectedRoom.Ressource = ConstVariables.AppointmentOfRoom;
                    appointmentOfSelectedRoom.EventColor = "#32CD32";
                    treatmentCreationModel.AppointmentsOfSelectedRessources.Add(appointmentOfSelectedRoom);
                }
            }

            // return ViewModel
            return treatmentCreationModel;
        }

        //[KrabsJ]
        //this method updates the viewModel data depending on the selected staffMembers
        //expected parameter: CreationTreatment viewModel
        //return: CreationTreatment viewModel
        private CreationTreatment UpdateViewModelByStaffSelection(CreationTreatment treatmentCreationModel)
        {
            //initialize list of selectedStaff (delete old selection)
            treatmentCreationModel.SelectedStaff = new List<Staff>();

            //helper list for unordered staffmembers
            List<Staff> unorderedStaff = new List<Staff>();

            // write selected staffmembers into the unordered list
            if (treatmentCreationModel.Staff != null)
            {
                foreach (var item in treatmentCreationModel.Staff)
                {
                    if (item.Selected == true)
                    {
                        unorderedStaff.Add(item);
                    }
                }
            }

            // sort list by DisplayName and write it into the list of viewModel
            treatmentCreationModel.SelectedStaff = unorderedStaff.OrderBy(s => s.DisplayName).ToList();

            // remove the appointments of the staffmembers that were selected before
            if (treatmentCreationModel.AppointmentsOfSelectedRessources != null)
            {
                foreach (var appointment in treatmentCreationModel.AppointmentsOfSelectedRessources.ToArray())
                {
                    if (appointment.Ressource != ConstVariables.AppointmentOfRoom && appointment.Ressource != ConstVariables.AppointmentOfClient && appointment.Ressource != ConstVariables.PlannedTreatment)
                    {
                        treatmentCreationModel.AppointmentsOfSelectedRessources.Remove(appointment);
                    }
                }
            }
            else
            {
                // for adding new appointments (see the steps below) it is necessary that the list is not null
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }

            // get the appointments of each selected staffmember
            foreach (var staffMember in treatmentCreationModel.SelectedStaff)
            {
                // load the appointments of the selected staffmember from db
                var newStaffAppointments = _db.Treatments.Where(t => t.EndDate > DateTime.Now && t.ApplicationUsers.Any(a => a.Id == staffMember.Id)).ToList();

                // convert these appointments (class treatment) into objects of AppointmentOfSelectedRessource and add them to treatmentCreationModel.AppointmentsOfSelectedRessources
                foreach (var appointment in newStaffAppointments)
                {
                    AppointmentOfSelectedRessource appointmentOfSelectedStaffMember = new AppointmentOfSelectedRessource();
                    appointmentOfSelectedStaffMember.Id = appointment.Id;
                    appointmentOfSelectedStaffMember.BeginDate = appointment.BeginDate;
                    appointmentOfSelectedStaffMember.EndDate = appointment.EndDate;
                    appointmentOfSelectedStaffMember.Ressource = staffMember.DisplayName;
                    appointmentOfSelectedStaffMember.EventColor = "#FFD700";
                    treatmentCreationModel.AppointmentsOfSelectedRessources.Add(appointmentOfSelectedStaffMember);
                }
            }

            //return ViewModel
            return treatmentCreationModel;
        }

        //[KrabsJ]
        //this method returns all appointments (in the future) of the client
        //expected parameter: int clientId
        //return: List<AppointmentOfSelectedRessource> clientAppointments
        private List<AppointmentOfSelectedRessource> GetClientAppointments(int clientId)
        {
            // get all stays of the client that are not finished
            var CurrentClientStays = _db.Stays.Where(s => s.ClientId == clientId && (s.EndDate == null || s.EndDate > DateTime.Now)).ToList();

            //extract the treatments of theese stays that are in the future 
            ICollection<Treatment> ClientTreatments = new Collection<Treatment>();
            foreach (var stay in CurrentClientStays)
            {
                foreach (var treatment in stay.Treatments)
                {
                    if (treatment.EndDate > DateTime.Now)
                    {
                        ClientTreatments.Add(treatment);
                    }
                }
            }

            //convert the list of treatments to a list of AppointmentsOfSelectedRessource (this class only contains the attributes that are necessary for creating a new treatment)
            List<AppointmentOfSelectedRessource> clientAppointments = new List<AppointmentOfSelectedRessource>();
            foreach (var treatment in ClientTreatments)
            {
                AppointmentOfSelectedRessource appointmentOfClient = new AppointmentOfSelectedRessource();
                appointmentOfClient.Id = treatment.Id;
                appointmentOfClient.BeginDate = treatment.BeginDate;
                appointmentOfClient.EndDate = treatment.EndDate;
                appointmentOfClient.Ressource = ConstVariables.AppointmentOfClient;
                clientAppointments.Add(appointmentOfClient);
            }

            return clientAppointments;
        }

        //[KrabsJ]
        //this method updates the viewModel data depending on the BeginDate and EndDate
        //expected parameter: CreationTreatment viewModel
        //return: CreationTreatment viewModel
        private CreationTreatment UpdateViewModelByPlannedTreatment(CreationTreatment treatmentCreationModel)
        {
            // remove the new treatment that was planned before
            if (treatmentCreationModel.AppointmentsOfSelectedRessources != null)
            {
                foreach (var appointment in treatmentCreationModel.AppointmentsOfSelectedRessources.ToArray())
                {
                    if (appointment.Ressource == ConstVariables.PlannedTreatment)
                    {
                        treatmentCreationModel.AppointmentsOfSelectedRessources.Remove(appointment);
                    }
                }
            }
            else
            {
                // for adding a new planned treatment (see the steps below) it is necessary that the list is not null
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }

            // store a new planned treatment in the viewModel if the time inputs (BeginDate & EndDate) are valid
            if (treatmentCreationModel.BeginDate != null && treatmentCreationModel.EndDate != null && (treatmentCreationModel.BeginDate < treatmentCreationModel.EndDate))
            {
                AppointmentOfSelectedRessource plannedTreatment = new AppointmentOfSelectedRessource()
                {
                    //all appointments that come from db have an Id of 1 or higher --> so the Id=0 is free
                    Id = 0,
                    BeginDate = treatmentCreationModel.BeginDate.GetValueOrDefault(DateTime.Now),
                    EndDate = treatmentCreationModel.EndDate.GetValueOrDefault(DateTime.Now),
                    Ressource = ConstVariables.PlannedTreatment,
                    EventColor = "#FF8C00"
                };
                treatmentCreationModel.AppointmentsOfSelectedRessources.Add(plannedTreatment);
            }

            return treatmentCreationModel;
        }

        //helper method for creating the JsonResult, this is required for the calendar in the create-view [KrabsJ]
        private JsonResult CreateJsonResult(IList<AppointmentOfSelectedRessource> appointmentList)
        {
            //Builds a JSon from the appointmentList
            var result = appointmentList.Select(a => new JsonEventTreatment()
            {
                start = a.BeginDate.ToString("s"),
                end = a.EndDate.ToString("s"),
                title = a.Ressource,
                id = a.Id.ToString(),
                color = a.EventColor,
            }).ToList();

            //Creates a JsonResult from the Json
            JsonResult resultJson = new JsonResult { Data = result };

            return resultJson;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}