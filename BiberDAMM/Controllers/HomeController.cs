using System.Web.Mvc;
using BiberDAMM.Helpers;
using BiberDAMM.Security;

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
                case ConstVariables.RoleAdministrator:
                    return View("IndexAdmin");
                case ConstVariables.RoleDoctor:
                    return View("IndexDoctor");
                case ConstVariables.RoleNurse:
                    return View("IndexNursingStaff");
                case ConstVariables.RoleCleaner:
                    return View("IndexCleaner");
                case ConstVariables.RoleTherapist:
                    return View("IndexTherapist");
                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            // TODO [KrabsJ] decide what to show on this page
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            // TODO [KrabsJ] decide what to show on this page
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}