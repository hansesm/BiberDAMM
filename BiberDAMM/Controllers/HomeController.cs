using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.Security;
using BiberDAMM.Helpers;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        // load the different homepages based on the usertype of the logged in user [KrabsJ]
        public ActionResult Index()
        {
            switch (User.Identity.GetUserType())
            {
                case "Administrator":
                    return View("IndexAdmin");
                case "Arzt":
                    return View("IndexDoctor");
                case "Pflegekraft":
                    return View("IndexNursingStaff");
                case "Reinigungskraft":
                    return View("IndexCleaner");
                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}