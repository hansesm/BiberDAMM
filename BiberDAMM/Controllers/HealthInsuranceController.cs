using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class HealthInsuranceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HealthInsurances
        public ActionResult Index()
        {
            return View(db.HealthInsurances.ToList());
        }

        // GET: HealthInsurances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

        // GET: HealthInsurances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HealthInsurances/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Type")] HealthInsurance healthInsurance)
        {
            if (ModelState.IsValid)
            {
                db.HealthInsurances.Add(healthInsurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(healthInsurance);
        }

        // GET: HealthInsurances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

        // POST: HealthInsurances/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Type")] HealthInsurance healthInsurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(healthInsurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(healthInsurance);
        }

        // GET: HealthInsurances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

        // POST: HealthInsurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            db.HealthInsurances.Remove(healthInsurance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}