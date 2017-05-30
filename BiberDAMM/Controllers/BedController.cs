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
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers
{
    public class BedController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //GET /Bed/
        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }
  
        //GET /Bed/Create to add a bed 
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bed bed, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                db.Beds.Add(bed);
                db.SaveChanges();
                TempData["CreateBedSuccess"] = " Das Bett wurde hinzugefügt";
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
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomNumber");
            return View(bed);
        }


       [HttpPost]
       [ValidateAntiForgeryToken]
          public ActionResult Edit(Bed bed, string command)
          {
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Bed", new { id = bed.Id });

            if (ModelState.IsValid)
              {
                  db.Entry(bed).State = EntityState.Modified;
                  db.SaveChanges();
                //TODO: Add error message if editing fails
                TempData["EditBedSuccess"] = " Die Eigenschaften wurden erfolgreich geändert";
                return RedirectToAction("Details", "Bed", new { id = bed.Id });
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
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomNumber");
            return View(bed);
        }

        //Function for deleting Datasets
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var bed = db.Beds.Find(id);
            if (bed == null)
                return HttpNotFound();
            return View(bed);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            TempData["DeleteBedSuccess"] = " Das Bett wurde entfernt";
            return RedirectToAction("Index");
        }
        public ActionResult Save()
        {
            return View();
        }
    }
}