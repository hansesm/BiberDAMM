/*
 * Controller for Bed
 * Initially created by: JEL, ANNAS
 * Edited: Jean-PierreK
 */

using System.Linq;
using System.Web.Mvc;
using BiberDAMM.Models;
using BiberDAMM.DAL;
using System.Data.Entity;

namespace BiberDAMM.Controllers
{
    public class BedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //GET /Bed/
        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }
        //GET /Bed/Create to add a bed 
        public ActionResult New()
        {
            return View();
        }
        // Validation token currently causing error, therefor commented out for now
        //[HttpPost]                        
        //[ValidateAntiForgeryToken]        
        public ActionResult Create([Bind(Include = "Id,Model,RoomId")] Bed bed)
        {
            if (ModelState.IsValid)
            {
                db.Beds.Add(bed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bed);
        }
        //Get /Bed/Edit for id
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Bed bed = db.Beds.Find(id);
            if (bed == null)
            {
                return HttpNotFound();
            }
            return View(bed);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Model,RoomId")] Bed bed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bed);
        }
        //GET Bed/Details for Bed with id
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Bed bed = db.Beds.Find(id);
            if (bed == null)
            {
                return HttpNotFound();
            }
            return View(bed);
        }
        //SAVE: Bed [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}