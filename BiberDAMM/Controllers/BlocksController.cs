using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
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

            if (stay != null)
            {
                var newBlocks = new Blocks();
                newBlocks.Stay = stay;
                newBlocks.StayId = stay.Id;

                //Gets a list of beds for the dropdownlist [HansesM]
                var listBedModels = _db.Beds.ToList();

                //Builds a selectesList out of the list of beds, only id and text are required [HansesM]
                //TODO [HansesM] Group-By to display only 1 model (Wait for Jean-Pierre to implement it as an enum)
                var selectetlistBedModels = new List<SelectListItem>();
                foreach (var m in listBedModels)
                    selectetlistBedModels.Add(new SelectListItem {Text = m.Model, Value = m.Id.ToString()});

                //Creates a View-Model and returns the view with the view-model inside [HansesM]
                var viewModel = new BlocksCreateViewModel(newBlocks, stay, selectetlistBedModels);
                return View(viewModel);
            }
            //TODO [Hansesm] No stay found
            return RedirectToAction("Index", "Stay");
        }
        
        //GET SINGLE: Blocks [HansesM]
        public ActionResult Detail()
        {
            //TODO Display Blocks-Details with Option to delete the blocks - no edit possible
            return View();
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
            var resultJson = new JsonResult {Data = result};

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