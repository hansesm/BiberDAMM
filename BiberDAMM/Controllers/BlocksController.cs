using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using BiberDAMM.ViewModels;
using Microsoft.Ajax.Utilities;

namespace BiberDAMM.Controllers
{
    public class BlocksController : Controller
        //TODO Comment the class ! [HansesM]
    {
        //The Database-Context [HansesM]
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //Inline-Class for for displaying treatment-data in a calendar view, needed in the Details-Method [HansesM]
        public class JsonEvent
        {
            public string value { get; set; }
            public string text { get; set; }
        }

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

        public JsonResult getFreeBeds(string begin, string end, string roomType, string model)
        {
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
                    roomType= ">=3";
                    break;
            }

            //Gets a list of free beds, matching the given parameters! [HansesM]
            //TODO TEST IT !!!!
            var events = _db.Beds.SqlQuery("select * from Beds b where b.RoomId in " +
                                           "(select RoomId from beds group by RoomId having count(*) " + roomType + ")" +
                                           "AND b.Model like '"+ model + "'" +
                                           "AND b.Id not in" +
                                           "(select BedId from blocks where " +
                                           "BeginDate between convert(datetime, '"+ begin + "', 104) and convert(datetime, '" + end + "', 104)" +
                                           "and EndDate between convert(datetime, '" + begin + "', 104) and convert(datetime, '" + end + "', 104))");
            
            //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
            var result = events.Select(e => new JsonEvent()
            {
                value = e.Id.ToString(),
                //TODO change to something with more sense
                text = e.Model.ToString() + " " + e.Id.ToString() + " in Raum " + e.Room.RoomNumber.ToString()
            }).ToList();

            //Creates a JsonResult from the Json [HansesM]
            JsonResult resultJson = new JsonResult { Data = result };
            
            return Json(result);
        }


        //Override on Dispose for security reasons. [HansesM]
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}