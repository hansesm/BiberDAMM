using BiberDAMM.DAL;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using BiberDAMM.ViewModels;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System;

namespace BiberDAMM.Controllers
{
    public class TreatmentController : Controller
    {
        //The Database-Context [KrabsJ]
        private ApplicationDbContext _db = new ApplicationDbContext();

        //TODO: [KrabsJ] place class JsonEvent to Helpers folder
        //Inline-Class for for displaying treatment-data in a calendar view, needed in the Details-Method [HansesM]
        public class JsonEvent
        {
            public string id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

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

            // get all stays of the client that are not finished
            var CurrentClientStays = _db.Stays.Where(s => s.ClientId == treatmentCreationModel.ClientId && (s.EndDate == null || s.EndDate > DateTime.Now)).ToList();

            //extract the treatments of theese stays that are in the future 
            ICollection <Treatment> ClientTreatments = new Collection<Treatment>();
            foreach (var stay in CurrentClientStays)
            {
                foreach(var treatment in stay.Treatments)
                {
                    if (treatment.End > DateTime.Now)
                    {
                        ClientTreatments.Add(treatment);
                    }
                }
            }

            //convert the list of treatments to a list of AppointmentsOfSelectedRessource (this class only contains the attributes that are necessary for creating a new treatment)
            treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            foreach (var treatment in ClientTreatments)
            {
                AppointmentOfSelectedRessource appointmentOfClient = new AppointmentOfSelectedRessource();
                appointmentOfClient.Id = treatment.Id;
                appointmentOfClient.Begin = treatment.Begin;
                appointmentOfClient.End = treatment.End;
                appointmentOfClient.Ressource = "Patientenbehandlung";
                treatmentCreationModel.AppointmentsOfSelectedRessources.Add(appointmentOfClient);
            }

            // create JsonResult for calendar in view
            treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

            //return view
            return View(treatmentCreationModel);
        }

        // GET: Treatment/Create [KrabsJ]
        // TODO: this is just a dummy method to check if validation already works
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreationTreatment treatmentCreationModel)
        {
            // check if model is valid
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
            if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
            {
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }
            treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

            // return view
            return View(treatmentCreationModel);
        }

        //POST: Treatment/UpdateCreatePageByRoomSelection [KrabsJ]
        //this method returns the view "create" with an updated viewModel
        //it updates the viewModel data depending on the selected room
        //expected parameter: CreationTreatment viewModel
        //return: view("create", CreationTreatment viewModel)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCreatePageByRoomSelection(CreationTreatment treatmentCreationModel)
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
                        if (appointment.Ressource == "Raum belegt")
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
                var newRoomAppointments = _db.Treatments.Where(t => t.RoomId == treatmentCreationModel.SelectedRoomId && t.End > DateTime.Now).ToList();

                // convert these appointments (class treatment) into objects of AppointmentOfSelectedRessource and add them to treatmentCreationModel.AppointmentsOfSelectedRessources
                foreach (var appointmnet in newRoomAppointments)
                {
                    AppointmentOfSelectedRessource appointmentOfSelectedRoom = new AppointmentOfSelectedRessource();
                    appointmentOfSelectedRoom.Id = appointmnet.Id;
                    appointmentOfSelectedRoom.Begin = appointmnet.Begin;
                    appointmentOfSelectedRoom.End = appointmnet.End;
                    appointmentOfSelectedRoom.Ressource = "Raum belegt";
                    treatmentCreationModel.AppointmentsOfSelectedRessources.Add(appointmentOfSelectedRoom);
                }
            }

            // create JsonResult for calendar in view (therefore it is necessary that the list of appointments is not null)
            if (treatmentCreationModel.AppointmentsOfSelectedRessources == null)
            {
                treatmentCreationModel.AppointmentsOfSelectedRessources = new List<AppointmentOfSelectedRessource>();
            }
            treatmentCreationModel.JsonAppointmentsOfSelectedRessources = CreateJsonResult(treatmentCreationModel.AppointmentsOfSelectedRessources);

            //clear ModeState, so that values are loaded from the updated model
            ModelState.Clear();

            // return view
            return View("Create", treatmentCreationModel);
        }

        //helper method for creating the JsonResult, this is required for the calendar in the create-view [KrabsJ]
        private JsonResult CreateJsonResult(IList<AppointmentOfSelectedRessource> appointmentList)
        {
            //Builds a JSon from the appointmentList
            var result = appointmentList.Select(a => new JsonEvent()
            {
                start = a.Begin.ToString("s"),
                end = a.End.ToString("s"),
                title = a.Ressource,
                id = a.Id.ToString()

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