using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class StayController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //Inline-Class for for displaying treatment-data in a calendar view, needed in the Details-Method [HansesM]
        public class JsonEvent
        {
            public string id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

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
                    //If parsing wasn't succsessfull return a empty page [HansesM]
                    //ToDo add nice errorpage for parsing error [HansesM]
                    return new EmptyResult();
                }
                //Request all stays matching the given date from the DB using a sql-querry [HansesM]
                var stays = _db.Stays.SqlQuery("select * from stays where CAST('" + requestedDate.ToString("MM-dd-yyyy") + "' AS DATE) between BeginDate and EndDate or CAST('" + requestedDate.ToString("MM-dd-yyyy") + "' AS DATE) = CAST(BeginDate AS DATE);").ToList();

                //Returns the result [HansesM]
                return View(new StayIndexViewModel(stays, requestedDate));
            }
        }

        //CREATE: Stay [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }

        //CHANGE: Stay [HansesM]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stay stay, string command)
        {
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
                stayInDb.LastUpdated = DateTime.Now;
                stayInDb.RowVersion++;

                //Saves all changes [HansesM]
                _db.SaveChanges();

                //Returns to the details-page and provides an succsessfull-alert [HansesM]
                TempData["EditStaySuccess"] = " Die Eigenschaften wurden erfolgreich geändert.";
                return RedirectToAction("Details", "Stay", new { id = stay.Id });
            }
            //TODO Model-State invalid
            return RedirectToAction("Index", "Stay", new { id = stay.Id });
        }

        //GET SINGLE: Stay [HansesM]
        public ActionResult Details(int id)
        {
            //Gets the stay from the database [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == id);

            //Gets all doctors from the database [HansesM]
            var listDoctors = _db.Users.Where(s => s.UserType == UserType.Arzt);
            //listDoctors = listDoctors.Where(s => s.UserType == UserType.Arzt);

            //Fits all Doctors into a selectetList to display in a dropdown-list[HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //_db.ApplicationUser.SqlQuery("select Id, Title, Surname, Lastname from AspNetUsers where UserType = 3;");

            //var listTreatments = _db.Treatments.AsQueryable();
            //listTreatments = listTreatments.Where(t => t.Stay.ClientId == id);

            //Gets a treatments from the given stay [HansesM]
            var events = stay.Treatments.ToList();

            //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
            var result = events.Select(e => new JsonEvent()
            {
                start = e.Begin.ToString("s"),
                end = e.End.ToString("s"),
                title = e.TreatmentType.Name.ToString(),
                id = e.Id.ToString()

            }).ToList();

            //Creates a JsonResult from the Json [HansesM]
            JsonResult resultJson = new JsonResult { Data = result };
            
            //Creats a new View-Model with stay, the selectable list of doctors and the json with treatment calendar data [HansesM]
            var viewModel = new DetailsStayViewModel(stay, selectetListDoctors, resultJson);

            //returns the viewmodel [HansesM]
            return View(viewModel);
        }

        //SAVE: Stay [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }

        //Override on Dispose for security reasons. [HansesM]
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}