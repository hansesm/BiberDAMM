using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.Helpers;
using System.Collections.Generic;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class TreatmentTypeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET all: TreatmentType [JEL]
        public ActionResult Index()
        {
            return View(db.TreatmentTypes.ToList());
        }

        ///CREATE: TreatmentType [JEL] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TreatmentType TreatmentType)
        {
            //checks if treatmenttype is already in use; an alert will be displayed
            if (db.TreatmentTypes.Any(r => r.Name.Equals(TreatmentType.Name)))
            {
                TempData["CreateTreatmentTypeFailed"] = " " + TreatmentType.Name.ToString() + " existiert bereits.";
                return RedirectToAction("Create", TreatmentType);
            }
            if (ModelState.IsValid)
            {
                db.TreatmentTypes.Add(TreatmentType);
                db.SaveChanges();
                TempData["CreateTreatmentTypeSuccess"] = " " + TreatmentType.Name.ToString() + " wurde hinzugefügt.";
                return RedirectToAction("Index");
            }
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            //adds one epmty item to give the list (used as a dropdownList) a optional empty entry
            selectedListRoomTypes.Add(new SelectListItem { Text = " ", Value = null });
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }
            var treatmentType = new TreatmentType();
            var viewModel = new TreatmentTypeViewModel(treatmentType, selectedListRoomTypes);
            return View(viewModel);
            
        }
        //CREATE: TreatmentType [JEL] 
        public ActionResult Create()
        {
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            //adds one epmty item to give the list (used as a dropdownList) a optional empty entry
            selectedListRoomTypes.Add(new SelectListItem{ Text = " ", Value = null });
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }
            var treatmentType = new TreatmentType();
            var viewModel = new TreatmentTypeViewModel(treatmentType, selectedListRoomTypes);
            return View(viewModel);
        }

        //GET SINGLE: TreatmentType [JEL]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var treatmentType = db.TreatmentTypes.Find(id);
            if (treatmentType == null)
                return HttpNotFound();
            return View(treatmentType);
        }
        //CHANGE: TreatmentType [JEL] 
        public ActionResult Edit(int? id)
        {      
            if (id == null)
                return RedirectToAction("Index");
            var treatmentType = db.TreatmentTypes.Find(id);
            if (treatmentType == null)
                return HttpNotFound();

            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            //adds one epmty item to give the list (used as a dropdownList) a optional empty entry
            selectedListRoomTypes.Add(new SelectListItem { Text = " ", Value = null });
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }
            
            var viewModel = new TreatmentTypeViewModel(treatmentType, selectedListRoomTypes);
            return View(viewModel);
        }
        //Post CHANGE: TreatmentType [JEL] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TreatmentType treatmentType, string command)
        {
            if (command.Equals(ConstVariables.AbortButton))
            {
                return RedirectToAction("Details", "TreatmentType", new { id = treatmentType.Id });
            }
            //checks if treatmentType exists already and if the current id is not equal to an existing id
            if (db.TreatmentTypes.Any(r => r.Name.Equals(treatmentType.Name) && !(r.Id.Equals(treatmentType.Id))))
            {
                TempData["EditTreatmentTypeFailed"] = " " + treatmentType.Name.ToString() + " existiert bereits.";
                return RedirectToAction("Edit", treatmentType);
            }
            if (ModelState.IsValid)
            {
                db.Entry(treatmentType).State = EntityState.Modified;
                db.SaveChanges();
                //if update succeeded
                TempData["EditTreatmentTypeSuccess"] = " " + treatmentType.Name.ToString() + " wurde aktualisiert.";
                return RedirectToAction("Details", "treatmentType", new { id = treatmentType.Id });
            }
            //Get all roomTypes from the database
            var listRoomTypes = db.RoomTypes;
            var selectedListRoomTypes = new List<SelectListItem>();
            foreach (var m in listRoomTypes)
            {
                selectedListRoomTypes.Add(new SelectListItem { Text = m.Name, Value = (m.Id.ToString()) });
            }
            var viewModel = new TreatmentTypeViewModel(treatmentType, selectedListRoomTypes);
            return View(viewModel);
           
        }
        //DELETE: TreatmentType [JEL]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            var treatmentType = db.TreatmentTypes.Find(id);
            if (treatmentType == null)
                return HttpNotFound();

            // check if there are dependencies
            //Treatment dependentTreatment = db.Rooms.Where(t => t.Id == id).SelectMany(u => u.Treatment).FirstOrDefault();
            Treatment dependentTreatment = db.TreatmentTypes.Where(tt => tt.Id == id).SelectMany(u => u.Treatments).FirstOrDefault();

            //// if there is a treatment that is linked to the treatmenttype, the treatmenttype can't be deleted
            if (dependentTreatment != null)
            {
                // failure-message for alert-statement
                TempData["DeleteTreatmentTypeFailed"] = " Es bestehen Abhängigkeiten zu einer vorhandenen Behandlung.";
                return RedirectToAction("Details", "treatmentType", new { id });
            }
            db.TreatmentTypes.Remove(treatmentType);
            db.SaveChanges();
            TempData["DeleteTreatmentTypeSuccess"] = " " + treatmentType.Name.ToString() + " wurde entfernt.";
            return RedirectToAction("Index");
        }
    }
}