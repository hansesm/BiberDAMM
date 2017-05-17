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
        public ActionResult Create([Bind(Include = "Id,Name,Type")] Room Room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(Room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Room);
        }
        public ActionResult New()
        {
            return View();
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