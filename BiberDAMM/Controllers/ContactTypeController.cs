using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class ContactTypeController : Controller
    {
        // GET all: ContactType [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: ContactType [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: ContactType [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: ContactType [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: ContactType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}