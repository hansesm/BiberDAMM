/*
 * Controller for Bed
 * Initially created by: JEL, ANNAS
 * Edited: Jean-PierreK
 */

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class BedController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //GET /Bed/
        /*public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }
        */
        public ActionResult Index(string searchString)
        {
            var beds = from m in db.Beds
                select m;

            if (!string.IsNullOrEmpty(searchString))
                beds = beds.Where(s => s.Model.Contains(searchString));

            return View(beds.OrderBy(o => o.Id));
        }

        //GET /Bed/Create to add a bed 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("Index");
            var bed = db.Beds.Find(id);
            if (bed == null)
                return HttpNotFound();
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
                return RedirectToAction("Index");
            var bed = db.Beds.Find(id);
            if (bed == null)
                return HttpNotFound();
            return View(bed);
        }
    }
}