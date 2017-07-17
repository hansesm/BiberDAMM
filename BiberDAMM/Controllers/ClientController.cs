using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //Getter for the Index-Page requires searchString for filtering entries, if searchString is empty, nothing will be displayed in order to optimize database-performance
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist + "," + ConstVariables.RoleNurse)]
        public ActionResult Index(string searchString)
        {
            Session["TempClient"] = null;
            Session["ClientIndexPage"] = "Index";

            var clients = new List<Client>().AsQueryable();
            var clientsOut = new List<Client>();

            if (!string.IsNullOrEmpty(searchString))
            {
                string[] searchStringSplit = searchString.Split(' ');

                clients = from m in db.Clients
                          select m;


                //Split Searchstring to search for multiple substrings
                foreach (string currentSearchString in searchStringSplit)
                {
                    var clientsTemp = clients.Where(s => s.Lastname.Contains(currentSearchString) || s.Surname.Contains(currentSearchString)
                                             || s.Sex.ToString().Contains(currentSearchString) || s.InsuranceNumber.ToString()
                                                 .Contains(currentSearchString));

                    //Check if Client already contained in out-List
                    foreach (Client actTempClient in clientsTemp)
                    {
                        Boolean clientConained = false;

                        foreach (Client actOutClient in clientsOut)
                        {
                            if (actOutClient.Id == actTempClient.Id)
                            {
                                clientConained = true;
                            }
                        }
                        //add Client if not contained
                        if (!clientConained)
                        {
                            clientsOut.Add(actTempClient);
                        }
                    }
                }

            }


            return View(clientsOut.OrderBy(o => o.Lastname));
        }


        //Gets the Last N modified Clients, if the given N is null, there will be the last 30 Clients taken
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist + "," + ConstVariables.RoleNurse)]
        public ActionResult LastNIndex(int? count)
        {
            Session["TempClient"] = null;
            Session["ClientIndexPage"] = "LastNIndex";
            int innerCount = 30;
            if (count != null && count != 0)
            {
                innerCount = count ?? default(int);
            }


            var clients = new List<Client>().AsQueryable();


            clients = from m in db.Clients
                      select m;
            clients.OrderBy(o => o.LastUpdated);

            return View(clients.Take(innerCount).OrderBy(o => o.Lastname));



        }


        //Gettter for the Details-Page
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleTherapist + "," + ConstVariables.RoleNurse)]
        public ActionResult Details(int? id)
        {
            //Session["TempClient"] = null;
            if (id == null)
            {
                string redirectString = (String)Session["ClientIndexPage"];
                if (redirectString == null || redirectString.Equals(""))
                {
                    redirectString = "LastNIndex";
                }
                return RedirectToAction(redirectString);
            }
            var client = db.Clients.Find(id);
            if (client == null)
                return HttpNotFound();

            Session["TempClient"] = client;
            return View(client);
        }




        //Getter and Setter for the creating-Page
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse + "," + ConstVariables.RoleTherapist)]
        public ActionResult Create()
        {

            Client client = (Client)Session["TempNewClient"];
            TempData["ClientSuccess"] = "Werte erfolgreich hinzugefügt";

            if (client == null)
            {
                client = new Client();
                client.Birthdate = DateTime.Now;
                TempData["ClientSuccess"] = null;

            }

            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse + "," + ConstVariables.RoleTherapist)]
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
                    Session["TempNewClient"] = null;


                    TempData["ClientSuccess"] = "Patient erfolgreich hinzugefügt!";

                    return RedirectToAction("Edit", new { id = client.Id });
                }
                TempData["ClientError"] = "Daten unvollständig oder fehlerhaft";
                return View(client);
            }
            else if (Request.Form["Cancel"] != null)
            {
                Session["TempNewClient"] = null;

                TempData["ClientError"] = "Bearbeitung abgebrochen";
                string redirectString = (String)Session["ClientIndexPage"];
                if (redirectString == null || redirectString.Equals(""))
                {
                    redirectString = "LastNIndex";
                }
                return RedirectToAction(redirectString);
            }
            else if (Request.Form["ChangeHealthInsurance"] != null)
            {
                Session["TempNewClient"] = client;
                TempData["RedirectFromClient"] = true;
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse + "," + ConstVariables.RoleTherapist)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                string redirectString = (String)Session["ClientIndexPage"];
                if (redirectString == null || redirectString.Equals(""))
                {
                    redirectString = "LastNIndex";
                }
                return RedirectToAction(redirectString);
            }
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
        [CustomAuthorize(Roles = ConstVariables.RoleAdministrator + "," + ConstVariables.RoleDoctor + "," + ConstVariables.RoleNurse + "," + ConstVariables.RoleTherapist)]
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
                    TempData["ClientSuccess"] = "Änderungen gespeichert";
                    return RedirectToAction("Details", new { id = client.Id });
                }
                TempData["ClientError"] = "Eingaben fehlerhaft oder unvollständig";
                return View(client);
            }
            else if (Request.Form["Cancel"] != null)
            {
                Session["TempClient"] = null;
                TempData["ClientError"] = "Bearbeitung abgebrochen";
                string redirectString = (String)Session["ClientIndexPage"];
                if (redirectString == null || redirectString.Equals(""))
                {
                    redirectString = "LastNIndex";
                }
                return RedirectToAction(redirectString);
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