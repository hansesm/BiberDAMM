using System;
using System.Collections;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;

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
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public void Create(ContactType ContactType)
        {
            try
            {
                ContactType.Name = "";
                db.ContactTypes.Add(ContactType);
                db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }

        //CHANGE: ContactType [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }

        //GET SINGLE: ContactType [JEL] [ANNAS]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var ContactTyp = db.ContactTypes.Find(id);
            if (ContactTyp == null)
                return HttpNotFound();
            return View(ContactTyp);
        }

        //SAVE: ContactType [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }
    }
}