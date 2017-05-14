using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    public class BlocksController : Controller
    {
        // GET all: Blocks [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }
        //CREATE: Blocks [JEL] [ANNAS]
        public ActionResult New()
        {
            return View();
        }
        //CHANGE: Blocks [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }
        //GET SINGLE: Blocks [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }
        //SAVE: Blocks [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}