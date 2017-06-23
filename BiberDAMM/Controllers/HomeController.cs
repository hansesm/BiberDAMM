﻿using System.Web.Mvc;
using BiberDAMM.Helpers;
using BiberDAMM.Security;
using BiberDAMM.DAL;
using BiberDAMM.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using BiberDAMM.ViewModels;
using Microsoft.AspNet.Identity;

namespace BiberDAMM.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // load the different homepages based on the usertype of the logged in user [KrabsJ]

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
                    var events = db.Treatments.Where(e => e.BeginDate>=DateTime.Now.AddDays(-7) && e.ApplicationUsers.Any(a => a.Id.ToString().Equals(User.Identity.GetUserId()))).ToList();

                    //var userEvents = from m in db.Clients
                                             //  select m;



                    //List<Treatment> events = new List<Treatment>();

                    //Builds a JSon from the stay-treatments, this is required for the calendar-view[HansesM]
                    var result = events.Select(e => new JsonEventTreatment()
                    {
                        start = e.BeginDate.ToString("s"),
                        end = e.EndDate.ToString("s"),
                        title = e.TreatmentType.Name.ToString(),
                        id = e.Id.ToString()

                    }).ToList();

                    //Creates a JsonResult from the Json [HansesM]
                    JsonResult resultJson = new JsonResult { Data = result };

                    DoctorIndexViewModel viewModel = new DoctorIndexViewModel(resultJson);

                    return View("IndexDoctor", viewModel);
                case ConstVariables.RoleNurse:
                    return View("IndexNursingStaff");
                case ConstVariables.RoleCleaner:

                    //Author: ChristeR
                    //First get relevant treatments
                    var treatments = new List<Treatment>().AsQueryable();


                    treatments = from m in db.Treatments
                                 select m;
                    //To get treatments, which are running and treatments, which are completed today
                    treatments = treatments.Where(s => s.BeginDate.Year <= DateTime.Now.Year && s.BeginDate.Month <= DateTime.Now.Month && s.BeginDate.Day <= DateTime.Now.Day && s.EndDate.Year >= DateTime.Now.Year
                    && s.EndDate.Month >= DateTime.Now.Month && s.EndDate.Day >= DateTime.Now.Day);

                    var rooms = new List<Room>();

                    //Get rooms from found treatments
                    foreach (Treatment actTreat in treatments)
                    {
                        //Get room from database and check, if already contained
                        var room = db.Rooms.Find(actTreat.RoomId);
                        Boolean roomContained = false;

                        foreach (Room actRoom in rooms)
                        {
                            if (actRoom.Id == actTreat.RoomId)
                            {
                                roomContained = true;
                            }
                        }

                        //if room not contained add to list
                        if (!roomContained)
                        {
                            rooms.Add(room);
                        }

                    }
                    return View("IndexCleaner", rooms);

                case ConstVariables.RoleTherapist:
                    return View("IndexTherapist");
                default:
                    return RedirectToAction("Login", "Account");
            }
        }



        public ActionResult About()
        {
            // TODO [KrabsJ] decide what to show on this page
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            // TODO [KrabsJ] decide what to show on this page
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}