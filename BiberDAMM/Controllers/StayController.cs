using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;

namespace BiberDAMM.Controllers
{
    public class StayController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // Method to get all stays 
        public ActionResult Index()
        {
            var stays = _db.Stays.SqlQuery(
                "select * from stays where CAST(CURRENT_TIMESTAMP AS DATE) between BeginDate and EndDate;").ToList();
            return View(stays);
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