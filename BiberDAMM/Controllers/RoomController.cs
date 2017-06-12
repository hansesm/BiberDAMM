using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;
using System.Collections.Generic;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET all: Room [JEL]
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        //CREATE: Room [JEL]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room Room)
        {
            //checks if roomNumber is already in use; an alert will be displayed
            if (db.Rooms.Any(r => r.RoomNumber.Equals(Room.RoomNumber))){
                TempData["CreateRoomFailed"] = " " + Room.RoomNumber.ToString() + " existiert bereits.";
                return RedirectToAction("Create", Room);
            }          
            
            if (ModelState.IsValid)
            {
                db.Rooms.Add(Room);
                db.SaveChanges();
                TempData["CreateRoomSuccess"] = " " + Room.RoomNumber.ToString() + " wurde hinzugefügt.";
                return RedirectToAction("Index");
            }
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }

           
            var viewModel = new RoomViewModel(Room, selectedListRoomTypes);
            return View(viewModel);
            }

        public ActionResult Create()
        {
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;            
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }
                     
            var room = new Room();
            var viewModel = new RoomViewModel(room,selectedListRoomTypes);
            return View(viewModel);
        }
        //DELETE: Room [JEL]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
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
            Bed dependentBed = db.Beds.Where(b => b.RoomId == id).FirstOrDefault();
            Treatment dependentTreatment = db.Rooms.Where(t => t.Id == id).SelectMany(u => u.Treatment).FirstOrDefault();

            // if there is a treatment or bed that is linked to the room, the room can't be deleted
            if (dependentBed != null || dependentTreatment != null)
            {
                // failure-message for alert-statement
                TempData["DeleteRoomFailed"] = " Es bestehen Abhängigkeiten zu anderen Krankenhausdaten.";
                return RedirectToAction("Details", "Room", new {  id });
            }
            
            var room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            TempData["DeleteRoomSuccess"] =" "+ room.RoomNumber.ToString()+ " wurde entfernt.";
            return RedirectToAction("Index");
        }
    
    //CHANGE: Room [JEL] 
    public ActionResult Edit(int? id)
        {  
            if (id == null)
                return RedirectToAction("Index");
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();

            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            //selectedListRoomTypes.Add(new SelectListItem{ Text = " ", Value = null });
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }            
            var viewModel = new RoomViewModel(room, selectedListRoomTypes);
            return View(viewModel);          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room, string command){
            if (command.Equals(ConstVariables.AbortButton))
            {
                return RedirectToAction("Details", "Room", new { id = room.Id });
            }
            //checks if roomNumber exists already and if the current id is not equal to an existing id
            if (db.Rooms.Any(r => r.RoomNumber.Equals(room.RoomNumber) && !(r.Id.Equals(room.Id))))
            {
                TempData["EditRoomFailed"] = " " + room.RoomNumber.ToString() + " existiert bereits.";
                return RedirectToAction("Edit", room);
            }        
      

            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                //if update succeeded
                TempData["EditRoomSuccess"] = " Die Raumdetails wurden aktualisiert.";
                return RedirectToAction("Details", "room", new { id = room.Id });
            }
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }


            var viewModel = new RoomViewModel(room, selectedListRoomTypes);
            return View(viewModel);
        }

        //GET SINGLE: Room [JEL]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();
            return View(room);
        }       
    }
}