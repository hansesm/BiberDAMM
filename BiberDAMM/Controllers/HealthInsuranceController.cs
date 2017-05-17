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

       
        public ActionResult Index()
        {
            return View(db.HealthInsurances.ToList().OrderBy(o => o.Name));
        }

   
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

        
        public ActionResult Create()
        {
            return View();
        }

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

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

       
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

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            HealthInsurance healthInsurance = db.HealthInsurances.Find(id);
            if (healthInsurance == null)
            {
                return HttpNotFound();
            }
            return View(healthInsurance);
        }

      
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