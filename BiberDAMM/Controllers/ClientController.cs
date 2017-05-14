using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;


namespace BiberDAMM.Controllers
{

    public class ClientController : Controller
    {

        // GET all: Client [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: Client [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: Client [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: Client [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: Client [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }

    }
}