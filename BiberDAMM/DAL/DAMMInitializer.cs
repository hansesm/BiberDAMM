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
            var store = new CustomUserStore(_context);
            var manager = new ApplicationUserManager(store);
            var user = new ApplicationUser
            {
                UserName = "LustigP",
                Email = "peter@gmx.de",
                Surname = "Peter",
                Lastname = "Lustig",
                UserType = UserType.Administrator
            };

            manager.Create(user, "BiberDamm!");



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
    }
}