using BiberDAMM.DAL;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using BiberDAMM.ViewModels;
using BiberDAMM.Helpers;

namespace BiberDAMM.Controllers
{
    public class TreatmentController : Controller
    {
        //The Database-Context [KrabsJ]
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Treatment/Create [KrabsJ]
        public ActionResult Create()
        {
            return View();
        }

        // GET Treatment/SelectTreatmentType/id [KrabsJ]
        // first step of creating a new treatment is to select a treatment type
        public ActionResult SelectTreatmentType(int id)
        {
            // check if the stay with the given id exists
            int stayId;
            if (_db.Stays.Any(s => s.Id == id))
            {
                stayId = id;
            }
            else
            {
                // if there is no stay with the given id, show home index page and failure alert
                TempData["UnexpectedFailure"] = " Es konnte kein Aufenthalt mit der übergebenen Id gefunden werden.";
                return RedirectToAction("Index", "Home");
            }

            //get all treatment types from db an put them into a selectList
            var listTreatmentTypes = _db.TreatmentTypes.ToList();

            var selectetListTreatmentTypes = new List<SelectListItem>();
            foreach (var t in listTreatmentTypes)
            {
                selectetListTreatmentTypes.Add(new SelectListItem { Text = (t.Name), Value = (t.Id.ToString()) });
            }

            // create the viewModel
            var viewModelSelectTreatmentType = new CreationTreatmentSelectType();
            viewModelSelectTreatmentType.StayId = stayId;
            viewModelSelectTreatmentType.ListTreatmentTypes = selectetListTreatmentTypes;

            return View(viewModelSelectTreatmentType);
        }

        //POST: Treatment/SelectTreatmentType [KrabsJ]
        // first step of creating a new treatment is to select a treatment type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectTreatmentType(CreationTreatmentSelectType model, string command)
        {
            // if the abort button was clicked return to the stay details page
            if (command.Equals(ConstVariables.AbortButton))
                return RedirectToAction("Details", "Stay", new { id = model.StayId });

            // else go to the create method
            return RedirectToAction("Create", "Treatment");

        }
    }
}