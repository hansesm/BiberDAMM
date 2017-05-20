using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using BiberDAMM.Models;
using Microsoft.AspNet.Identity;
using BiberDAMM.Helpers;

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
            var roleAdmin = new CustomRole { Name =  ConstVariables.RoleAdministrator};
            CustomRole dbrole = roleManager.FindByName(roleAdmin.Name);
            if(dbrole == null)
            {
                roleManager.Create(roleAdmin);
            }

            // role Pflegekraft
            var roleNurse = new CustomRole { Name = ConstVariables.RoleNurse };
            dbrole = roleManager.FindByName(roleNurse.Name);
            if(dbrole == null)
            {
                roleManager.Create(roleNurse);
            }

            // role Arzt
            var roleDoctor = new CustomRole { Name = ConstVariables.RoleDoctor };
            dbrole = roleManager.FindByName(roleDoctor.Name);
            if (dbrole == null)
            {
                roleManager.Create(roleDoctor);
            }

            // role Reinigungskraft
            var roleCleaner = new CustomRole { Name = ConstVariables.RoleCleaner };
            dbrole = roleManager.FindByName(roleCleaner.Name);
            if (dbrole == null)
            {
                roleManager.Create(roleCleaner);
            }

            // role Therapeut
            var roleTherapist = new CustomRole { Name = ConstVariables.RoleTherapist };
            dbrole = roleManager.FindByName(roleTherapist.Name);
            if (dbrole == null)
            {
                roleManager.Create(roleTherapist);
            }

            var user1 = new ApplicationUser{UserName = "LustigP", Email = "peter@gmx.de", Surname = "Peter", Lastname = "Lustig", UserType = UserType.Administrator, Active = true, PhoneNumber = "12345670"};

            // create user and add the role Administator
            userManager.Create(user1, "BiberDamm!");
            userManager.AddToRole(user1.Id, ConstVariables.RoleAdministrator);

            // up to this point the data will be stored in the database by default when the aplication is going productive
            // #############################################################################################################
            // the following data is only available during the development phase. It will not be stored in the seed method of the migrations configuration file.
            // Therefore it will not be stored in the productive database by default. [KrabsJ]

            // creating three testusers of type "Arzt"
            var user2 = new ApplicationUser { UserName = "BeckerF", Email = "Franz@gmx.de", Surname = "Franz", Lastname = "Becker", UserType = UserType.Arzt, Active = true, PhoneNumber = "5236984" };
            userManager.Create(user2, "BiberDamm!");
            userManager.AddToRole(user2.Id, ConstVariables.RoleDoctor);

            var user3 = new ApplicationUser { UserName = "MuellerA", Email = "Anna@gmx.de", Surname = "Anna", Lastname = "Mueller", UserType = UserType.Arzt, Active = true, PhoneNumber = "1452398" };
            userManager.Create(user3, "BiberDamm!");
            userManager.AddToRole(user3.Id, ConstVariables.RoleDoctor);

            var user4 = new ApplicationUser { UserName = "MustermannO", Email = "Otto@gmx.de", Surname = "Otto", Lastname = "Mustermann", UserType = UserType.Arzt, Active = false, PhoneNumber = "6354720" };
            userManager.Create(user4, "BiberDamm!");
            userManager.AddToRole(user4.Id, ConstVariables.RoleDoctor);

            //creating three testusers of type "Pflegekraft"
            var user5 = new ApplicationUser { UserName = "ReisM", Email = "Michael@gmx.de", Surname = "Michael", Lastname = "Reis", UserType = UserType.Pflegekraft, Active = true, PhoneNumber = "6985324" };
            userManager.Create(user5, "BiberDamm!");
            userManager.AddToRole(user5.Id, ConstVariables.RoleNurse);

            var user6 = new ApplicationUser { UserName = "FleischerK", Email = "Katharina@gmx.de", Surname = "Katharina", Lastname = "Fleischer", UserType = UserType.Pflegekraft, Active = true, PhoneNumber = "1153369" };
            userManager.Create(user6, "BiberDamm!");
            userManager.AddToRole(user6.Id, ConstVariables.RoleNurse);

            var user7 = new ApplicationUser { UserName = "WeissS", Email = "Sandra@gmx.de", Surname = "Sandra", Lastname = "Weiss", UserType = UserType.Pflegekraft, Active = false, PhoneNumber = "9968534" };
            userManager.Create(user7, "BiberDamm!");
            userManager.AddToRole(user7.Id, ConstVariables.RoleNurse);

            //creating two testuser of type "Reinigungskraft"
            var user8 = new ApplicationUser { UserName = "WolfM", Email = "Marko@gmx.de", Surname = "Marko", Lastname = "Wolf", UserType = UserType.Reinigungskraft, Active = true, PhoneNumber = "6653842" };
            userManager.Create(user8, "BiberDamm!");
            userManager.AddToRole(user8.Id, ConstVariables.RoleCleaner);

            var user9 = new ApplicationUser { UserName = "JaegerT", Email = "Tobias@gmx.de", Surname = "Tobias", Lastname = "Jaeger", UserType = UserType.Reinigungskraft, Active = false, PhoneNumber = "8853364" };
            userManager.Create(user9, "BiberDamm!");
            userManager.AddToRole(user9.Id, ConstVariables.RoleCleaner);

            //creating one testuder of type "Therapeut"
            var user10 = new ApplicationUser { UserName = "FriedrichH", Email = "Hans@gmx.de", Surname = "Hans", Lastname = "Friedrich", UserType = UserType.Therapeut, Active = true, PhoneNumber = "5586694" };
            userManager.Create(user10, "BiberDamm!");
            userManager.AddToRole(user10.Id, ConstVariables.RoleTherapist);

            // creating roomtypes
            var roomTypes = new List<RoomType>
            {
                new RoomType{Name = "Patientenzimmer"},
                new RoomType{Name = "OP-Saal"},
                new RoomType{Name = "Therapiezimmer"},
                new RoomType{Name = "Behandlungszimmer"}
            };
            roomTypes.ForEach(r => _context.RoomTypes.Add(r));
            _context.SaveChanges();

            // creating rooms
            var rooms = new List<Room>
            {
                new Room{RoomNumber="O-1", RoomTypeId=2},
                new Room{RoomNumber="O-2", RoomTypeId=2},
                new Room{RoomNumber="KS-100", RoomTypeId=1},
                new Room{RoomNumber="I-20", RoomTypeId=1},
                new Room{RoomNumber="2OG-12", RoomTypeId=3},
                new Room{RoomNumber="E-10", RoomTypeId=4},
                new Room{RoomNumber="E-11", RoomTypeId=4},
                new Room{RoomNumber="P-32", RoomTypeId=1}
            };
            rooms.ForEach(r => _context.Rooms.Add(r));
            _context.SaveChanges();

            //creating beds
            var beds = new List<Bed>
            {
                new Bed{Model="Kinderbett", RoomId=3},
                new Bed{Model="Kinderbett", RoomId=3},
                new Bed{Model="Intensivbett", RoomId=4},
                new Bed{Model="Erwachsenenbett", RoomId=8}
            };
            beds.ForEach(b => _context.Beds.Add(b));
            _context.SaveChanges();

            //creating healthInsurances
            var healthInsurances = new List<HealthInsurance>
            {
                new HealthInsurance{Name="Viactiv Krankenkasse", Type=InsuranceType.gesetzlich, Number="1"},
                new HealthInsurance{Name="Techniker Krankenkasse", Type=InsuranceType.gesetzlich, Number="2"},
                new HealthInsurance{Name="Axa Krankenversicherung", Type=InsuranceType.privat, Number="3"},
                new HealthInsurance{Name="Provinzial Krankenversicherung", Type=InsuranceType.privat, Number="4"},
            };
            healthInsurances.ForEach(h => _context.HealthInsurances.Add(h));
            _context.SaveChanges();

            //creating clients
            var clients = new List<Client>
            {
                new Client{Surname = "Sebastion", Lastname = "Neudorf", Sex = Sex.männlich, Birthdate = DateTime.Parse("16.02.1942"), Captured = DateTime.Now, LastUpdated = DateTime.Now, RowVersion = 1, HealthInsuranceId = 1, InsuranceNumber=12586, Comment="Nussallergie"},
                new Client{Surname = "Sarah", Lastname = "Shuster", Sex = Sex.weiblich, Birthdate = DateTime.Parse("07.02.1998"), Captured = DateTime.Now, LastUpdated = DateTime.Now, RowVersion = 1, HealthInsuranceId = 2, InsuranceNumber=98746, Comment="Pollenallergie"},
                new Client{Surname = "Eric", Lastname = "Bar", Sex = Sex.männlich, Birthdate = DateTime.Parse("05.06.1970"), Captured = DateTime.Now, LastUpdated = DateTime.Now, RowVersion = 1, HealthInsuranceId = 3, InsuranceNumber=66983},
                new Client{Surname = "Dennis", Lastname = "Bader", Sex = Sex.männlich, Birthdate = DateTime.Parse("14.06.2010"), Captured = DateTime.Now, LastUpdated = DateTime.Now, RowVersion = 1}
            };
            clients.ForEach(c => _context.Clients.Add(c));
            _context.SaveChanges();

            //creating contacttypes
            var contactTypes = new List<ContactType>
            {
                new ContactType{Name="Heimatadresse"},
                new ContactType{Name="Arbeitsadresse"},
                new ContactType{Name="Verwandschaft"},
                new ContactType{Name="Freunde"},
                new ContactType{Name="Erziehungsberechtigte"}
            };
            contactTypes.ForEach(c => _context.ContactTypes.Add(c));
            _context.SaveChanges();

            //creating contactDatas
            var contactDatas = new List<ContactData>
            {
                new ContactData{Email="Neudorf@gmx.de", Phone="1536984", Mobile="3652149", Street="An der Alster 32", Postcode="39241", City="Gommern", ClientId=1, ContactTypeId=1},
                new ContactData{Email="Shuster@gmx.de", Phone="2356478", Mobile="1123588", Street="Fontenay 46", Postcode="95213", City="Münchberg", ClientId=2, ContactTypeId=1},
                new ContactData{Email="Shuster@uni-siegen.de", Phone="3365874", Street="Adolf-Reichwein-Straße 3", Postcode="57072", City="Siegen", ClientId=2, ContactTypeId=2},
                new ContactData{Email="Bar@gmx.de", Phone="6698423", Mobile="9985366", Street="Hallesches Ufer 30", Postcode="66675", City="Losheim", ClientId=3, ContactTypeId=1},
                new ContactData{Description="Alexandra Bar", Phone="6698425", ClientId=3, ContactTypeId=3},
                new ContactData{Description="Axel Neuhaus", Mobile="6687455", ClientId=3, ContactTypeId=4},
                new ContactData{Street="Holstenwall 45", Postcode="06416", City="Könnern", ClientId=4, ContactTypeId=4},
                new ContactData{Description="Herr & Frau Bader", Email="Bader@gmx.de", Phone="1536842", Mobile="5536985", Street="Holstenwall 45", Postcode="06416", City="Könnern", ClientId=4, ContactTypeId=5},
            };
            contactDatas.ForEach(c => _context.ContactDatas.Add(c));
            _context.SaveChanges();

            //creating stays
            var stays = new List<Stay>
            {
                new Stay{BeginDate=DateTime.Parse("01.02.2016"), EndDate=DateTime.Parse("04.02.2016"), ICD10="AB-100", Comment="Keine Komplikationen", Result="entlassen nach erfolgreicher Operation", StayType=StayType.stationär, RowVersion=1, LastUpdated=DateTime.Parse("04.02.2016"), ClientId=1, ApplicationUserId=2},
                new Stay{BeginDate=DateTime.Parse("01.07.2016"), EndDate=DateTime.Parse("8.07.2016"), ICD10="CD-99", Comment="Physiotherapie erforderlich, 2 Sitzungen", Result="Zur weiteren Behandlung überwiesen an Physiotherapeuten Dr. Brand", StayType=StayType.ambulant, RowVersion=1, LastUpdated=DateTime.Parse("08.07.2016"), ClientId=2, ApplicationUserId=3},
                new Stay{BeginDate=DateTime.Today.Date, StayType=StayType.ambulant, RowVersion=1, LastUpdated=DateTime.Now, ClientId=2, ApplicationUserId=4},
                new Stay{BeginDate=DateTime.Today.Date, ICD10="FF-365", StayType=StayType.stationär, RowVersion=1, LastUpdated=DateTime.Now, ClientId=3, ApplicationUserId=2},
                new Stay{BeginDate=DateTime.Today.Date, StayType=StayType.ambulant, RowVersion=1, LastUpdated=DateTime.Now, ClientId=4, ApplicationUserId=3},
            };
            stays.ForEach(s => _context.Stays.Add(s));
            _context.SaveChanges();

            //creating blocks
            var blocks = new List<Blocks>
            {
                new Blocks{Date=DateTime.Parse("01.02.2016"), BedId=4, StayId=1},
                new Blocks{Date=DateTime.Parse("02.02.2016"), BedId=4, StayId=1},
                new Blocks{Date=DateTime.Parse("03.02.2016"), BedId=4, StayId=1},
                new Blocks{Date=DateTime.Parse("04.02.2016"), BedId=4, StayId=1},
                new Blocks{Date=DateTime.Today.Date, BedId=4, StayId=4},
                new Blocks{Date=DateTime.Today.Date.AddDays(1), BedId=4, StayId=4},
                new Blocks{Date=DateTime.Today.Date.AddDays(2), BedId=4, StayId=4},
                new Blocks{Date=DateTime.Today.Date.AddDays(3), BedId=4, StayId=4},
            };
            blocks.ForEach(b => _context.Blocks.Add(b));
            _context.SaveChanges();

            //creating treamentTypes
            var treatmentTypes = new List<TreatmentType>
            {
                new TreatmentType{Name="Untersuchung"},
                new TreatmentType{Name="Operation", RoomTypeId=2},
                new TreatmentType{Name="Therapie", RoomTypeId=3},
                new TreatmentType{Name="Medikation"},
            };
            treatmentTypes.ForEach(t => _context.TreatmentTypes.Add(t));
            _context.SaveChanges();

            // creating treatments
            var treatments = new List<Treatment>
            {
                new Treatment{Begin=DateTime.Parse("01.02.2016 10:00"), End=DateTime.Parse("01.02.2016 11:00"), StayId=1, RoomId=6, Description="Vorsorgeuntersuchung", TreatmentTypeId=1, ApplicationUsers=new List<ApplicationUser>{user2}},
                new Treatment{Begin=DateTime.Parse("02.02.2016 08:00"), End=DateTime.Parse("02.02.2016 10:00"), StayId=1, RoomId=2, Description="Nasenbeinoperation", TreatmentTypeId=2, ApplicationUsers=new List<ApplicationUser>{user2, user5, user6} },
                new Treatment{Begin=DateTime.Parse("01.07.2016 15:00"), End=DateTime.Parse("01.07.2016 16:00"), StayId=2, RoomId=5, Description="Untersuchung der Beschwerden", TreatmentTypeId=1, ApplicationUsers=new List<ApplicationUser>{user3} },
                new Treatment{Begin=DateTime.Parse("03.07.2016 15:00"), End=DateTime.Parse("03.07.2016 16:00"), StayId=2, RoomId=5, Description="Physiotherapie rechtes Handgelenk", TreatmentTypeId=3, ApplicationUsers=new List<ApplicationUser>{user10} },
                new Treatment{Begin=DateTime.Parse("08.07.2016 11:00"), End=DateTime.Parse("08.07.2016 12:00"), StayId=2, RoomId=5, Description="Physiotherapie rechtes Handgelenk", TreatmentTypeId=3, ApplicationUsers=new List<ApplicationUser>{user10} },
                new Treatment{Begin=DateTime.Today.Date.AddHours(13), End=DateTime.Today.Date.AddHours(14), StayId=3, RoomId=6, Description="Aufnahmeuntersuchung", TreatmentTypeId=1, ApplicationUsers=new List<ApplicationUser>{user4} },
                new Treatment{Begin=DateTime.Today.Date.AddHours(10), End=DateTime.Today.Date.AddHours(11), StayId=4, RoomId=7, Description="Aufnahmeuntersuchung", TreatmentTypeId=1, ApplicationUsers=new List<ApplicationUser>{user2} },
                new Treatment{Begin=DateTime.Today.Date.AddDays(1).AddHours(8), End=DateTime.Today.Date.AddDays(1).AddHours(8).AddMinutes(5), StayId=4, RoomId=8, Description="Calciumkanalblocker verabreichen", TreatmentTypeId=4, ApplicationUsers=new List<ApplicationUser>{user7} },
                new Treatment{Begin=DateTime.Today.Date.AddDays(2).AddHours(8), End=DateTime.Today.Date.AddDays(2).AddHours(8).AddMinutes(5), StayId=4, RoomId=8, Description="Calciumkanalblocker verabreichen", TreatmentTypeId=4, ApplicationUsers=new List<ApplicationUser>{user7} },
                new Treatment{Begin=DateTime.Today.Date.AddDays(3).AddHours(8), End=DateTime.Today.Date.AddDays(3).AddHours(8).AddMinutes(5), StayId=4, RoomId=8, Description="Calciumkanalblocker verabreichen", TreatmentTypeId=4, ApplicationUsers=new List<ApplicationUser>{user7} },
                new Treatment{Begin=DateTime.Today.Date.AddHours(8), End=DateTime.Today.Date.AddHours(9), StayId=5, RoomId=7, Description="Aufnahmeuntersuchung", TreatmentTypeId=1, ApplicationUsers=new List<ApplicationUser>{user3} },
            };
            treatments.ForEach(t => _context.Treatments.Add(t));
            _context.SaveChanges();
        }

        //TODO [KrabsJ] check if there has to be a Dispose method
    }
}