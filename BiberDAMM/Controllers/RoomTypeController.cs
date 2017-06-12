using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers {

    public class RoomTypeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        // GET all: RoomType [JEL]
        public ActionResult Index()
        {
            return View(db.RoomTypes.ToList());
        }

        //CREATE: RoomType [JEL] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomType RoomType)
        {
            //checks if roomtype is already in use; an alert will be displayed
            if (db.RoomTypes.Any(r => r.Name.Equals(RoomType.Name)))
            {
                TempData["CreateRoomTypeFailed"] = " " + RoomType.Name.ToString() + " existiert bereits.";
                return RedirectToAction("Create", RoomType);
            }
            if (ModelState.IsValid)
            {
                db.RoomTypes.Add(RoomType);
                db.SaveChanges();
                TempData["CreateRoomTypeSuccess"] = " " + RoomType.Name.ToString() + " wurde hinzugefügt.";
                return RedirectToAction("Index");
            }
            return View(RoomType);
        }
        //CREATE: RoomType [JEL] 
        public ActionResult Create()
        {            
            return View();
        }

      
        //GET SINGLE: RoomType [JEL]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var roomType = db.RoomTypes.Find(id);
            if (roomType == null)
                return HttpNotFound();
            return View(roomType);
        }

        //CHANGE: RoomType [JEL] 
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var room = db.RoomTypes.Find(id);
            if (room == null)
                return HttpNotFound();
            return View(room);
        }
        //Post CHANGE: RoomType [JEL] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomType roomType, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
            {
                return RedirectToAction("Details", "RoomType", new { id = roomType.Id });
            }
            //checks if roomType exists already and if the current id is not equal to an existing id
            if (db.RoomTypes.Any(r => r.Name.Equals(roomType.Name) && !(r.Id.Equals(roomType.Id))))
            {
                TempData["EditRoomTypeFailed"] = " " + roomType.Name.ToString() + " existiert bereits.";
                return RedirectToAction("Edit", roomType);
            }


            if (ModelState.IsValid)
            {
                db.Entry(roomType).State = EntityState.Modified;
                db.SaveChanges();
                //if update succeeded
                TempData["EditRoomTypeSuccess"] = " " + roomType.Name.ToString() + " wurde aktualisiert.";
                return RedirectToAction("Details", "roomType", new { id = roomType.Id });
            }

            return View(roomType);
        }
        //DELETE: RoomType [JEL]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var roomType = db.RoomTypes.Find(id);
            if (roomType == null)
                return HttpNotFound();

            //// check if there are dependencies
            
            Room dependentRoom = db.Rooms.Where(r => r.RoomTypeId == id).FirstOrDefault();
            TreatmentType dependentTreatmentType = db.RoomTypes.Where(tt => tt.Id == id).SelectMany(u => u.TreatmentType).FirstOrDefault();            

            //// if there is a treatmenttype or room that is linked to the roomtype, the roomtype can't be deleted
            if (dependentRoom != null || dependentTreatmentType != null)
            {
                // failure-message for alert-statement
                TempData["DeleteRoomTypeFailed"] = " Es bestehen Abhängigkeiten zu einem vorhandenen Behandlungstypen oder Raum.";
                return RedirectToAction("Details", "RoomType", new { id });
            }
            db.RoomTypes.Remove(roomType);
            db.SaveChanges();
            TempData["DeleteRoomTypeSuccess"] = " " + roomType.Name.ToString() + " wurde entfernt.";
            return RedirectToAction("Index");
        }

    }
}