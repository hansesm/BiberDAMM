using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class ContactDataController : Controller
    {
        // GET all: ContactData [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: ContactData [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: ContactData [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: ContactData [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: ContactData [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}