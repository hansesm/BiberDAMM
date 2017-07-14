using System.Web.Mvc;
using BiberDAMM.Helpers;
using BiberDAMM.Security;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using BiberDAMM.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace BiberDAMM.Controllers
{
    // load the different homepages based on the usertype of the logged in user [KrabsJ]
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //Author: ChristesR
        public ActionResult Index()
        {

            // show warning alert statement if the initialPassword flag is true
            if (User.Identity.GetInitialPassword() == true.ToString())
            {
                TempData["InitialPasswordTrue"] = " Sie verwenden ein Initialpasswort. Bitte vergeben Sie hier ein eigenes Passwort:";
            }

            switch (User.Identity.GetUserType())
            {
                case ConstVariables.RoleAdministrator:
                    return View("IndexAdmin");
                case ConstVariables.RoleDoctor:

                    //Gets all treatments not older than 7 Days [ChristesR]
                    DateTime daysConstantDoctor = DateTime.Now.AddDays(-7);
                    string loggedInUserIDDoctor = User.Identity.GetUserId();
                    var eventsDoctor = db.Treatments.Where(e => e.BeginDate >= daysConstantDoctor && e.ApplicationUsers.Any
                    (a => a.Id.ToString().Equals(loggedInUserIDDoctor))).ToList();

                    //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
                    var resultDoctor = eventsDoctor.Select(e => new JsonEventTreatment()
                    {
                        start = e.BeginDate.ToString("s"),
                        end = e.EndDate.ToString("s"),
                        title = e.TreatmentType.Name.ToString(),
                        id = e.Id.ToString()

                    }).ToList();

                    //Creates a JsonResult from the Json [HansesM]
                    JsonResult resultJson = new JsonResult { Data = resultDoctor };

                    DoctorIndexViewModel viewModelDoctor = new DoctorIndexViewModel(resultJson);

                    return View("IndexDoctor", viewModelDoctor);
                case ConstVariables.RoleNurse:

                    //Gets all treatments not older than 7 Days [ChristesR]
                    DateTime daysConstantNurse = DateTime.Now.AddDays(-7);
                    string loggedInUserIDNurse = User.Identity.GetUserId();
                    var eventsNurse = db.Treatments.Where(e => e.BeginDate >= daysConstantNurse && e.ApplicationUsers.Any
                    (a => a.Id.ToString().Equals(loggedInUserIDNurse))).ToList();

                    //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
                    var resultNurse = eventsNurse.Select(e => new JsonEventTreatment()
                    {
                        start = e.BeginDate.ToString("s"),
                        end = e.EndDate.ToString("s"),
                        title = e.TreatmentType.Name.ToString(),
                        id = e.Id.ToString()

                    }).ToList();

                    //Creates a JsonResult from the Json [HansesM]
                    JsonResult resultJsonNurse = new JsonResult { Data = resultNurse };

                    NurseIndexViewModel viewModelNurse = new NurseIndexViewModel(resultJsonNurse);
                    return View("IndexNursingStaff", viewModelNurse);

                case ConstVariables.RoleCleaner:
                //-- The following implements the cleaning schedule [Jean-PierreK] --/
                
                    // Return a list of all occupied patient rooms for the day: 
                    var PatientRooms = db.Rooms.SqlQuery ("select * from rooms r where r.id in " +
                                                    "(select be.RoomId from beds be where be.Id in " +
                                                    "(select b.BedId from blocks b where " +
                                                    "convert(datetime, GetDate() , 104) between b.BeginDate " +
                                                    "and b.EndDate ))");
                
                    // Gather the to do list of scheduled cleaning jobs for cleaning personnel:
                    var RoomsToClean = db.Cleaner.Where(e => e.CleaningDone == false).ToList();

                    ViewBag.OccupiedRooms = PatientRooms;
                    ViewBag.CleaningEvents = RoomsToClean;

                    return View("IndexCleaner");

                case ConstVariables.RoleTherapist:

                    //Gets all treatments not older than 7 Days [ChristesR]
                    DateTime daysConstantTherapist = DateTime.Now.AddDays(-7);
                    string loggedInUserIDTherapist = User.Identity.GetUserId();
                    var eventsTherapist = db.Treatments.Where(e => e.BeginDate >= daysConstantTherapist && e.ApplicationUsers.Any
                    (a => a.Id.ToString().Equals(loggedInUserIDTherapist))).ToList();

                    //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
                    var resultTherapist = eventsTherapist.Select(e => new JsonEventTreatment()
                    {
                        start = e.BeginDate.ToString("s"),
                        end = e.EndDate.ToString("s"),
                        title = e.TreatmentType.Name.ToString(),
                        id = e.Id.ToString()

                    }).ToList();

                    //Creates a JsonResult from the Json [HansesM]
                    JsonResult resultJsonTherapist = new JsonResult { Data = resultTherapist };

                    TherapistIndexViewModel viewModelTherapist = new TherapistIndexViewModel(resultJsonTherapist);
                    return View("IndexTherapist", viewModelTherapist);
                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        //-- Post function to mark a cleaning job as done [Jean-PierreK] --// 
        [HttpPost]
        [ActionName("Check")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckConfirmed(int id)
        {
            var cleaned = db.Cleaner.Find(id);
            cleaned.CleaningDone = true;
            db.Entry(cleaned).State = EntityState.Modified;
            db.SaveChanges();
            var PatientRooms = db.Rooms.SqlQuery("select * from rooms r where r.id in " +
                               "(select be.RoomId from beds be where be.Id in " +
                               "(select b.BedId from blocks b where " +
                               "convert(datetime, GetDate() , 104) between b.BeginDate " +
                               "and b.EndDate ))");
            var RoomsToClean = db.Cleaner.Where(e => e.CleaningDone == false).ToList();
            ViewBag.OccupiedRooms = PatientRooms;
            ViewBag.CleaningEvents = RoomsToClean;
            // Return notification if cleaning done
            TempData["CleaningDone"] = " Reinigung erfolgt und Raum zur Verwendung freigegeben";
            return View("IndexCleaner");
        }
    }
}