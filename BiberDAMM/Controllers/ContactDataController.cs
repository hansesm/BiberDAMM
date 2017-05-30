using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;

namespace BiberDAMM.Controllers
{
    // ===============================
    // AUTHOR     : ChristesR
    // ===============================
    public class ContactDataController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private Client getCachedClient()
        {
            Client client = null;
            if (Session["TempNewClient"] != null)
            {
                client = (Client)Session["TempNewClient"];
            }
            else if (Session["TempViewClient"] != null)
            {
                client = (Client)Session["TempViewClient"];
            }
            else
            {
                client = (Client)Session["TempClient"];
            }
            
            return client;

        }

        public ActionResult GoBackToClient()
        {
            Client cachedClient = getCachedClient();
            if (Session["TempNewClient"] != null)
            {
                return RedirectToAction("Create", "Client");
            }
            else if (Session["TempViewClient"] != null)
            {
                return RedirectToAction("Details", "Client", new { id = cachedClient.Id });
            }
            else
            {
                return RedirectToAction("Edit", "Client", new { id = cachedClient.Id });
            }
        }



        public ActionResult Index()
        {
            //To prevent rendering the Page if no Client is cached
            Client cachedClient = getCachedClient();
            if (cachedClient == null)
            {
                return RedirectToAction("Index", "Client");
            }

            ViewBag.Title = "Kontaktübersicht für Patient " + getCachedClient().Id;
            

            //To show values which are linked to a new Client with a 0-ID
            int? clientID = cachedClient.Id;
            if(clientID==0)
            {
                clientID = null;
            }
            var contactDatas = db.ContactDatas.Where(c => c.ClientId == clientID);
            return View(contactDatas.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            return View(contactData);
        }

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname");
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name");
            return View(new ContactData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactData contactData)
        {
            if (Request.Form["Save"] != null)
            {

                if (ModelState.IsValid)
                {
                    Client cachedClient = (Client)getCachedClient();
                    if(cachedClient.Id!=0)
                    {
                        contactData.ClientId = cachedClient.Id;
                    }
                    else
                    {
                        contactData.ClientId = null;
                    }

                    db.ContactDatas.Add(contactData);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


                return View(contactData);
            }
            else
            {
                //Creating was canceled
                return RedirectToAction("Index");
            }
        }


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
        public ActionResult Edit(ContactData contactData)
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
                db.Entry(contactData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
            return View(contactData);
            }
            else
            {
                //Editing was canceled
                return RedirectToAction("Index");
            }
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var contactData = db.ContactDatas.Find(id);
            if (contactData == null)
                return HttpNotFound();
            return View(contactData);
        }


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
        public ActionResult DeleteConfirmed(int id)
        {
            var contactData = db.ContactDatas.Find(id);
            db.ContactDatas.Remove(contactData);
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