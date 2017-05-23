using System.Web.Mvc;

namespace BiberDAMM.Controllers
{
    public class AccessDeniedController : Controller
    {
        // This action is called when an authenticated user tries to call an action he is not athorized for [KrabsJ]
        // GET: AccessDenied
        public ActionResult Index()
        {
            return View();
        }
    }
}