using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class RoomController : Controller     {

        private ApplicationDbContext db = new ApplicationDbContext();

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
            else
            {
                return View(Room);
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //CHANGE: Room [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: Room [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: Room [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}