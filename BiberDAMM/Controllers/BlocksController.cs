using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;

/*This is the controller witch handels blocks.
 * Each Client "blocks" a bed when he sleeps a night in the Hospital 
 * Author: [HansesM]
 * */

namespace BiberDAMM.Controllers
{
    public class BlocksController : Controller
    //TODO Comment the class ! [HansesM]
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //TODO [HansesM] maybe display load-factor of beds for the hospital
        public ActionResult Index()
        {
            return View();
        }

        //CREATE: Blocks [HansesM]
        //Method for creating a new blocks, returns a create-blocks view with a view-model
        public ActionResult Create(int id)
        {
            //Gets the Stay from the given stay-id [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == id);
            //Eager-Loading not needed [HansesM]
            //_db.Entry(stay).Reference(m => m.Client).Load();

            if (stay != null)
            {
                var blocks = new Blocks();
                blocks.Stay = stay;
                blocks.StayId = stay.Id;

                //Gets a list of beds for the dropdownlist [HansesM]
                var listBedModels = _db.Beds.ToList();

                //Builds a selectesList out of the list of beds, only id and text are required [HansesM]
                //TODO [HansesM] Group-By to display only 1 model (Wait for Jean-Pierre to implement it as an enum)
                var selectetlistBedModels = new List<SelectListItem>();
                foreach (var m in listBedModels)
                    // usage of type conversion to get enum for bed types to work and be able to test it [Jean-Pierre]
                    selectetlistBedModels.Add(new SelectListItem { Value = (m.Model.ToString()) });

                //Creates a View-Model and returns the view with the view-model inside [HansesM]
                var viewModel = new BlocksCreateViewModel(blocks, selectetlistBedModels);
                return View(viewModel);
            }
            //TODO [Hansesm] No stay found
            return RedirectToAction("Index", "Stay");
        }

        //Post Method for creating new Stay [HansesM]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blocks blocks, string command)
        {

            //If abort button is pressed we get a new stay-details-view and dismiss all changes [HansesM]
            if (command.Equals(ConstVariables.AbortButton))
            {
                //Returns the user and displays a alert [HansesM]
                TempData["CreateBlocksAbort"] = " erfolgreich abgebrochen.";
                return RedirectToAction("Details", "Stay", new { id = blocks.StayId });
            }

            //Checks if Modelstate is valid [HanseM]
            if (ModelState.IsValid)
            {
                _db.Blocks.Add(blocks);
                _db.SaveChanges();

                TempData["CreateBlocksSuccsess"] = "Neue Übernachtung erfasst.";
                return RedirectToAction("Details", "Stay", new { id = blocks.StayId });
            }

            //Gets the Stay from the stayid in blocks [HansesM]
            var stay = _db.Stays.SingleOrDefault(m => m.Id == blocks.StayId);
            blocks.Stay = stay;

            //Gets a list of beds for the dropdownlist [HansesM]
            var listBedModels = _db.Beds.ToList();

            //Builds a selectesList out of the list of beds, only id and text are required [HansesM]
            //TODO [HansesM] Group-By to display only 1 model (Wait for Jean-Pierre to implement it as an enum)
            var selectetlistBedModels = new List<SelectListItem>();
            foreach (var m in listBedModels)
                // usage of type conversion to get enum for bed types to work and be able to test it [Jean-Pierre]
                selectetlistBedModels.Add(new SelectListItem { Value = (m.Model.ToString()) });

            //Creates a View-Model and returns the view with the view-model inside [HansesM]
            var viewModel = new BlocksCreateViewModel(blocks, selectetlistBedModels);

            return View(viewModel);
        }


        //GET SINGLE: Blocks [HansesM]
        public ActionResult Details(int id)
        {
            var room = _db.Blocks.SingleOrDefault(m => m.Id == id);

            if (room == null)
                return HttpNotFound();

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Blocks blocks, string command)
        {
            //If abort button is pressed we get a new stay-details-view and dismiss all changes [HansesM]
            if (command.Equals(ConstVariables.AbortButton))
            {
                //Returns the user and displays a alert [HansesM]
                TempData["DeleteBlocksAbort"] = " erfolgreich abgebrochen.";
                return RedirectToAction("Details", "Blocks", new { id = blocks.Id });
            }

            var  blocksInDb = _db.Blocks.SingleOrDefault(m => m.Id == blocks.Id);
            _db.Blocks.Remove(blocksInDb);
            _db.SaveChanges();

            TempData["DeleteBlocksSuccsess"] = " Übernachtung erfolgreich gelöscht.";
            return RedirectToAction("Details", "Stay", new { id = blocks.StayId });
        }

        //Post-method witch will be called by create-blocks-view 
        //Jquery-Ajax and returns a list of "free" beds at the given date, roomtype and model combination
        //[HansesM]
        [HttpPost]
        public JsonResult getFreeBeds(string begin, string end, string roomType, string model)
        {
            //First the given roomtype will be transformed into an expression for the sql-search [HansesM]
            switch (roomType)
            {
                case "0":
                    roomType = "=1";
                    break;
                case "1":
                    roomType = "=2";
                    break;
                case "2":
                    roomType = ">=3";
                    break;
                default:
                    roomType = ">=3";
                    break;
            }

            //Gets a list of free beds, matching the given parameters! [HansesM]
            //TODO TEST IT !!!!
            var events = _db.Beds.SqlQuery("select * from Beds b where b.RoomId in " +
                                           "(select RoomId from beds group by RoomId having count(*) " + roomType +
                                           ")" +
                                           "AND b.Model like '" + model + "'" +
                                           "AND b.Id not in" +
                                           "(select BedId from blocks where " +
                                           "BeginDate between convert(datetime, '" + begin +
                                           "', 104) and convert(datetime, '" + end + "', 104)" +
                                           "and EndDate between convert(datetime, '" + begin +
                                           "', 104) and convert(datetime, '" + end + "', 104))");

            //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
            var result = events.Select(e => new JsonEvent
            {
                value = e.Id.ToString(),
                //TODO change to something with more sense
                text = e.Model.ToString() + " " + e.Id.ToString() + " in Raum " + e.Room.RoomNumber.ToString()
            }).ToList();

            //Creates a JsonResult from the Json [HansesM]
            var resultJson = new JsonResult { Data = result };

            //returns the Json to the calling-function [HansesM]
            return Json(result);
        }


        //Override on Dispose for security reasons. [HansesM]
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        //Inline-Class for for displaying treatment-data in a calendar view, needed in the Details-Method [HansesM]
        public class JsonEvent
        {
            public string value { get; set; }
            public string text { get; set; }
        }
    }
}