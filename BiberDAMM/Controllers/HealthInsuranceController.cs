using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class HealthInsuranceController : Controller
    {
        // GET all: HealthInsurance [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: HealthInsurance [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: HealthInsurance [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: HealthInsurance [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: HealthInsurance [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}