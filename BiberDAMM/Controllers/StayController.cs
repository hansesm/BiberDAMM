using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;

namespace BiberDAMM.Controllers
{
    public class StayController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // Method to get all stays of a given day [HansesM]
        public ActionResult Index(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                var stays = _db.Stays.SqlQuery(
                    "select * from stays where CAST(CURRENT_TIMESTAMP AS DATE) between BeginDate and EndDate or CAST(CURRENT_TIMESTAMP AS DATE) = CAST(BeginDate AS DATE);").ToList();
                return View(stays);
            }
            else
            {


                //var requestedDate = DateTime.ParseExact(date, "yyyy-MM-dddd", CultureInfo.InvariantCulture);

                DateTime requestedDate;
                if (!DateTime.TryParse(date, out requestedDate))
                {
                    return new EmptyResult();
                }
                
                //TODO Find Errors ! [HansesM]
                var stays = _db.Stays.SqlQuery("select * from stays where CAST('" + requestedDate.ToString("dd/MM/yyyy") + "' AS DATE) between BeginDate and EndDate or CAST('" + requestedDate.ToString("dd/MM/yyyy") + "' AS DATE) = CAST(BeginDate AS DATE);").ToList();

                return View(stays);


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