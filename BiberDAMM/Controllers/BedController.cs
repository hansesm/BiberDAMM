using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class BedController : Controller
    {
        // GET all: Bed [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: Bed [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: Bed [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: Bed [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: Bed [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}