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
            /* 
             * Create a list containing all rooms.
             * List all created beds.
             * Iterate through rooms and cross check with capacity if beds can be placed in it. 
             * Rooms with MaxSize of 0 are not selectable for bed placement same goes
             * for rooms which reached their maximum bed capacity
             */
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

            // only if ModelState is valid will the new bed be added
            if (ModelState.IsValid)
            {
                db.Beds.Add(bed);
                db.SaveChanges();
                // Return notification if adding a new bed was successful
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
                return RedirectToAction("Index"); // return to index if current id is null
            var bed = db.Beds.Find(id); // return 404 if id not present in db
            if (bed == null)
                return HttpNotFound();

            // for detailed comment refer to create bed method
            var AllRooms = db.Rooms.ToList();
            var AllBeds = new List<Bed>().AsQueryable();
            AllBeds = from m in db.Beds select m;
            var RoomListing = new List<Room>();
            
            /* Here it is imperative to add the current room of selected bed 
             * to the selectlist RoomList even if Room reached its max capacity 
             * to avoid an exception being thrown when changing BedModel in a full room.
             * Second if statement in the loop checks if Room is full and then checks if RoomId
             * matches the Room of currently selected bed.
             */
            foreach (Room AvailableRoom in AllRooms)
            {
                var ListTempBeds = AllBeds.Where(a => a.RoomId.Equals(AvailableRoom.Id));
                var RoomOfCurrentBed = AllBeds.Where(q => q.Id == id).Select(q => q.RoomId).FirstOrDefault();
                if (ListTempBeds.Count() < AvailableRoom.RoomMaxSize)
                {
                    RoomListing.Add(AvailableRoom);
                }
                if (ListTempBeds.Count() == AvailableRoom.RoomMaxSize)
                {
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

            // if AbortButton is pressed forgo changes and return to index page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Bed", new { id = bed.Id });

            // check if ModelState is valid after edit
            if (ModelState.IsValid)
            {
                db.Entry(bed).State = EntityState.Modified;
                db.SaveChanges();
                // Return notification if editing bed was successful
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
            // Check if bed is blocked
            Blocks dependentBlock = db.Blocks.Where(b => b.BedId == id).FirstOrDefault();

            if (dependentBlock != null) // if bed is blocked by a patient
            {
                //-- Return alert-message if deletion of bed not possible --//
                TempData["DeleteBedFailed"] = " Das Bett ist belegt";
                return RedirectToAction("Details", "Bed", new { id });
            }

            // if bed is not blocked find id in db and remove bed from db
            var bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            // Return notification if deleting bed was successful
            TempData["DeleteBedSuccess"] = " Das Bett wurde entfernt";
            return RedirectToAction("Index");
        }
    }
}