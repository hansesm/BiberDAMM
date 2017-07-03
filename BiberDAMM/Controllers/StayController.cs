﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using BiberDAMM.Security;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse + "," + ConstVariables.RoleTherapist )]
    public class StayController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // Method to get all stays of a given day [HansesM]
        public ActionResult Index(string date)
        {
            //Initalizes the requested Date to now.
            var requestedDate = DateTime.Now;

            //If there is no date given use toay. [HansesM]
            if (string.IsNullOrEmpty(date))
            {
                var stays = _db.Stays.SqlQuery(
                    "select * from stays where CAST(CURRENT_TIMESTAMP AS DATE) between BeginDate and EndDate or CAST(CURRENT_TIMESTAMP AS DATE) = CAST(BeginDate AS DATE);").ToList();
                return View(new StayIndexViewModel(stays, requestedDate));
            }
            else
            {
                //Parse the given string into a datetime-object. [HansesM]

                if (!DateTime.TryParse(date, out requestedDate))
                {
                    //If parsing wasn't succsessfull return to home :D [HansesM]
                    return RedirectToAction("Index", "Home");
                }
                //Request all stays matching the given date from the DB using a sql-querry [HansesM]
                var stays = _db.Stays.SqlQuery("select * from stays where CAST('" + requestedDate.ToString("MM-dd-yyyy") + "' AS DATE) between BeginDate and EndDate or CAST('" + requestedDate.ToString("MM-dd-yyyy") + "' AS DATE) = CAST(BeginDate AS DATE);").ToList();

                //Returns the result [HansesM]
                return View(new StayIndexViewModel(stays, requestedDate));
            }
        }

        //CREATE: Stay [HansesM]
        public ActionResult Create(int id)
        {
            //gets the client from the given id
            var client = _db.Clients.SingleOrDefault(m => m.Id == id);

            if (client == null)
            {
                return RedirectToAction("Index", "Stay");
            }

            //Creates a new, empty stay [HansesM]
            var newStay = new Stay();
            //Sets some Values [HansesM]
            newStay.Client = client;
            newStay.ClientId = client.Id;
            newStay.BeginDate = DateTime.Now;

            //Gets a list of Doctors for the Dropdownlist [HansesM]
            var listDoctors = _db.Users.Where(s => s.UserType == UserType.Arzt);
            //Builds a selectesList out of the list of doctors [HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //Creates the view model [HansesM]
            var viewModel = new StayCreateViewModel(id, newStay, selectetListDoctors);

            return View(viewModel);
        }

        //Post Method for creating new Stay [HansesM]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stay stay, string command)
        {
            //If abort button is pressed we get a new details-view and dismiss all changes [HansesM]
            if (command.Equals(ConstVariables.AbortButton))
            {
                //Returns the user and displays a alert [HansesM]
                TempData["CreateStayAbort"] = "Anlegen eines neuen Aufenthalts erfolgreich abgebrochen.";
                return RedirectToAction("Details", "Client", new { id = stay.ClientId });
            }

            //Checks if begin is greater than enddate [HansesM]
            if (stay.BeginDate > stay.EndDate)
            {
                //Added errormessage name to dispay in loginview [HansesM]
                ModelState.AddModelError("EndDateError", "Das Enddatum muss nach dem Beginndatum liegen");
            }

            //Checks if Modelstate is valid [HanseM]
            if (ModelState.IsValid)
            {
                stay.LastUpdated = DateTime.Now;
                _db.Stays.Add(stay);
                _db.SaveChanges();
                return RedirectToAction("Details", "Stay", new { id = stay.Id });
            }

            //gets the client from the id [HansesM]
            var client = _db.Clients.SingleOrDefault(m => m.Id == stay.ClientId);
            //Sets the client to the invalid-stay [HansesM]
            stay.Client = client;

            //Gets a list of Doctors for the Dropdownlist [HansesM]
            var listDoctors = _db.Users.Where(s => s.UserType == UserType.Arzt);

            //Builds a selectesList out of the list of doctors [HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //Creates the view model with the Id, the invalid-stay and the list of doctors [HansesM]
            var viewModel = new StayCreateViewModel(stay.ClientId, stay, selectetListDoctors);
            return View(viewModel);
        }
        
        //GET SINGLE: Stay [HansesM]
        public ActionResult Details(int id)
        {
            //Gets the stay from the database [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == id);

            //Gets all doctors from the database [HansesM]
            var listDoctors = _db.Users.Where(s => s.UserType == UserType.Arzt);
            
            //Fits all Doctors into a selectetList to display in a dropdown-list[HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //Gets a treatments from the given stay [HansesM]
            var events = stay.Treatments.ToList();

            //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
            var result = events.Select(e => new JsonEventTreatment()
            {
                start = e.BeginDate.ToString("s"),
                end = e.EndDate.ToString("s"),
                title = e.TreatmentType.Name.ToString(),
                id = e.Id.ToString()

            }).ToList();

            //Creates a JsonResult from the Json [HansesM]
            JsonResult resultJson = new JsonResult { Data = result };

            //Creats a new View-BedModels with stay, the selectable list of doctors and the json with treatment calendar data [HansesM]
            var viewModel = new StayDetailsViewModel(stay, selectetListDoctors, resultJson);

            //returns the view [HansesM]
            return View(viewModel);
        }

        //CHANGE: Stay [HansesM]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Stay stay, string command)
        {
            //Checks if begin is greater than enddate [HansesM]
            if (stay.BeginDate > stay.EndDate)
            {
                //Added errormessage name to dispay in loginview [HansesM]
                ModelState.AddModelError("EndDateError", "Das Enddatum muss nach dem Beginndatum liegen");
            }

            //If abort button is pressed we get a new details-view and dismiss all changes [HansesM]
            if (command.Equals(ConstVariables.AbortButton))
            {
                //Returns the user and displays a alert [HansesM]
                TempData["EditStayAbort"] = " erfolgreich verworfen.";
                return RedirectToAction("Details", "Stay", new { id = stay.Id });

            }
            else if (ModelState.IsValid)
            {
                //Gets the stay from the Database [HansesM]
                var stayInDb = _db.Stays.Single(s => s.Id == stay.Id);

                //Checks if stay was updated in the meantime to handle concurrency ! [HansesM]
                if (stayInDb.RowVersion != stay.RowVersion)
                {
                    TempData["EditStayConcurrency"] = " Der Datensatz wurde zwischenzeitlich verändert. Bitte laden Sie den Datensatz neu und versuchen Sie es erneut.";
                    return RedirectToAction("Details", "Stay", new { id = stay.Id });
                }

                //Updates the Values [HansesM]
                stayInDb.ICD10 = stay.ICD10;
                stayInDb.ApplicationUserId = stay.ApplicationUserId;
                stayInDb.Result = stay.Result;
                stayInDb.Comment = stay.Comment;
                stayInDb.BeginDate = stay.BeginDate;
                stayInDb.EndDate = stay.EndDate;
                stayInDb.StayType = stay.StayType;
                stayInDb.LastUpdated = DateTime.Now;
                stayInDb.RowVersion++;

                //Saves all changes [HansesM]
                _db.SaveChanges();

                //Returns to the details-page and provides an succsessfull-alert [HansesM]
                TempData["EditStaySuccess"] = " Die Eigenschaften wurden erfolgreich geändert.";
                return RedirectToAction("Details", "Stay", new { id = stay.Id });
            }

            //gets the client from the id [HansesM]
            var client = _db.Clients.SingleOrDefault(m => m.Id == stay.ClientId);
            //Sets the client to the invalid-stay [HansesM]
            stay.Client = client;

            //Gets a list of Doctors for the Dropdownlist [HansesM]
            var listDoctors = _db.Users.Where(s => s.UserType == UserType.Arzt);

            //Builds a selectesList out of the list of doctors [HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //Gets a treatments from the given stay [HansesM]
            var stayFromDb = _db.Stays.Single(s => s.Id == stay.Id);
            var events = stayFromDb.Treatments.ToList();
            //Gets the related Data again from the Database
            stay.Client = stayFromDb.Client;
            stay.Treatments = stayFromDb.Treatments;
            stay.Blocks = stayFromDb.Blocks;

            //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
            var result = events.Select(e => new JsonEventTreatment()
            {
                start = e.BeginDate.ToString("s"),
                end = e.EndDate.ToString("s"),
                title = e.TreatmentType.Name.ToString(),
                id = e.Id.ToString()

            }).ToList();

            //Creates a JsonResult from the Json [HansesM]
            JsonResult resultJson = new JsonResult { Data = result };

            //Creates the view model with the Id, the invalid-stay and the list of doctors [HansesM]
            var viewModel = new StayDetailsViewModel(stay, selectetListDoctors, resultJson);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Stay stay, string command)
        {

            var stayInDb = _db.Stays.SingleOrDefault(m => m.Id == stay.Id);

            if (stayInDb.Treatments.Count != 0 || stayInDb.Blocks.Count != 0)
            {
                TempData["DeleteStayError"] = " Aufenthalt kann nicht gelöscht werden, da noch abhängige Daten vorhanden sind.";
                return RedirectToAction("Details", "Stay", new { id = stay.Id });
            }
            else
            {
                var tempClientId = stayInDb.ClientId;
                _db.Stays.Remove(stayInDb);
                _db.SaveChanges();

                TempData["DeleteStaySuccsess"] = " Aufenthalt erfolgreich gelöscht.";
                return RedirectToAction("Details", "Client", new { id = tempClientId });
            }

            
        }

        //Override on Dispose for security reasons. [HansesM]
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}