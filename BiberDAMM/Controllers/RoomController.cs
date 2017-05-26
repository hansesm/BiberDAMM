using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;
namespace BiberDAMM.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET all: Room [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        //CREATE: Room [JEL] [ANNAS]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room Room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(Room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Room);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete(int? id, string command)
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
        public ActionResult DeleteConfirmed(int id, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
            {
                return RedirectToAction("Details", "Room", id);
            }
            
            var room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //CHANGE: Room [JEL] [ANNAS]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room, string command){
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Room", new { id = room.Id });

            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                //if update succeeded #ToDo fehlermeldung einfügen falls nciht möglich
                TempData["EditRoomSuccess"] = " Die Raumdetails wurden aktualisiert.";
                return RedirectToAction("Details", "room", new { id = room.Id });
            }
           
            return View(room);
        }

        //GET SINGLE: Room [JEL] [ANNAS]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var room = db.Rooms.Find(id);
            if (room == null)
                return HttpNotFound();
            return View(room);
        }
        //SAVE: Room [JEL] [ANNAS]
            public ActionResult Save()
        {
            return View();
        }
    }
}