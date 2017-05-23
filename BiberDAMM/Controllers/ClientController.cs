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
using BiberDAMM.Security;

namespace BiberDAMM.Controllers
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    [CustomAuthorize]
    public class ClientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Getter for the Index-Page requires searchString for filtering entries
        public ActionResult Index(string searchString)
        {
            IQueryable<Client> clients = new List<Client>().AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = from m in db.Clients
                          select m;
                clients = clients.Where(s => s.Lastname.Contains(searchString) || s.Surname.Contains(searchString)
                || s.Sex.ToString().Contains(searchString) || s.InsuranceNumber.ToString().Contains(searchString));
            }

            return View(clients.OrderBy(o => o.Lastname));
        }


        //Gettter for the Details-Page

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
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
            {
                return RedirectToAction("Index");
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.HealthInsuranceId = new SelectList(db.HealthInsurances, "Id", "Name", client.HealthInsuranceId);
            return View(client);
        }

        [HttpPost]
        public JsonResult Edit(string Prefix)
        {
            //Note : you can bind same list from database  
            var healthInsurances = db.HealthInsurances;

            //Searching records from list using LINQ query  
            var Name = (from N in healthInsurances
                                        where N.Name.StartsWith(Prefix)
                            select new { N.Name });
            return Json(Name, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                client.LastUpdated = DateTime.Now;
                client.RowVersion += 1;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HealthInsuranceId = new SelectList(db.HealthInsurances, "Id", "Name", client.HealthInsuranceId);
            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHealthInsurance(Client client)
        {

            client.LastUpdated = DateTime.Now;
            client.RowVersion += 1;
            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.TempClient = client;
            return RedirectToAction("Index", "HealthInsurance");



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
