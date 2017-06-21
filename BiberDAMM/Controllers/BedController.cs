/*
 * Controller for Bed
 * Author: Jean-PierreK 
 */

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;
using BiberDAMM.Security;
using System;
using System.Collections.Generic;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize]
    public class BedController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //-- GET Index page /Bed/ --//
        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }
  
        //-- GET page /Bed/Create to add a bed --//
        [HttpGet]
        public ActionResult Create()
        {
            /* 
             * Create a list containing all rooms
             * List all created beds
             * Iterate through rooms and cross check with capacity if beds can be placed in it. 
             * Rooms with MaxSize of 0 are not selectable for bed placement same goes
             * for Rooms which reached their maximum bed capacity
             */
            var AllRooms = db.Rooms.ToList();
            var AllBeds = new List<Bed>().AsQueryable();
            AllBeds = from m in db.Beds select m;
            var RoomListing = new List<Room>();
            foreach (Room AvailableRoom in AllRooms)
            {
                var ListTempBeds = AllBeds.Where(a => a.RoomId.Equals(AvailableRoom.Id));
                if (ListTempBeds.Count() < AvailableRoom.RoomMaxSize)
                {
                    RoomListing.Add(AvailableRoom);
                }
            }
            // Returned list of all selectable rooms for bed placement
            ViewBag.RoomList = new SelectList(RoomListing, "Id", "RoomNumber");
            return View();
        }

        //-- SET method to create a new entry for bed --//
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
                //-- Return notification if adding a new bed was successful --//
                TempData["CreateBedSuccess"] = " Das Bett wurde hinzugefügt";
                return RedirectToAction("Index");
            }
            
            return View(bed);
        }

        //-- GET /Bed/Edit to edit selected bed entry --//
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var bed = db.Beds.Find(id);
            if (bed == null)
                return HttpNotFound();

            // Same as for create method
            var AllRooms = db.Rooms.ToList();
            var AllBeds = new List<Bed>().AsQueryable();
            AllBeds = from m in db.Beds select m;
            var RoomListing = new List<Room>();
            foreach (Room AvailableRoom in AllRooms)
            {
                var ListTempBeds = AllBeds.Where(a => a.RoomId.Equals(AvailableRoom.Id));
                if (ListTempBeds.Count() < AvailableRoom.RoomMaxSize)
                {
                    RoomListing.Add(AvailableRoom);
                }
            }
            // Returned list of all selectable rooms for bed placement
            ViewBag.RoomList = new SelectList(RoomListing, "Id", "RoomNumber");
            return View(bed);
        }

       //-- SET method to change details of selected entry --// 
       [HttpPost]
       [ValidateAntiForgeryToken]
          public ActionResult Edit(int id, Bed bed, string command)
          {
            System.Diagnostics.Debug.WriteLine(bed.BedModels + " " + bed.RoomId);
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Bed", new { id = bed.Id });

            if (ModelState.IsValid)
            { 
                  db.Entry(bed).State = EntityState.Modified;
                  db.SaveChanges();
                //-- Return notification if editing bed was successful --//
                TempData["EditBedSuccess"] = " Die Eigenschaften wurden erfolgreich geändert";
                return RedirectToAction("Details", "Bed", new { id = bed.Id });
            }
            return View("Bed");
          }
       
        //-- GET Bed/Details page for Bed with id --//
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var bed = db.Beds.Find(id);
            if (bed == null)
                return HttpNotFound();
            return View(bed);
        }

        //-- Function to delete Bed datasets --//
        [HttpGet]
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
            //-- Check if bed is blocked --//
            Blocks dependentBlock = db.Blocks.Where(b => b.BedId == id).FirstOrDefault();

            if (dependentBlock != null )
            {
                //-- Return alert-message if deletion of bed not possible --//
                TempData["DeleteBedFailed"] = " Das Bett ist belegt";
                return RedirectToAction("Details", "Bed", new { id });
            }

            var bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            //-- Return notification if deleting bed was successful --//
            TempData["DeleteBedSuccess"] = " Das Bett wurde entfernt";
            return RedirectToAction("Index");
        }
    }
}