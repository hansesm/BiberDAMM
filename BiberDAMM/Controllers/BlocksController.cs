using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;

namespace BiberDAMM.Controllers
{
    public class BlocksController : Controller
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET all: Blocks [JEL] [ANNAS]
        public ActionResult Index()
        {
            return View();
        }

        //CREATE: Blocks [HansesM]
        public ActionResult Create(int id)
        {
            //Gets the Stay from the given id [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == id);

            if (stay != null)
            {
                var newBlocks = new Blocks();
                newBlocks.Stay = stay;
                newBlocks.StayId = stay.Id;

                //Gets a list of Doctors for the Dropdownlist [HansesM]
                var listBedModels = _db.Beds.ToList();
                //Builds a selectesList out of the list of doctors [HansesM]
                var selectetlistBedModels = new List<SelectListItem>();
                foreach (var m in listBedModels)
                {
                    selectetlistBedModels.Add(new SelectListItem { Text = m.Model.ToString(), Value = (m.Id.ToString()) });
                }
                
                var viewModel = new BlocksCreateViewModel(newBlocks, stay, selectetlistBedModels);
                return View(viewModel);
            }
            else
            {
                //TODO No stay found
                return RedirectToAction("Index", "Stay");
            }
        }

        //CHANGE: Blocks [JEL] [ANNAS]
        public ActionResult Edit()
        {
            return View();
        }

        //GET SINGLE: Blocks [JEL] [ANNAS]
        public ActionResult Detail()
        {
            return View();
        }

        //SAVE: Blocks [JEL] [ANNAS]
        public ActionResult Save()
        {
            return View();
        }

        //Override on Dispose for security reasons. [HansesM]
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}