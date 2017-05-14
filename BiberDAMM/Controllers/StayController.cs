using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class StayController : Controller
    {
        // GET all: Stay [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
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