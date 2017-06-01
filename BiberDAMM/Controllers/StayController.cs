using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
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

        //GET SINGLE: Stay [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }

        //SAVE: Stay [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}