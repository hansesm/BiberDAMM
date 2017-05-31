using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers {

    public class RoomTypeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET all: RoomType [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View(db.RoomTypes.ToList());
        }

        //CREATE: RoomType [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }

        //CHANGE: RoomType [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }

        //GET SINGLE: RoomType [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }

        //SAVE: RoomType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}