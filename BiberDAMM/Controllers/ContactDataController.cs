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
    public class ContactDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContactData
        public ActionResult Index()
        {
            var contactDatas = db.ContactDatas.Include(c => c.Client).Include(c => c.ContactType);
            return View(contactDatas.ToList());
        }

        // GET: ContactData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactData contactData = db.ContactDatas.Find(id);
            if (contactData == null)
            {
                return HttpNotFound();
            }
            return View(contactData);
        }

        // GET: ContactData/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname");
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name");
            return View();
        }

        // POST: ContactData/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Email,Phone,Mobile,Street,Postcode,City,ClientId,ContactTypeId")] ContactData contactData)
        {
            if (ModelState.IsValid)
            {
                db.ContactDatas.Add(contactData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname", contactData.ClientId);
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
            return View(contactData);
        }

        // GET: ContactData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactData contactData = db.ContactDatas.Find(id);
            if (contactData == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname", contactData.ClientId);
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
            return View(contactData);
        }

        // POST: ContactData/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Email,Phone,Mobile,Street,Postcode,City,ClientId,ContactTypeId")] ContactData contactData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Surname", contactData.ClientId);
            ViewBag.ContactTypeId = new SelectList(db.ContactTypes, "Id", "Name", contactData.ContactTypeId);
            return View(contactData);
        }

        // GET: ContactData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactData contactData = db.ContactDatas.Find(id);
            if (contactData == null)
            {
                return HttpNotFound();
            }
            return View(contactData);
        }

        // POST: ContactData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactData contactData = db.ContactDatas.Find(id);
            db.ContactDatas.Remove(contactData);
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
