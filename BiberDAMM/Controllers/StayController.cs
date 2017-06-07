using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class StayController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //A Json-Event for displaying treatment-data in a calendar view [HansesM]
        public class JsonEvent
        {
            public string id { get; set; }
            public string text { get; set; }
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

        //CHANGE: Stay [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }

        //GET SINGLE: Stay [HansesM]
        public ActionResult Detail(int id)
        {
            //Gets the stay from the database [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == id);
            
            //Gets all doctors from the database [HansesM]
            var listDoctors = _db.Users.AsQueryable();
            listDoctors = listDoctors.Where(s => s.UserType == UserType.Arzt);

            //Fits all Doctors into a selectetList [HansesM]
            var selectetListDoctors = new List<SelectListItem>();
            foreach (var m in listDoctors)
            {
                selectetListDoctors.Add(new SelectListItem { Text = (m.Title + " " + m.Lastname), Value = (m.Id.ToString()) });
            }

            //_db.ApplicationUser.SqlQuery("select Id, Title, Surname, Lastname from AspNetUsers where UserType = 3;");

            //var listTreatments = _db.Treatments.AsQueryable();
            //listTreatments = listTreatments.Where(t => t.Stay.ClientId == id);

            var events = stay.Treatments.ToList();

            var result = events.Select(e => new JsonEvent()
            {
                start = e.Begin.ToString("s"),
                end = e.End.ToString("s"),
                text = e.TreatmentType.Name.ToString(),
                id = e.Id.ToString()

            }).ToList();

            JsonResult resultJson = new JsonResult { Data = result };


            //Creats a new View-Model with the list of Doctors and the stay [HansesM]
            var viewModel = new DetailsStayViewModel(stay, selectetListDoctors, resultJson);
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