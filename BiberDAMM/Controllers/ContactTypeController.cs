using System;
using System.Collections;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using System.Net;
using System.Linq;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers
{
    public class ContactTypeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private object[] id;

        public ContactType ContactTypes { get; private set; }

        // GET all: ContactType [JEL] [ANNAS]
        public ActionResult Index()
        {
            IEnumerable model = db.ContactTypes;
            return View(model);
        }

        //CREATE: ContactType [JEL] [ANNAS]
        //[HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactType ContactType)
        {
            ContactType checkContactType = db.ContactTypes.Where(c => c.Name == ContactType.Name).FirstOrDefault();
            if (checkContactType != null)
            {
                // Fehler alert schreiben
                return View();
            }
            db.ContactTypes.Add(ContactType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //CHANGE: ContactType [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }

        //GET SINGLE: ContactType [JEL] [ANNAS]
        public ActionResult Details(int? contactTypeId)
        {
            if (contactTypeId == null)
            {
                return RedirectToAction("Index");
            }
            var id = contactTypeId ?? default(int);
            ContactType _contactType = db.ContactTypes.Where(c => c.Id == contactTypeId).FirstOrDefault();

            return View(_contactType);
        }
        //SAVE: ContactType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
        //DELETE: ContactType [ANNAS]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contactType = db.ContactTypes.Find(id);
            if (contactType == null)
                return HttpNotFound();
            return View(contactType);
        }

        // POST: ContactType/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var contactType = db.ContactTypes.Find(id);
            db.ContactTypes.Remove(contactType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}