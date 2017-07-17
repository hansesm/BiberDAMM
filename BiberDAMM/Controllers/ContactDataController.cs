﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Security;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    [CustomAuthorize]
    public class ContactDataController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        private Client getCachedClient()
        {
            Client client = null;
            if (Session["TempNewClient"] != null)
            {
                client = (Client)Session["TempNewClient"];
            }
            else
            {
                client = (Client)Session["TempClient"];
            }

            return client;

        }

        //Method which decides to which client page should be redirected
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult GoBackToClient()
        {
            Client cachedClient = getCachedClient();
            if (Session["TempNewClient"] != null)
            {
                return RedirectToAction("Create", "Client");
            }
            else if (Session["TempClient"] != null)
            {
                return RedirectToAction("Edit", "Client", new { id = cachedClient.Id });
            }
            else
            {
                ContactData data = (ContactData)Session["DetailViewContact"];
                return RedirectToAction("Details", "Client", new { id = data.ClientId });
            }
        }


        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Index()
        {
            //To prevent rendering the Page if no Client is cached
            Client cachedClient = getCachedClient();
            if (cachedClient == null)
            {
                return RedirectToAction("Index", "Client");
            }

            ViewBag.Title = "Kontaktübersicht für Patient: " + getCachedClient().Lastname + ", " + getCachedClient().Surname;


            //To show values which are linked to a new Client with a 0-ID
            int? clientID = cachedClient.Id;
            if (clientID == 0)
            {
                clientID = null;
            }
            var contactDatas = db.ContactDatas.Where(c => c.ClientId == clientID);
            return View(contactDatas.ToList());
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            Session["DetailViewContact"] = contactData;
            return View(contactData);
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname");
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name");
            return View(new ContactData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Create(ContactData contactData)
        {
            if (Request.Form["Save"] != null)
            {

                if (ModelState.IsValid)
                {
                    Client cachedClient = (Client)getCachedClient();
                    if (cachedClient.Id != 0)
                    {
                        contactData.ClientId = cachedClient.Id;
                    }
                    else
                    {
                        contactData.ClientId = null;
                    }

                    db.ContactDatas.Add(contactData);
                    db.SaveChanges();
                    TempData["ContactDataSuccess"] = "Daten erfolgreich gespeichert";
                    return RedirectToAction("Index");
                }

                TempData["ContactDataError"] = "Eingegebene Daten unvollständig oder fehlerhaft";
                return View(contactData);
            }
            else
            {
                //Creating was canceled
                TempData["ContactDataError"] = "Bearbeitung abgebrochen";
                return RedirectToAction("Index");
            }
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname", contactData.ClientId);
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
            return View(contactData);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Edit(ContactData contactData)
        {
            if (Request.Form["Save"] != null)
            {
                if (ModelState.IsValid)
                {

                    Client cachedClient = (Client)getCachedClient();
                    if (cachedClient != null && cachedClient.Id != 0)
                    {
                        contactData.ClientId = cachedClient.Id;
                    }
                    else
                    {
                        contactData.ClientId = null;
                    }
                    db.Entry(contactData).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ContactDataSuccess"] = "Daten erfolgreich gespeichert";
                    return RedirectToAction("Details", new { id = contactData.Id });
                }
                ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
                TempData["ContactDataError"] = "Eingaben fehlerhaft oder unvollständig";
                return View(contactData);
            }
            else
            {
                //Editing was canceled
                TempData["ContactDataError"] = "Bearbeitung wurde abgebrochen";
                return RedirectToAction("Index");
            }
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            return View(contactData);
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult DeleteCheck(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            TempData["ContactDeleteConfirmation"] = true;
            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        public ActionResult DeleteConfirmed(int id)
        {
            var contactData = db.ContactDatas.Find(id);
            int clientID = contactData.ClientId ?? default(int);
            db.ContactDatas.Remove(contactData);
            db.SaveChanges();
            TempData["ContactDataSuccess"] = "Kontakt erfolgreich gelöscht";
            TempData["ClientSuccess"] = "Kontakt erfolgreich gelöscht";
            if (getCachedClient() == null)
            {
                return RedirectToAction("Details", "Client", new { id = clientID });
            }
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist)]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}