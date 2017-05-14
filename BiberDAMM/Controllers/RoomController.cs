using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;
namespace BiberDAMM.Controllers
{
    public class RoomController : Controller
    {
        // GET all: Room [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: Room [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: Room [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: Room [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: Room [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}