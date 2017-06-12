using BiberDAMM.DAL;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using BiberDAMM.ViewModels;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BiberDAMM.Controllers
{
    public class TreatmentController : Controller
    {
        //The Database-Context [KrabsJ]
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Treatment/Create [KrabsJ]
        // this method return the view "create" that enables the user to create a new treatment
        // the method loads the data that is necessary for creation
        // expected parameter: CreationTreatmentSelectType viewModel1
        // return: view("create", CreationTreatment viewModel2)
        public ActionResult Create(CreationTreatmentSelectType model)
        {
            // load data from the CreationTreatmentSelectType-viewmodel to a CreationTreatment-viewmodel
            CreationTreatment viewModelCreationTreatment = new CreationTreatment();
            viewModelCreationTreatment.StayId = model.StayId;
            viewModelCreationTreatment.TreatmentTypeId = model.TreatmentTypeId;

            //load selectedTreatmentType from db and set attribute TreatmentTypeName of viewModel
            TreatmentType selectedTreatmentType = _db.TreatmentTypes.Where(t => t.Id == model.TreatmentTypeId).FirstOrDefault();
            viewModelCreationTreatment.TreatmentTypeName = selectedTreatmentType.Name;

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
            viewModelCreationTreatment.Rooms = new List<SelectionRoom>();
            foreach (var item in rooms)
            {
                SelectionRoom selectionRoom = new SelectionRoom();
                selectionRoom.Id = item.Id;
                selectionRoom.RoomNumber = item.RoomNumber;
                selectionRoom.RoomTypeName = item.RoomType.Name;
                viewModelCreationTreatment.Rooms.Add(selectionRoom);
            }

            //return view
            return View(viewModelCreationTreatment);
        }

        // GET: Treatment/Create [KrabsJ]
        // TODO: this is just a dummy method to check if validation already works
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreationTreatment treatmentCreationModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(treatmentCreationModel);
        }

        // GET Treatment/SelectTreatmentType/id [KrabsJ]
        // first step of creating a new treatment is to select a treatment type
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

            //get all treatment types from db an put them into a selectList
            var listTreatmentTypes = _db.TreatmentTypes.ToList();

            var selectetListTreatmentTypes = new List<SelectListItem>();
            foreach (var t in listTreatmentTypes)
            {
                selectetListTreatmentTypes.Add(new SelectListItem { Text = (t.Name), Value = (t.Id.ToString()) });
            }

            // create the viewModel
            var viewModelSelectTreatmentType = new CreationTreatmentSelectType();
            viewModelSelectTreatmentType.StayId = stayId;
            viewModelSelectTreatmentType.ListTreatmentTypes = selectetListTreatmentTypes;

            return View(viewModelSelectTreatmentType);
        }

        //POST: Treatment/SelectTreatmentType [KrabsJ]
        // first step of creating a new treatment is to select a treatment type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectTreatmentType(CreationTreatmentSelectType model, string command)
        {
            // if the abort button was clicked return to the stay details page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Stay", new { id = model.StayId });

            // else go to the create method
            return RedirectToAction("Create", "Treatment", model);

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
            // if there was no room selected the slectedRoomId is 0 [db-IDs start with 1]
            // in this case no data has to be updated
            if (treatmentCreationModel.SelectedRoomId != 0)
            {
                // load the selectedRoomNumber from db and set the ViewModel-attribute
                string selectedRoomNumber = _db.Rooms.Where(r => r.Id == treatmentCreationModel.SelectedRoomId).FirstOrDefault().RoomNumber;
                treatmentCreationModel.SelectedRoomNumber = selectedRoomNumber;

                //clear ModeState, so that values are loaded from the updated model
                ModelState.Clear();
            }
            return View("Create", treatmentCreationModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}