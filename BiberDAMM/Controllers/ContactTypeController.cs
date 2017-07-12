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
using System.Web.Security;

namespace BiberDAMM.Controllers
{
    [Security.CustomAuthorize(Roles = ConstVariables.RoleAdministrator + ",")]
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
            try
            {
                ContactType checkContactType = db.ContactTypes.Where(c => c.Name == ContactType.Name).FirstOrDefault();
                if (ContactType.Name == "")
                {
                    TempData["CreateContactTypeSaved_Bool"] = "0";
                    TempData["CreateContactTypeSaved"] = "Der Name darf nicht leer sein.";
                    return RedirectToAction("Create");
                }
                if (checkContactType != null)
                {
                    // Fehler alert schreiben und wenn es erfolgreich gespeichert ist
                    TempData["CreateContactTypeSaved_Bool"] = "0";
                    TempData["CreateContactTypeFailed"] = " Bei der Erstellung ist ein Fehler aufgetreten";
                    return View();
                }
                if (ModelState.IsValid)
                {
                    db.ContactTypes.Add(ContactType);
                    db.SaveChanges();
                }

                TempData["CreateContactTypeSaved_Bool"] = "1";
                TempData["CreateContactTypeSaved"] = " Die Erstellung war erfolgreich";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["CreateContactTypeSaved_Bool"] = "0";
                TempData["CreateContactTypeSaved"] = "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut.";
                return View(ContactType);
            }

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
            try
            {
                if (ContactType.Name == "")
                {
                    return View(ContactType);
                }
                List<ContactData> usedContactTypes = db.ContactDatas.Where(cd => cd.ContactTypeId == ContactType.Id).ToList();
                List<ContactType> existingContactTypes = db.ContactTypes.Where(cd => cd.Name == ContactType.Name).ToList();
                if (usedContactTypes.Count > 0)
                {
                    TempData["ContactTypeError"] = "Dieser Kontakttyp ist in Verwendung und kann nicht geändert werden.";
                    return RedirectToAction("Details");
                }
                if(existingContactTypes.Count > 0)
                {
                    TempData["ContactTypeError"] = "Ein Kontakttyp mit diesem Namen ist bereits vorhanden.";
                    return View(ContactType);
                }
                else if (ModelState.IsValid)
                {
                    db.Entry(ContactType).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ContactTypeSuccess"] = "Daten erfolgreich gespeichert";
                    return RedirectToAction("Details", new { id = ContactType.Id });
                }

                TempData["ContactTypeError"] = "Eingaben fehlerhaft oder unvollständig";
                return View(ContactType);
            }
            catch (Exception)
            {
                TempData["EditContactTypeSaved_Bool"] = "0";
                TempData["EditContactTypeSaved"] = "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut.";
                return View(ContactType);
            }
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

