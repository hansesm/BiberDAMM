using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class TreatmentTypeController : Controller
    {
        // GET all: TreatmentType [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: TreatmentType [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: TreatmentType [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: TreatmentType [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: TreatmentType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}