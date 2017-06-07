using System;
using System.Collections;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using System.Net;
using System.Linq;
using BiberDAMM.Helpers;
using System.Data.Entity;
using System.Collections.Generic;

namespace BiberDAMM.Controllers
{
    public class ContactTypeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        
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
            TempData["CreateContactTypeSaved_Bool"] = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactType ContactType)
        {
            ContactType checkContactType = db.ContactTypes.Where(c => c.Name == ContactType.Name).FirstOrDefault();
            if (checkContactType != null)
            {
                // Fehler alert schreiben und wenn es erfolgreich gespeichert ist
                TempData["CreateContactTypeSaved_Bool"] = "0";
                TempData["CreateContactTypeFailed"] = " Bei der Erstellung ist ein Fehler aufgetreten";
                return View();
            }
            db.ContactTypes.Add(ContactType);
            db.SaveChanges();

            TempData["CreateContactTypeSaved_Bool"] = "1";
            TempData["CreateContactTypeSaved"] = " Die Erstellung war erfolgreich";
            return View();
            //return RedirectToAction("Index");
        }

        [HttpGet]
        //CHANGE: ContactType [JEL] [ANNAS]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var ContactType = db.ContactTypes.Find(id);
            if (ContactType == null)
                return HttpNotFound();
            return View(ContactType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactType ContactType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ContactType).State = EntityState.Modified;
                db.SaveChanges();

                TempData["ContactTypeSuccess"] = "Daten erfolgreich gespeichert";
                return RedirectToAction("Details", new { id = ContactType.Id });
            }

            TempData["ContactTypeError"] = "Eingaben fehlerhaft oder unvollständig";
            return View(ContactType);
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

        //new ActionResult RedirectToAction(string v)
        //{
        //    throw new NotImplementedException();
        //}

        //SAVE: ContactType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
        //DELETE: ContactType [ANNAS]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var ContactType = db.ContactTypes.Find(id);
            if (ContactType == null)
                return HttpNotFound();
            return View(ContactType);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            var id = int.Parse(Request.QueryString["ContacktTypeId"]);
            var ContactType = db.ContactTypes.Find(id);
            IQueryable<ContactData> cd = db.ContactDatas.Where(c => c.ContactTypeId == id);
            if (cd.Count() != 0)
            {
                TempData["ContactTypeError"] = "Der Kontakttyp ist noch Patienten zugeordnet";
                return RedirectToAction("Details", "ContactType", new { id = ContactType.Id });
            }
            db.ContactTypes.Remove(ContactType);
            db.SaveChanges();

            TempData["ContactTypeSuccess"] = "Kontakttyp erfolgreich gelöscht";
            return RedirectToAction("Index");
        }
    }
}
