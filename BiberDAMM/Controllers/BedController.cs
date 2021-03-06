﻿/*
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
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize]
    public class BedController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //-- GET Index page /Bed/ and return a list of all beds --//
        [HttpGet]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }

        //-- GET page /Bed/Create to add a new bed --//
        [HttpGet]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator)]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Check if bed is or was tied to a patient or block
            Blocks dependentBlock = db.Blocks.Where(b => b.BedId == id).FirstOrDefault();
            /* The following would allow the deletion of beds not currently in use or booked
             * Blocks dependentBlock = db.Blocks.Where(b => b.BedId == id 
                                    && b.BeginDate <= DateTime.Now 
                                    && b.EndDate >= DateTime.Now).FirstOrDefault();
             */
            if (dependentBlock != null) // if bed is blocked
            {
                // Return alert-message if bed deletion not possible
                TempData["DeleteBedFailed"] = " Es bestehen Abhängigkeiten zu Patientenaufenthalten";
                return RedirectToAction("Details", "Bed", new { id });
            }

            var bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            // Return notification if delete was successful
            TempData["DeleteBedSuccess"] = " Das Bett wurde entfernt";
            return RedirectToAction("Index");
        }
        //-- Get the schedule for currently occupied beds --//
        [HttpGet]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse)]
        public ActionResult BedSchedule()
        {
            // simple statistic gimmick
            double CountAllBeds = (from b in db.Beds where b.Id > 0 select b).Count();
            double CountOccupiedBeds = (from b in db.Beds join bl in db.Blocks on b.Id equals bl.BedId
                                        where bl.BeginDate <= DateTime.Now && bl.EndDate >= DateTime.Now select b).Count();
            double OccupationRate = Math.Round((100 * CountOccupiedBeds / CountAllBeds), 2);

            ViewBag.TotalBeds = CountAllBeds;
            ViewBag.BedsInUse = CountOccupiedBeds;
            ViewBag.Percentage = OccupationRate;

            // fetch all beds currently in use, show start and enddate, get name of patient in it
            var DataQuery = from c in db.Clients
                            join s in db.Stays on c.Id equals s.ClientId
                            join bl in db.Blocks on s.Id equals bl.StayId
                            join b in db.Beds on bl.BedId equals b.Id
                            where bl.BeginDate <= DateTime.Now && bl.EndDate >= DateTime.Now
                            join r in db.Rooms on b.RoomId equals r.Id
                            select new BedScheduleViewModel
                            {
                                BedNbr = b.Id,
                                BedModel = b.BedModels,
                                RoomNbr = r.RoomNumber,
                                PatientSName = c.Surname,
                                PatientLName = c.Lastname,
                                StartDate = bl.BeginDate,
                                EndDate = bl.EndDate
                            };
            
            return View(DataQuery);
        }
    } 
}