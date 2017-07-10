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

        //-- GET Index page /Bed/ and return a list of all beds --//
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }

        //-- GET page /Bed/Create to add a new bed --//
        [HttpGet]
        public ActionResult Create()
        {
            var AllRooms = db.Rooms.ToList(); // list of all rooms
            var AllBeds = new List<Bed>().AsQueryable();
            AllBeds = from m in db.Beds select m; // list of all beds
            var RoomListing = new List<Room>(); // new list for all rooms with less than max. beds 
            foreach (Room AvailableRoom in AllRooms) // iterate though all rooms
            {
                var ListTempBeds = AllBeds.Where(a => a.RoomId.Equals(AvailableRoom.Id)); // temp list for beds in each room
                if (ListTempBeds.Count() < AvailableRoom.RoomMaxSize) // check if bed capacity reached in current room
                {
                    RoomListing.Add(AvailableRoom); // if not add room to list RoomListing
                }
            }
            // Returned list of all selectable rooms for bed placement
            ViewBag.RoomList = new SelectList(RoomListing, "Id", "RoomNumber");

            return View();
        }

        //-- SET/ Post method to add the newly created bed to the system --//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bed bed, string command)
        {
            // if AbortButton is pressed drop selection and return to index page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                db.Beds.Add(bed);
                db.SaveChanges();
                // Return notification if adding the new bed was successful
                TempData["CreateBedSuccess"] = " Das Bett wurde hinzugefügt";
                return RedirectToAction("Index");
            }
            else
            {
                // Return notification if adding the new bed was unsuccessful
                TempData["CreateBedFailed"] = " Das Bett konnte nicht hinzugefügt werden";
                return RedirectToAction("Index");
            }
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

            var AllRooms = db.Rooms.ToList();
            var AllBeds = new List<Bed>().AsQueryable();
            AllBeds = from m in db.Beds select m;
            var RoomListing = new List<Room>();

            /* Here it is necessary to add the current room of selected bed 
             * to RoomList, even if Room reached its max capacity. */
            foreach (Room AvailableRoom in AllRooms)
            {
                var ListTempBeds = AllBeds.Where(a => a.RoomId.Equals(AvailableRoom.Id));
                // Return room of selected bed from db
                var RoomOfCurrentBed = AllBeds.Where(q => q.Id == id).Select(q => q.RoomId).FirstOrDefault();
                if (ListTempBeds.Count() < AvailableRoom.RoomMaxSize)
                {
                    RoomListing.Add(AvailableRoom);
                }
                if (ListTempBeds.Count() == AvailableRoom.RoomMaxSize) // if room is full
                {
                    // if RoomId matches the room of currently selected bed add that room to RoomList
                    if (RoomOfCurrentBed.Equals(AvailableRoom.Id))  
                    {
                        RoomListing.Add(AvailableRoom); 
                    }
                }
            }

            ViewBag.RoomList = new SelectList(RoomListing, "Id", "RoomNumber");
            return View(bed);
        }

        //-- SET/ Post method to change details of selected bed entry --// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Bed bed, string command)
        {
            // if AbortButton is pressed forgo changes and return to details page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Bed", new { id = bed.Id });

            // check if ModelState is valid after edit
            if (ModelState.IsValid)
            {
                db.Entry(bed).State = EntityState.Modified;
                db.SaveChanges();
                // Return notification if editing was successful
                TempData["EditBedSuccess"] = " Die Eigenschaften wurden erfolgreich geändert";
                return RedirectToAction("Details", "Bed", new { id = bed.Id });
            }
            return View(bed);
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

        //-- Get for bed deletion --//
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

        //-- Post function to delete Bed --//
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Check if bed is occupied by a patient
            Blocks dependentBlock = db.Blocks.Where(b => b.BedId == id).FirstOrDefault();

            if (dependentBlock != null) // if bed is blocked
            {
                // Return alert-message if bed deletion not possible
                TempData["DeleteBedFailed"] = " Das Bett ist belegt";
                return RedirectToAction("Details", "Bed", new { id });
            }

            var bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            // Return notification if delete was successful
            TempData["DeleteBedSuccess"] = " Das Bett wurde entfernt";
            return RedirectToAction("Index");
        }
    }
}