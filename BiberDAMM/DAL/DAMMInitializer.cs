using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using BiberDAMM.Models;
using Microsoft.AspNet.Identity;

namespace BiberDAMM.DAL
{
    public class DammInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext _context)
        {
            // create userStore and user userManager for seeding default users [KrabsJ]
            var userStore = new CustomUserStore(_context);
            var userManager = new ApplicationUserManager(userStore);

            // create roleStore and roleManager for seeding default roles [KrabsJ]
            var roleStore = new CustomRoleStore(_context);
            var roleManager = new ApplicationRoleManager(roleStore);

            // create all roles for application and seed them into the database if they not exist [KrabsJ]

            // role Administrator
            var roleAdmin = new CustomRole { Name = "Administrator" };
            CustomRole dbrole = roleManager.FindByName(roleAdmin.Name);
            if(dbrole == null)
            {
                roleManager.Create(roleAdmin);
            }

            // role Pflegekraft
            var roleNurse = new CustomRole { Name = "Pflegekraft" };
            dbrole = roleManager.FindByName(roleNurse.Name);
            if(dbrole == null)
            {
                roleManager.Create(roleNurse);
            }

            // role Arzt
            var roleDoctor = new CustomRole { Name = "Arzt" };
            dbrole = roleManager.FindByName(roleDoctor.Name);
            if (dbrole == null)
            {
                roleManager.Create(roleDoctor);
            }

            // role Reinigungskraft
            var roleCleaner = new CustomRole { Name = "Reinigungskraft" };
            dbrole = roleManager.FindByName(roleCleaner.Name);
            if (dbrole == null)
            {
                roleManager.Create(roleCleaner);
            }

            var user = new ApplicationUser
            {
                UserName = "LustigP",
                Email = "peter@gmx.de",
                Surname = "Peter",
                Lastname = "Lustig",
                UserType = UserType.Administrator,
                Active = true
            };

            // create user and add the role Administator
            userManager.Create(user, "BiberDamm!");
            userManager.AddToRole(user.Id, "Administrator");



            var roomType = new RoomType
            {
                Name="Superraum"

            };

            _context.RoomTypes.Add(roomType);

            _context.SaveChanges();


            var room = new Room
            {
                RoomNumber = "1",
                RoomTypeId = 1

            };

            _context.Rooms.Add(room);

            _context.SaveChanges();

            var bed = new Bed
            {
                Model = "Schlafimax",
                RoomId = 1
            };

            _context.Beds.Add(bed);

            _context.SaveChanges();


            var healthInsurance = new HealthInsurance
            {
                Name = "Verwurstung",
                Type = InsuranceType.gesetzlich
            };

            _context.HealthInsurances.Add(healthInsurance);

            _context.SaveChanges();

            healthInsurance = new HealthInsurance
            {
                Name = "Robin's unschlagbare Turboversicherung",
                Type = InsuranceType.privat
            };

            _context.HealthInsurances.Add(healthInsurance);

            _context.SaveChanges();

            healthInsurance = new HealthInsurance
            {
                Name = "AAA Krankenkasse",
                Type = InsuranceType.privat
            };

            _context.HealthInsurances.Add(healthInsurance);

            _context.SaveChanges();


            var client = new Client
            {
                Surname = "Max",
                Lastname = "Mustermann",
                Sex = Sex.männlich,
                Birthdate = DateTime.Now,
                Captured = DateTime.Now,
                LastUpdated = DateTime.Now,
                RowVersion = 1,
                HealthInsuranceId = 1
            };

            _context.Clients.Add(client);

            _context.SaveChanges();

            var stay = new Stay
            {
               BeginDate=DateTime.Now,
               StayType = StayType.ambulant,
               RowVersion = 1,
               LastUpdated = DateTime.Now,
               ClientId = 1,
               ApplicationUserId = 1
            };

            _context.Stays.Add(stay);

            _context.SaveChanges();



            var blocks = new Blocks
            {
                Date = DateTime.Today,
                StayId = 1,
                BedId = 1
            };

            _context.Blocks.Add(blocks);

            _context.SaveChanges();



            var contactType = new ContactType
            {
                Name = "Verwandtschaft"

            };

            _context.ContactTypes.Add(contactType);

            _context.SaveChanges();



            var contactData = new ContactData
            {
               Description = "Tante Mia",
               ContactTypeId = 1,
               ClientId = 1
            };

            _context.ContactDatas.Add(contactData);

            _context.SaveChanges();

            var treatmentType = new TreatmentType
            {
                Name="Todesbehandlung"
            };

            _context.TreatmentTypes.Add(treatmentType);

            _context.SaveChanges();

            var treatment = new Treatment
            {
                Begin = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                StayId = 1,
                RoomId = 1,
                Description = "Experimentelle Behandlung unter Verwendung von Bibern",
                TreatmentTypeId = 1

            };

            _context.Treatments.Add(treatment);


            _context.SaveChanges();

        }

        //TODO [KrabsJ] check if there has to be a Dispose method
    }
}