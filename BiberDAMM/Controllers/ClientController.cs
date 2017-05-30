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

            Client client = (Client)Session["TempNewClient"];

            if (client == null)
            {
                client = new Client();
                client.Birthdate = DateTime.Now;

            }

            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            //Null the Editing Client for preventing user to get into undefined state
            Session["TempClient"] = null;

            if (Request.Form["Save"] != null)
            {
                if (ModelState.IsValid)
                {
                    client.Captured = DateTime.Now;
                    client.LastUpdated = DateTime.Now;
                    client.RowVersion = 1;
                    db.Clients.Add(client);
                    db.SaveChanges();

                    int clientID = db.Clients.Max(u => u.Id);

                    //Update Contact-Rows which are temporary inserted with Null-Value  

                    var results = from p in db.ContactDatas select p;
                    results = results.Where(s =>  s.ClientId==null);

                    foreach (ContactData c in results)
                    {
                        c.ClientId = clientID;
                    }

                    db.SaveChanges();



                    return RedirectToAction("Index");
                }
                return View(client);
            }
            else if (Request.Form["Cancel"] != null)
            {
                Session["TempNewClient"] = null;
                return RedirectToAction("Index");
            }
            else if (Request.Form["ChangeHealthInsurance"] != null)
            {
                Session["TempNewClient"] = client;
                TempData["RedirectFromClient"]=true;
                return RedirectToAction("Index", "HealthInsurance");
            }
            else 
            {
                //Redirect to ContactData
                Session["TempNewClient"] = client;
                TempData["RedirectFromClient"] = true;
                return RedirectToAction("Index", "ContactData");
            }

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

            if (tempClient != null && tempClient.Id == client.Id)
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
            //Null the New Client for preventing user to get into undefined state
            Session["TempNewClient"] = null;
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
            else if (Request.Form["Cancel"] != null)
            {
                Session["TempClient"] = null;
                return RedirectToAction("Index");
            }
            else if (Request.Form["EditContacts"] != null)
            {
                Session["TempClient"] = client;
                TempData["RedirectFromClient"] = true;
                return RedirectToAction("Index", "ContactData");
            }
            else
            {
                //Redirect to edit HealthInsurances
                Session["TempClient"] = client;
                TempData["RedirectFromClient"] = true;
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