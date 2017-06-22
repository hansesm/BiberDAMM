using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET all: Room [JEL]
        public ActionResult Index()
        {   // returns listed rooms
            return View(db.Rooms.ToList());
        }

        //CREATE: Room [JEL]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room Room)
        {
            //checks if roomNumber is already in use; an alert will be displayed
            if (db.Rooms.Any(r => r.RoomNumber.Equals(Room.RoomNumber)))
            {
                TempData["CreateRoomFailed"] = " " + Room.RoomNumber + " existiert bereits.";
                return RedirectToAction("Create", Room);
            }
            // checks if ModelState is valid, if this is the case the room will be saved 
            if (ModelState.IsValid)
            {
                db.Rooms.Add(Room);
                db.SaveChanges();
                TempData["CreateRoomSuccess"] = " " + Room.RoomNumber + " wurde hinzugefügt.";
                return RedirectToAction("Index");
            }
            //Get all roomTypes from the database           
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
                selectedListRoomTypes.Add(new SelectListItem {Text = m.Name, Value = m.Id.ToString()});
            //creats viewModel
            var viewModel = new RoomViewModel(Room, selectedListRoomTypes);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
                selectedListRoomTypes.Add(new SelectListItem {Text = m.Name, Value = m.Id.ToString()});
            var room = new Room();
            //creates viewModel
            var viewModel = new RoomViewModel(room, selectedListRoomTypes);
            return View(viewModel);
        }

        //DELETE: Room [JEL]
        public ActionResult Delete(int? id)
        {   
            //checks if given id is equal null, if this is the case return to "index-view" 
            if (id == null)
                return RedirectToAction("Index");

            //checks if room with given id exists
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();
            return View(room);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // check if there are dependencies
            var dependentBed = db.Beds.Where(b => b.RoomId == id).FirstOrDefault();
            var dependentTreatment = db.Rooms.Where(t => t.Id == id).SelectMany(u => u.Treatment).FirstOrDefault();
            //Treatment dependentTreatment = db.Treatments.Where(t => t.RoomId == id).FirstOrDefault();

            // if there is a treatment or bed that is linked to the room, the room can't be deleted
            if (dependentBed != null || dependentTreatment != null)
            {
                // failure-message for alert-statement
                TempData["DeleteRoomFailed"] = " Es bestehen Abhängigkeiten zu anderen Krankenhausdaten.";
                return RedirectToAction("Details", "Room", new {id});
            }
            //deletes room
            var room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            TempData["DeleteRoomSuccess"] = " " + room.RoomNumber + " wurde entfernt.";
            return RedirectToAction("Index");
        }

        //CHANGE: Room [JEL] 
        public ActionResult Edit(int? id)
        {
            //checks if given id is equal null, if this is the case return to "index-view" 
            if (id == null)
                return RedirectToAction("Index");
            //checks if room with given id exists
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();

            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            //selectedListRoomTypes.Add(new SelectListItem{ Text = " ", Value = null });
            foreach (var m in listRoomTypes)
                selectedListRoomTypes.Add(new SelectListItem {Text = m.Name, Value = m.Id.ToString()});
            //creates viewModel
            var viewModel = new RoomViewModel(room, selectedListRoomTypes);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room, string command)
        {   
           
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Room", new {id = room.Id});
            //checks if roomNumber exists already and if the current id is not equal to an existing id
            if (db.Rooms.Any(r => r.RoomNumber.Equals(room.RoomNumber) && !r.Id.Equals(room.Id)))
            {
                TempData["EditRoomFailed"] = " " + room.RoomNumber + " existiert bereits.";
                return RedirectToAction("Edit", room);
            }


            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                //if update succeeded
                TempData["EditRoomSuccess"] = " Die Raumdetails wurden aktualisiert.";
                return RedirectToAction("Details", "room", new {id = room.Id});
            }
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
                selectedListRoomTypes.Add(new SelectListItem {Text = m.Name, Value = m.Id.ToString()});

            //creates viewModel
            var viewModel = new RoomViewModel(room, selectedListRoomTypes);
            return View(viewModel);
        }

        //GET SINGLE: Room [JEL]
        public ActionResult Details(int? id)
        {
            //checks if given id is equal null, if this is the case return to "index-view" 
            if (id == null)
                return RedirectToAction("Index");
            //checks if room with given id exists
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();
            //returns room
            return View(room);
        }

        public ActionResult RoomScheduler()
        {
            //Gets a list of RoomTypes for the Dropdownlist [HansesM]
            var listRoomTypes = db.RoomTypes;

            //Builds a selectesList out of the list of RoomTypes [HansesM]
            var selectetListRoomTypes = new List<SelectListItem>();
            selectetListRoomTypes.Add(new SelectListItem());
            foreach (var m in listRoomTypes)
                selectetListRoomTypes.Add(new SelectListItem {Text = m.Name, Value = m.Id.ToString()});

            var viewModel = new RoomSchedulerViewModel(selectetListRoomTypes);

            return View(viewModel);
        }

        //Post-method witch will be called by room-scheduler
        //Jquery-Ajax and returns a list of rooms witch matches the given roomType
        //[HansesM]
        [HttpPost]
        public JsonResult GetSchedulerRooms(string roomTypeName)
        {
            //Gets a list of free beds, matching the given parameters! [HansesM]
            var rooms = db.Rooms.Where(m => m.RoomType.Name.Equals(roomTypeName)).ToList();

            //Creates a array of roomnames out of the given rooms-list[HansesM]
            var result = (from a in rooms
                select a.RoomNumber).ToArray();


            //returns the array as json to the calling-function [HansesM]
            return Json(result);
        }

        //Post-method witch will be called by room-scheduler
        //Jquery-Ajax and returns a list of treatments witch matches roomtype and the current-date
        //[HansesM]
        [HttpPost]
        public JsonResult GetSchedulerEvents(string roomTypeName)
        {
            //Gets a list of treatments, matching the given parameters! [HansesM]
            var events = db.Treatments.SqlQuery(
                "select * from treatments t where t.RoomId in (select ro.id from rooms ro where ro.RoomTypeId = (select rt.Id from RoomTypes rt where rt.name like '" +
                roomTypeName + "')) and (convert(date, BeginDate, 104) = convert(date, CURRENT_TIMESTAMP, 104))");

            //Builds a JSon from the treatments [HansesM]
            var resultEvent = events.Select(a => new JsonRoomSchedulerEvents
            {
                roomName = a.Room.RoomNumber.ToString(),
                treatmentType = a.Description.ToString(),
                beginDate = a.BeginDate.ToString("s"),
                endDate = a.EndDate.ToString("s")
            }).ToList();

            //returns the Json to the calling-function [HansesM]
            return Json(resultEvent);
        }
    }
}