using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Security;

namespace BiberDAMM.Controllers
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    [CustomAuthorize]
    public class HealthInsuranceController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //Generating Index Page
        public ActionResult Index(string searchString)
        {
            var healthInsurances = from m in db.HealthInsurances
                select m;

            if (!string.IsNullOrEmpty(searchString))
                healthInsurances =
                    healthInsurances.Where(s => s.Name.Contains(searchString) ||
                                                s.Type.ToString().Contains(searchString) ||
                                                s.Number.Contains(searchString));

            return View(healthInsurances.OrderBy(o => o.Name));
        }

        //Generating Details Page
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
                return HttpNotFound();
            return View(healthInsurance);
        }


        //Getter and Setter for Creation-Page

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthInsurance healthInsurance)
        {
            if (ModelState.IsValid)
            {
                db.HealthInsurances.Add(healthInsurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(healthInsurance);
        }


        //Getter and Setter for Edit-Page
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
                return HttpNotFound();
            return View(healthInsurance);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HealthInsurance healthInsurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(healthInsurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(healthInsurance);
        }


        //Function for deleting Datasets
        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var healthInsurance = db.HealthInsurances.Find(id);
            db.HealthInsurances.Remove(healthInsurance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Function for Redirecting HealthInsurance to Client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GoToClient(int id )
        {
            var healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
                return HttpNotFound();
            ViewBag.TempHealthInsurance = healthInsurance;
            return RedirectToAction("Edit", "Client", new { id = (Client)ViewBag.TempClient.Id });
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}