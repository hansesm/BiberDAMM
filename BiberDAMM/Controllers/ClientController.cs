using System;
using System.Collections.Generic;
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
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //Getter for the Index-Page requires searchString for filtering entries
        public ActionResult Index(string searchString)
        {
            var clients = new List<Client>().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = from m in db.Clients
                          select m;
                clients = clients.Where(s => s.Lastname.Contains(searchString) || s.Surname.Contains(searchString)
                                             || s.Sex.ToString().Contains(searchString) || s.InsuranceNumber.ToString()
                                                 .Contains(searchString));
            }

            return View(clients.OrderBy(o => o.Lastname));
        }


        //Gettter for the Details-Page

        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var client = db.Clients.Find(id);
            if (client == null)
                return HttpNotFound();
            return View(client);
        }


        //Getter and Setter for the creating-Page

        public ActionResult Create()
        {
            ViewBag.HealthInsuranceId = new SelectList(db.HealthInsurances, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Captured = DateTime.Now;
                client.LastUpdated = DateTime.Now;
                client.RowVersion = 1;
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HealthInsuranceId = new SelectList(db.HealthInsurances, "Id", "Name", client.HealthInsuranceId);
            return View(client);
        }


        //Getter and Setter for the Editing Page

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var client = db.Clients.Find(id);
            if (client == null)
                return HttpNotFound();


            Client tempClient = (Client)Session["TempClient"];

            if (tempClient != null && tempClient.Id==client.Id)
            {
                client = tempClient;
            }
            else
            {
                Session["TempClient"] = null;
            }

            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            //Check if the Contents should only be temporarly saved for adding e Healthinsurance
            if (Request.Form["Save"] != null)
            {
                client.LastUpdated = DateTime.Now;
                if (ModelState.IsValid)
                {
                    client.RowVersion += 1;
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(client);
            }
            //Set HealthInsurance
            else if (Request.Form["ChangeHealthInsurance"] != null)
            {
                Session["TempClient"] = client;
                return RedirectToAction("Index", "HealthInsurance");
            }
            //Add Contacts
            else
            {
                return RedirectToAction("Index", "HealthInsurance");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}