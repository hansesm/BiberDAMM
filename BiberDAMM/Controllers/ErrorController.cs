using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiberDAMM.Controllers
{
    //Controller witch handels error request. [HansesM]
    public class ErrorController : Controller
    {
        //When there is no path specified the OOps page will be shown [HansesM]
        public ViewResult Index()
        {
            return View("Oops");
        }

        //This should be the normal case. [HansesM]
        public ActionResult Oops(int id)
        {
            Response.StatusCode = id;
            return View();
        }
    }
}