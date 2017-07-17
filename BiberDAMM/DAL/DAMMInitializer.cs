using System;
using System.Collections.Generic;
using System.Data.Entity;
using BiberDAMM.Helpers;
using BiberDAMM.Models;
using Microsoft.AspNet.Identity;

namespace BiberDAMM.DAL
{
    public class DammInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
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
            var roleAdmin = new CustomRole { Name = ConstVariables.RoleAdministrator };
            var dbrole = roleManager.FindByName(roleAdmin.Name);
            if (dbrole == null)
                roleManager.Create(roleAdmin);

            // role Pflegekraft
            var roleNurse = new CustomRole { Name = ConstVariables.RoleNurse };
            dbrole = roleManager.FindByName(roleNurse.Name);
            if (dbrole == null)
                roleManager.Create(roleNurse);

            // role Arzt
            var roleDoctor = new CustomRole { Name = ConstVariables.RoleDoctor };
            dbrole = roleManager.FindByName(roleDoctor.Name);
            if (dbrole == null)
                roleManager.Create(roleDoctor);

            // role Reinigungskraft
            var roleCleaner = new CustomRole { Name = ConstVariables.RoleCleaner };
            dbrole = roleManager.FindByName(roleCleaner.Name);
            if (dbrole == null)
                roleManager.Create(roleCleaner);

            // role Therapeut
            var roleTherapist = new CustomRole { Name = ConstVariables.RoleTherapist };
            dbrole = roleManager.FindByName(roleTherapist.Name);
            if (dbrole == null)
                roleManager.Create(roleTherapist);

            var user1 = new ApplicationUser
            {
                UserName = "LustigP",
                Email = "lustig@gmx.de",
                Surname = "Peter",
                Lastname = "Lustig",
                UserType = UserType.Administrator,
                Active = true,
                PhoneNumber = "12345670",
                InitialPassword = true,
            };

            // create user and add the role Administator
            userManager.Create(user1, "BiberDamm123!");
            userManager.AddToRole(user1.Id, ConstVariables.RoleAdministrator);

            // up to this point the data will be stored in the database by default when the aplication is going productive
            // #############################################################################################################
            // the following data is only available during the development phase and the test-version of the application. It will not be stored in the seed method of the migrations configuration file.
            // Therefore it will not be stored in the productive database by default. [KrabsJ]

            // creating three testusers of type "Arzt"
            var user2 = new ApplicationUser
            {
                UserName = "BeckerF",
                Email = "becker@gmx.de",
                Title = "Dr.",
                Surname = "Franz",
                Lastname = "Becker",
                UserType = UserType.Arzt,
                Active = true,
                PhoneNumber = "5236984",
                InitialPassword = true
            };
            userManager.Create(user2, "BiberDamm123!");
            userManager.AddToRole(user2.Id, ConstVariables.RoleDoctor);

            var user3 = new ApplicationUser
            {
                UserName = "MuellerA",
                Email = "mueller@gmx.de",
                Title = "Dr.",
                Surname = "Anna",
                Lastname = "Mueller",
                UserType = UserType.Arzt,
                Active = true,
                PhoneNumber = "1452398",
                InitialPassword = true
            };
            userManager.Create(user3, "BiberDamm123!");
            userManager.AddToRole(user3.Id, ConstVariables.RoleDoctor);

            var user4 = new ApplicationUser
            {
                UserName = "MustermannO",
                Email = "mustermann@gmx.de",
                Title = "Dr.",
                Surname = "Otto",
                Lastname = "Mustermann",
                UserType = UserType.Arzt,
                Active = true,
                PhoneNumber = "6354720",
                InitialPassword = true
            };
            userManager.Create(user4, "BiberDamm123!");
            userManager.AddToRole(user4.Id, ConstVariables.RoleDoctor);

            //creating three testusers of type "Pflegekraft"
            var user5 = new ApplicationUser
            {
                UserName = "ReisM",
                Email = "reis@gmx.de",
                Surname = "Michael",
                Lastname = "Reis",
                UserType = UserType.Pflegekraft,
                Active = true,
                PhoneNumber = "6985324",
                InitialPassword = true
            };
            userManager.Create(user5, "BiberDamm123!");
            userManager.AddToRole(user5.Id, ConstVariables.RoleNurse);

            var user6 = new ApplicationUser
            {
                UserName = "FleischerK",
                Email = "fleischer@gmx.de",
                Surname = "Katharina",
                Lastname = "Fleischer",
                UserType = UserType.Pflegekraft,
                Active = true,
                PhoneNumber = "1153369",
                InitialPassword = true
            };
            userManager.Create(user6, "BiberDamm123!");
            userManager.AddToRole(user6.Id, ConstVariables.RoleNurse);

            var user7 = new ApplicationUser
            {
                UserName = "WeissS",
                Email = "weiss@gmx.de",
                Surname = "Sandra",
                Lastname = "Weiss",
                UserType = UserType.Pflegekraft,
                Active = true,
                PhoneNumber = "9968534",
                InitialPassword = true
            };
            userManager.Create(user7, "BiberDamm123!");
            userManager.AddToRole(user7.Id, ConstVariables.RoleNurse);

            //creating three testuser of type "Reinigungskraft"
            var user8 = new ApplicationUser
            {
                UserName = "WolfM",
                Email = "wolf@gmx.de",
                Surname = "Marko",
                Lastname = "Wolf",
                UserType = UserType.Reinigungskraft,
                Active = true,
                PhoneNumber = "6653842",
                InitialPassword = true
            };
            userManager.Create(user8, "BiberDamm123!");
            userManager.AddToRole(user8.Id, ConstVariables.RoleCleaner);

            var user9 = new ApplicationUser
            {
                UserName = "JaegerT",
                Email = "jaeger@gmx.de",
                Surname = "Tobias",
                Lastname = "Jaeger",
                UserType = UserType.Reinigungskraft,
                Active = true,
                PhoneNumber = "8853364",
                InitialPassword = true
            };
            userManager.Create(user9, "BiberDamm123!");
            userManager.AddToRole(user9.Id, ConstVariables.RoleCleaner);

            var user11 = new ApplicationUser
            {
                UserName = "EichelK",
                Email = "eichel@gmx.de",
                Surname = "Kristin",
                Lastname = "Eichel",
                UserType = UserType.Reinigungskraft,
                Active = false,
                PhoneNumber = "5682354",
                InitialPassword = true
            };
            userManager.Create(user11, "BiberDamm123!");
            userManager.AddToRole(user11.Id, ConstVariables.RoleCleaner);

            //creating three testuser of type "Therapeut"
            var user10 = new ApplicationUser
            {
                UserName = "FriedrichH",
                Email = "friedrich@gmx.de",
                Surname = "Hans",
                Lastname = "Friedrich",
                UserType = UserType.Therapeut,
                Active = true,
                PhoneNumber = "5586694",
                InitialPassword = true
            };
            userManager.Create(user10, "BiberDamm123!");
            userManager.AddToRole(user10.Id, ConstVariables.RoleTherapist);

            var user12 = new ApplicationUser
            {
                UserName = "KrugerM",
                Email = "kruger@gmx.de",
                Surname = "Matthias",
                Lastname = "Kruger",
                UserType = UserType.Therapeut,
                Active = true,
                PhoneNumber = "1522369",
                InitialPassword = true
            };
            userManager.Create(user12, "BiberDamm123!");
            userManager.AddToRole(user12.Id, ConstVariables.RoleTherapist);

            var user13 = new ApplicationUser
            {
                UserName = "MeierM",
                Email = "meier@gmx.de",
                Surname = "Martin",
                Lastname = "Meier",
                UserType = UserType.Therapeut,
                Active = true,
                PhoneNumber = "4556638",
                InitialPassword = true
            };
            userManager.Create(user13, "BiberDamm123!");
            userManager.AddToRole(user13.Id, ConstVariables.RoleTherapist);

            // creating roomtypes
            var roomTypes = new List<RoomType>
            {
                new RoomType {Name = "Patientenzimmer"},
                new RoomType {Name = "OP-Saal"},
                new RoomType {Name = "Therapiezimmer"},
                new RoomType {Name = "Behandlungszimmer"}
            };
            roomTypes.ForEach(r => _context.RoomTypes.Add(r));
            _context.SaveChanges();

            // creating rooms
            var rooms = new List<Room>
            {
                new Room {RoomNumber = "O-1", RoomTypeId = 2, RoomMaxSize = 0},
                new Room {RoomNumber = "O-2", RoomTypeId = 2, RoomMaxSize = 0},
                new Room {RoomNumber = "O-3", RoomTypeId = 2, RoomMaxSize = 0},
                new Room {RoomNumber = "KS-100", RoomTypeId = 1, RoomMaxSize = 3},
                new Room {RoomNumber = "I-20", RoomTypeId = 1, RoomMaxSize = 2},
                new Room {RoomNumber = "P-30", RoomTypeId = 1, RoomMaxSize = 2},
                new Room {RoomNumber = "P-31", RoomTypeId = 1, RoomMaxSize = 2},
                new Room {RoomNumber = "P-32", RoomTypeId = 1, RoomMaxSize = 2},
                new Room {RoomNumber = "2OG-Th1", RoomTypeId = 3, RoomMaxSize = 0},
                new Room {RoomNumber = "2OG-Th2", RoomTypeId = 3, RoomMaxSize = 0},
                new Room {RoomNumber = "2OG-Th3", RoomTypeId = 3, RoomMaxSize = 0},
                new Room {RoomNumber = "E-10", RoomTypeId = 4, RoomMaxSize = 0},
                new Room {RoomNumber = "E-11", RoomTypeId = 4, RoomMaxSize = 0},
                new Room {RoomNumber = "E-12", RoomTypeId = 4, RoomMaxSize = 0}
            };
            rooms.ForEach(r => _context.Rooms.Add(r));
            _context.SaveChanges();

            //creating beds
            var beds = new List<Bed>
            {
                new Bed {BedModels = BedModels.Klinikbett, RoomId = 6},
                new Bed {BedModels = BedModels.Klinikbett, RoomId = 6},
                new Bed {BedModels = BedModels.Klinikbett, RoomId = 7},
                new Bed {BedModels = BedModels.Intensivbett, RoomId = 5},
                new Bed {BedModels = BedModels.Intensivbett, RoomId = 5},
                new Bed {BedModels = BedModels.Säuglingsbett, RoomId = 4},
                new Bed {BedModels = BedModels.Säuglingsbett, RoomId = 4},
                new Bed {BedModels = BedModels.Säuglingsbett, RoomId = 4},
            };
            beds.ForEach(b => _context.Beds.Add(b));
            _context.SaveChanges();

            //creating healthInsurances
            var healthInsurances = new List<HealthInsurance>
            {
                new HealthInsurance {Name = "Viactiv Krankenkasse", Type = InsuranceType.gesetzlich, Number = "104526376"},
                new HealthInsurance {Name = "Techniker Krankenkasse", Type = InsuranceType.gesetzlich, Number = "101575519"},
                new HealthInsurance {Name = "Axa Krankenversicherung", Type = InsuranceType.privat, Number = "168140950"},
                new HealthInsurance {Name = "Provinzial Krankenversicherung", Type = InsuranceType.privat, Number = "168141358"}
            };
            healthInsurances.ForEach(h => _context.HealthInsurances.Add(h));
            _context.SaveChanges();

            //creating clients
            var clients = new List<Client>
            {
                new Client
                {
                    Surname = "Sebastion",
                    Lastname = "Neudorf",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Parse("16.02.1942"),
                    Captured = DateTime.Parse("29.05.1982 10:00:00"),
                    LastUpdated = DateTime.Parse("29.05.1982 10:00:00"),
                    RowVersion = 1,
                    HealthInsuranceId = 1,
                    InsuranceNumber = 1536988,
                    Comment = "Nussallergie"
                },
                new Client
                {
                    Surname = "Sarah",
                    Lastname = "Shuster",
                    Sex = Sex.weiblich,
                    Birthdate = DateTime.Parse("07.02.1998"),
                    Captured = DateTime.Parse("05.06.2007 10:45:00"),
                    LastUpdated = DateTime.Parse("05.06.2007 10:45:00"),
                    RowVersion = 1,
                    HealthInsuranceId = 2,
                    InsuranceNumber = 9874658,
                    Comment = "Pollenallergie"
                },
                new Client
                {
                    Surname = "Eric",
                    Lastname = "Bar",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Parse("05.06.1970"),
                    Captured = DateTime.Parse("21.08.2010 10:00:00"),
                    LastUpdated = DateTime.Parse("21.08.2010 10:00:00"),
                    RowVersion = 1,
                    HealthInsuranceId = 3,
                    InsuranceNumber = 6698366,
                    Comment = "Laktoseintoleranz"
                },
                new Client
                {
                    Surname = "Dennis",
                    Lastname = "Bader",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Parse("14.06.2010"),
                    Captured = DateTime.Parse("14.06.2010 09:00:00"),
                    LastUpdated = DateTime.Parse("14.06.2010 09:00:00"),
                    HealthInsuranceId = 1,
                    InsuranceNumber = 9874647,
                    RowVersion = 1
                },
                new Client
                {
                    Surname = "Jonas",
                    Lastname = "Weier",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Parse("17.11.1990"),
                    Captured = DateTime.Today.AddHours(8).AddMinutes(30),
                    LastUpdated = DateTime.Today.AddHours(8).AddMinutes(30),
                    RowVersion = 1,
                    HealthInsuranceId = 4,
                    InsuranceNumber = 1522476,
                },
                new Client
                {
                    Surname = "Gerda",
                    Lastname = "Pohl",
                    Sex = Sex.weiblich,
                    Birthdate = DateTime.Parse("15.02.1949"),
                    HealthInsuranceId = 4,
                    InsuranceNumber = 2256987,
                    Captured = DateTime.Parse("13.05.1998 10:15:00"),
                    LastUpdated = DateTime.Parse("13.05.1998 10:15:00"),
                    RowVersion = 1
                },
                new Client
                {
                    Surname = "Kevin",
                    Lastname = "Zeis",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Parse("03.01.1976"),
                    HealthInsuranceId = 1,
                    InsuranceNumber = 1236559,
                    Captured = DateTime.Parse("30.04.2000 12:26:00"),
                    LastUpdated = DateTime.Parse("30.04.2000 12:26:00"),
                    RowVersion = 1
                },
                new Client
                {
                    Surname = "Yvonne",
                    Lastname = "Quinn",
                    Sex = Sex.weiblich,
                    Birthdate = DateTime.Parse("24.07.2001"),
                    HealthInsuranceId = 2,
                    InsuranceNumber = 1258645,
                    Captured = DateTime.Today.AddHours(16).AddMinutes(14),
                    LastUpdated = DateTime.Today.AddHours(16).AddMinutes(14),
                    RowVersion = 1
                },
                new Client
                {
                    Surname = "Manuel",
                    Lastname = "Draxel",
                    Sex = Sex.männlich,
                    Birthdate = DateTime.Today,
                    Captured = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    RowVersion = 1
                }
            };
            clients.ForEach(c => _context.Clients.Add(c));
            _context.SaveChanges();

            //creating contacttypes
            var contactTypes = new List<ContactType>
            {
                new ContactType {Name = "Heimatadresse"},
                new ContactType {Name = "Arbeitsadresse"},
                new ContactType {Name = "Verwandschaft"},
                new ContactType {Name = "Freunde"},
                new ContactType {Name = "Erziehungsberechtigte"}
            };
            contactTypes.ForEach(c => _context.ContactTypes.Add(c));
            _context.SaveChanges();


            //creating contactDatas
            var contactDatas = new List<ContactData>
            {
                new ContactData
                {
                    Email = "Neudorf@gmx.de",
                    Phone = "1536984",
                    Mobile = "3652149",
                    Street = "An der Alster 32",
                    Postcode = "39241",
                    City = "Gommern",
                    ClientId = 1,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Email = "Shuster@gmx.de",
                    Phone = "2356478",
                    Mobile = "1123588",
                    Street = "Fontenay 46",
                    Postcode = "95213",
                    City = "Münchberg",
                    ClientId = 2,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Email = "Shuster@uni-siegen.de",
                    Phone = "3365874",
                    Street = "Adolf-Reichwein-Straße 3",
                    Postcode = "57072",
                    City = "Siegen",
                    ClientId = 2,
                    ContactTypeId = 2
                },
                new ContactData
                {
                    Email = "Bar@gmx.de",
                    Phone = "6698423",
                    Mobile = "9985366",
                    Street = "Hallesches Ufer 30",
                    Postcode = "66675",
                    City = "Losheim",
                    ClientId = 3,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Description = "Alexandra Bar",
                    Phone = "6698425",
                    ClientId = 3,
                    ContactTypeId = 3
                },
                new ContactData
                {
                    Description = "Axel Neuhaus",
                    Mobile = "6687455",
                    ClientId = 3,
                    ContactTypeId = 4
                },
                new ContactData
                {
                    Street = "Holstenwall 45",
                    Postcode = "06416",
                    City = "Könnern",
                    ClientId = 4,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Description = "Herr & Frau Bader",
                    Email = "Bader@gmx.de",
                    Phone = "1536842",
                    Mobile = "5536985",
                    Street = "Holstenwall 45",
                    Postcode = "06416",
                    City = "Könnern",
                    ClientId = 4,
                    ContactTypeId = 5
                },
                new ContactData
                {
                    Phone = "1536687",
                    Street = "Leipziger Strasse 60",
                    Postcode = "54292",
                    City = "Trier Eitelsbach",
                    ClientId = 6,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Description = "Thomas Pohl",
                    Mobile = "1552368",
                    ClientId = 6,
                    ContactTypeId = 3
                },
                new ContactData
                {
                    Email = "weier@gmx.de",
                    Phone = "1544288",
                    Mobile = "2336545",
                    Street = "Güntzelstrasse 91",
                    Postcode = "54518",
                    City = "Gladbach",
                    ClientId = 5,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Description = "Lukas Apfel",
                    Mobile = "1522364",
                    ClientId = 5,
                    ContactTypeId = 4
                },
                new ContactData
                {
                    Email = "zeis@gmx.de",
                    Phone = "1523685",
                    Mobile = "1225336",
                    Street = "Reeperbahn 32",
                    Postcode = "93348",
                    City = "Kirchdorf",
                    ClientId = 7,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Email = "yquinn@gmx.de",
                    Mobile = "1235568",
                    Street = "Pappelallee 39",
                    Postcode = "36463",
                    City = "Dermbach",
                    ClientId = 8,
                    ContactTypeId = 1
                },
                new ContactData
                {
                    Description = "Herr & Frau Quinn",
                    Phone = "1522566",
                    Street = "Pappelallee 39",
                    Postcode = "36463",
                    City = "Dermbach",
                    ClientId = 8,
                    ContactTypeId = 5
                },

            };
            contactDatas.ForEach(c => _context.ContactDatas.Add(c));
            _context.SaveChanges();

            //creating treamentTypes
            var treatmentTypes = new List<TreatmentType>
            {
                new TreatmentType {Name = "Untersuchung"},
                new TreatmentType {Name = "Operation", RoomTypeId = 2},
                new TreatmentType {Name = "Therapie", RoomTypeId = 3},
                new TreatmentType {Name = "Medikation"},
                new TreatmentType {Name = "Blutspende"}
            };
            treatmentTypes.ForEach(t => _context.TreatmentTypes.Add(t));
            _context.SaveChanges();

            //creating stays
            var stays = new List<Stay>
            {
                new Stay
                {
                    BeginDate = DateTime.Parse("14.06.2010 09:00:00"),
                    EndDate = DateTime.Parse("19.06.2010 10:00:00"),
                    ICD10 = "G-100",
                    Comment = null,
                    Result = "Geburt verlief Reibungslos; Entlassung nach Hause nach 5 Tagen",
                    StayType = StayType.stationär,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("19.06.2010 10:00:00"),
                    ClientId = 4,
                    ApplicationUserId = 2
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddHours(8),
                    EndDate = null,
                    ICD10 = null,
                    Comment = null,
                    Result = null,
                    StayType = StayType.ambulant,
                    RowVersion = 0,
                    LastUpdated = DateTime.Today.AddHours(8),
                    ClientId = 4,
                    ApplicationUserId = 3
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("21.08.2010 10:00:00"),
                    EndDate = DateTime.Parse("21.08.2010 13:00:00"),
                    ICD10 = "BL-210",
                    Comment = null,
                    Result = "Blutspende verlief ohne Komplikationen; Patient entlassen",
                    StayType = StayType.ambulant,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("21.08.2010 13:00:00"),
                    ClientId = 3,
                    ApplicationUserId = 4
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddDays(-1).AddHours(9),
                    EndDate = null,
                    ICD10 = "NB-333",
                    Comment = null,
                    Result = null,
                    StayType = StayType.stationär,
                    RowVersion = 0,
                    LastUpdated = DateTime.Today.AddDays(-1).AddHours(9),
                    ClientId = 3,
                    ApplicationUserId = 2
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("13.05.1998 10:15:00"),
                    EndDate = DateTime.Parse("13.05.1998 13:15:00"),
                    ICD10 = "BL-210",
                    Comment = "leichte Kreislaufprobleme nach Blutspende",
                    Result = "nach Blutspende entlassen",
                    StayType = StayType.ambulant,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("13.05.1998 13:15:00"),
                    ClientId = 6,
                    ApplicationUserId = 4
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddHours(8).AddMinutes(30),
                    EndDate = null,
                    ICD10 = "B-345",
                    Comment = "Handgelenksbruch im Marienhospital operiert; Zur Nachbehandlung an uns überwiesen",
                    Result = null,
                    StayType = StayType.ambulant,
                    RowVersion = 0,
                    LastUpdated = DateTime.Today.AddHours(8).AddMinutes(30),
                    ClientId = 5,
                    ApplicationUserId = 3
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("30.04.2000 12:26:00"),
                    EndDate = DateTime.Parse("30.04.2000 15:33:00"),
                    ICD10 = "MD-238",
                    Comment = "Patient kommt mit Verdacht auf Blinddarmentzündung",
                    Result = "Blinddarmtests alle negativ; Patient mit Magendarm nach Hause entlassen",
                    StayType = StayType.ambulant,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("30.04.2000 15:33:00"),
                    ClientId = 7,
                    ApplicationUserId = 2
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddHours(8).AddMinutes(45),
                    EndDate = null,
                    ICD10 = "HE-518",
                    Comment = null,
                    Result = null,
                    StayType = StayType.stationär,
                    RowVersion = 1,
                    LastUpdated = DateTime.Today.AddHours(8).AddMinutes(45),
                    ClientId = 7,
                    ApplicationUserId = 4
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("05.06.2007 10:45:29"),
                    EndDate = DateTime.Parse("05.06.2007 14:28:00"),
                    ICD10 = "A-771",
                    Comment = "starke allergische Reaktion auf Birkenpollen",
                    Result = "Allergiemittel verschrieben und entlassen",
                    StayType = StayType.ambulant,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("05.06.2007 14:28:00"),
                    ClientId = 2,
                    ApplicationUserId = 2
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("29.05.1982 10:00:00"),
                    EndDate = DateTime.Parse("29.05.1982 12:00:00"),
                    ICD10 = "BL-2010",
                    Comment = null,
                    Result = "Blutspende verlief ohne Komplikationen; Patient entlassen",
                    StayType = StayType.ambulant,
                    RowVersion = 1,
                    LastUpdated = DateTime.Parse("29.05.1982 12:00:00"),
                    ClientId = 1,
                    ApplicationUserId = 4
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddHours(11),
                    EndDate = null,
                    ICD10 = "H-201",
                    Comment = null,
                    Result = null,
                    StayType = StayType.stationär,
                    RowVersion = 1,
                    LastUpdated = DateTime.Today.AddHours(11),
                    ClientId = 1,
                    ApplicationUserId = 3
                },
                new Stay
                {
                    BeginDate = DateTime.Today.AddHours(16).AddMinutes(14),
                    EndDate = null,
                    ICD10 = "GS-776",
                    Comment = "Entfernung von Gallensteinen und Nachversorgung",
                    Result = null,
                    StayType = StayType.stationär,
                    RowVersion = 1,
                    LastUpdated = DateTime.Today.AddHours(16).AddMinutes(14),
                    ClientId = 8,
                    ApplicationUserId = 3
                },
                new Stay
                {
                    BeginDate = DateTime.Parse("18.09.2013 16:26:49"),
                    EndDate = DateTime.Parse("18.09.2013 17:30:00"),
                    ICD10 = "BH-229",
                    Comment = "Bruch muss von Spezialisten behandelt werden",
                    Result = "Patient wurde mit kompliziertem Knochenbruch in der rechten Hand an die Handchirurgie Dr. Pahl überwiesen",
                    StayType = StayType.ambulant,
                    RowVersion = 3,
                    LastUpdated = DateTime.Parse("18.09.2013 17:30:00"),
                    ClientId = 3,
                    ApplicationUserId = 3
                },


            };
            stays.ForEach(s => _context.Stays.Add(s));
            _context.SaveChanges();

            //creating blocks
            var blocks = new List<Blocks>
            {
                new Blocks
                {
                    BeginDate = DateTime.Parse("14.06.2010 00:00:00"),
                    EndDate = DateTime.Parse("19.06.2010 00:00:00"),
                    StayId = 1,
                    BedId = 6,
                    ClientRoomType = ClientRoomType.Mehrbettzimmer
                },
                new Blocks
                {
                    BeginDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddDays(1),
                    StayId = 4,
                    BedId = 3,
                    ClientRoomType = ClientRoomType.Einzelzimmer
                },
                new Blocks
                {
                    BeginDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(5),
                    StayId = 8,
                    BedId = 4,
                    ClientRoomType = ClientRoomType.Doppelzimmer
                },
                new Blocks
                {
                    BeginDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(7),
                    StayId = 11,
                    BedId = 1,
                    ClientRoomType = ClientRoomType.Doppelzimmer
                },
                new Blocks
                {
                    BeginDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(5),
                    StayId = 12,
                    BedId = 3,
                    ClientRoomType = ClientRoomType.Einzelzimmer
                },
            };
            blocks.ForEach(b => _context.Blocks.Add(b));
            _context.SaveChanges();

            // creating treatments
            var treatments = new List<Treatment>
            {
                new Treatment
                {
                    BeginDate = DateTime.Parse("15.06.2010 10:00:00"),
                    EndDate = DateTime.Parse("15.06.2010 11:00:00"),
                    StayId = 1,
                    RoomId = 12,
                    Description = "Routineuntersuchung von Neugeborenen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddHours(11),
                    EndDate = DateTime.Today.AddHours(11).AddMinutes(30),
                    StayId = 2,
                    RoomId = 13,
                    Description = "Aufnahmeuntersuchung; Verdacht auf Bänderriss",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3},
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("21.08.2010 11:30:00"),
                    EndDate = DateTime.Parse("21.08.2010 12:15:00"),
                    StayId = 3,
                    RoomId = 14,
                    Description = "Standardblutspende 0,5l",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 5,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user6},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(-1).AddHours(14),
                    EndDate = DateTime.Today.AddDays(-1).AddHours(15),
                    StayId = 4,
                    RoomId = 12,
                    Description = "OP-Aufklärung und Voruntersuchung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddHours(12),
                    EndDate = DateTime.Today.AddHours(13),
                    StayId = 4,
                    RoomId = 1,
                    Description = "Nasenbeinoperation",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 2,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2, user5},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(1).AddHours(9),
                    EndDate = DateTime.Today.AddDays(1).AddHours(9).AddMinutes(30),
                    StayId = 4,
                    RoomId = 13,
                    Description = "OP-Nachuntersuchung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2},
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("13.05.1998 11:15:00"),
                    EndDate = DateTime.Parse("13.05.1998 11:45:00"),
                    StayId = 5,
                    RoomId = 14,
                    Description = "Standardblutspende 0,5l",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 5,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user7},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(4).AddHours(14),
                    EndDate = DateTime.Today.AddDays(4).AddHours(15),
                    StayId = 6,
                    RoomId = 9,
                    Description = "Physiotherapie nach Handgelenksbruch und Operation",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 8,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(11).AddHours(14),
                    EndDate = DateTime.Today.AddDays(11).AddHours(15),
                    StayId = 6,
                    RoomId = 9,
                    Description = "Physiotherapie nach Handgelenksbruch und Operation",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 8,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(18).AddHours(14),
                    EndDate = DateTime.Today.AddDays(18).AddHours(15),
                    StayId = 6,
                    RoomId = 9,
                    Description = "Physiotherapie nach Handgelenksbruch und Operation",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 8,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(25).AddHours(14),
                    EndDate = DateTime.Today.AddDays(25).AddHours(15),
                    StayId = 6,
                    RoomId = 9,
                    Description = "Physiotherapie nach Handgelenksbruch und Operation",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 8,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("30.04.2000 13:00:00"),
                    EndDate = DateTime.Parse("30.04.2000 14:00:00"),
                    StayId = 7,
                    RoomId = 14,
                    Description = "Blinddarm untersuchen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddHours(11),
                    EndDate = DateTime.Today.AddHours(12).AddMinutes(30),
                    StayId = 8,
                    RoomId = 12,
                    Description = "Aufnahmeuntersuchung; Patient beschreibt Symptome einer Herzmuskelentzündung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user4},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(1).AddHours(8),
                    EndDate = DateTime.Today.AddDays(1).AddHours(8).AddMinutes(10),
                    StayId = 8,
                    RoomId = 5,
                    Description = "Herzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 14,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(2).AddHours(8),
                    EndDate = DateTime.Today.AddDays(2).AddHours(8).AddMinutes(10),
                    StayId = 8,
                    RoomId = 5,
                    Description = "Herzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 14,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(3).AddHours(8),
                    EndDate = DateTime.Today.AddDays(3).AddHours(8).AddMinutes(10),
                    StayId = 8,
                    RoomId = 5,
                    Description = "Herzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 14,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(4).AddHours(8),
                    EndDate = DateTime.Today.AddDays(4).AddHours(8).AddMinutes(10),
                    StayId = 8,
                    RoomId = 5,
                    Description = "Herzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 14,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(5).AddHours(8),
                    EndDate = DateTime.Today.AddDays(5).AddHours(8).AddMinutes(10),
                    StayId = 8,
                    RoomId = 5,
                    Description = "Herzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 14,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("05.06.2007 11:30:00"),
                    EndDate = DateTime.Parse("05.06.2007 12:15:00"),
                    StayId = 9,
                    RoomId = 12,
                    Description = "Aufnahmeuntersuchung; Patient zeigt starke Anzeichen einer allergischen Reaktion",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2},
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("29.05.1982 10:30:00"),
                    EndDate = DateTime.Parse("29.05.1982 11:15:00"),
                    StayId = 10,
                    RoomId = 14,
                    Description = "Standardblutspende 0,5l",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 5,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user5},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddHours(16),
                    EndDate = DateTime.Today.AddHours(16).AddMinutes(30),
                    StayId = 11,
                    RoomId = 12,
                    Description = "Aufklärung Operation und Voruntersuchung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(1).AddHours(11),
                    EndDate = DateTime.Today.AddDays(1).AddHours(12).AddMinutes(30),
                    StayId = 11,
                    RoomId = 2,
                    Description = "Hüftimplantat einsetzen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 2,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user2, user3, user5, user7},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(2).AddHours(8),
                    EndDate = DateTime.Today.AddDays(2).AddHours(8).AddMinutes(10),
                    StayId = 11,
                    RoomId = 6,
                    Description = "Schmerzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 23,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(3).AddHours(8),
                    EndDate = DateTime.Today.AddDays(3).AddHours(8).AddMinutes(10),
                    StayId = 11,
                    RoomId = 6,
                    Description = "Schmerzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 23,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(4).AddHours(8),
                    EndDate = DateTime.Today.AddDays(4).AddHours(8).AddMinutes(10),
                    StayId = 11,
                    RoomId = 6,
                    Description = "Schmerzmittel verabreichen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 4,
                    IdOfSeries = 23,
                    ApplicationUsers = null,
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddHours(16).AddMinutes(30),
                    EndDate = DateTime.Today.AddHours(17),
                    StayId = 12,
                    RoomId = 12,
                    Description = "OP-Vorbesprechung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(1).AddHours(9),
                    EndDate = DateTime.Today.AddDays(1).AddHours(10),
                    StayId = 12,
                    RoomId = 3,
                    Description = "Gallen-OP: Gallensteine entfernen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 2,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3, user5},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(2).AddHours(9),
                    EndDate = DateTime.Today.AddDays(2).AddHours(9).AddMinutes(30),
                    StayId = 12,
                    RoomId = 7,
                    Description = "Nachbesprechung und Untersuchung",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(3).AddHours(13),
                    EndDate = DateTime.Today.AddDays(3).AddHours(13).AddMinutes(30),
                    StayId = 12,
                    RoomId = 10,
                    Description = "Massagetherapie",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 29,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Today.AddDays(4).AddHours(13),
                    EndDate = DateTime.Today.AddDays(4).AddHours(13).AddMinutes(30),
                    StayId = 12,
                    RoomId = 10,
                    Description = "Massagetherapie",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 3,
                    IdOfSeries = 29,
                    ApplicationUsers = new List<ApplicationUser> {user10},
                },
                new Treatment
                {
                    BeginDate = DateTime.Parse("18.09.2013 16:45:00"),
                    EndDate = DateTime.Parse("18.09.2013 17:15:00"),
                    StayId = 13,
                    RoomId = 13,
                    Description = "Bruch in rechter Hand untersuchen",
                    UpdateTimeStamp = DateTime.Now,
                    TreatmentTypeId = 1,
                    IdOfSeries = null,
                    ApplicationUsers = new List<ApplicationUser> {user3},
                },

            };
            treatments.ForEach(t => _context.Treatments.Add(t));
            _context.SaveChanges();

            // creating cleaning appointments
            var cleaningAppointments = new List<Cleaner>
            {
                new Cleaner
                {
                    RoomId = 13,
                    BeginDate = DateTime.Today.AddHours(11).AddMinutes(30),
                    EndDate = DateTime.Today.AddHours(11).AddMinutes(40),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 2
                },
                new Cleaner
                {
                    RoomId = 14,
                    BeginDate = DateTime.Parse("21.08.2010 12:15:00"),
                    EndDate = DateTime.Parse("21.08.2010 12:25:00"),
                    CleaningDone = true,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 3
                },
                new Cleaner
                {
                    RoomId = 1,
                    BeginDate = DateTime.Today.AddHours(13),
                    EndDate = DateTime.Today.AddHours(13).AddMinutes(20),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.twentyMinutes,
                    TreatmentId = 5
                },
                new Cleaner
                {
                    RoomId = 14,
                    BeginDate = DateTime.Parse("13.05.1998 11:45:00"),
                    EndDate = DateTime.Parse("13.05.1998 11:55:00"),
                    CleaningDone = true,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 7
                },
                new Cleaner
                {
                    RoomId = 9,
                    BeginDate = DateTime.Today.AddDays(4).AddHours(15),
                    EndDate = DateTime.Today.AddDays(4).AddHours(15).AddMinutes(10),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 8
                },
                new Cleaner
                {
                    RoomId = 9,
                    BeginDate = DateTime.Today.AddDays(11).AddHours(15),
                    EndDate = DateTime.Today.AddDays(11).AddHours(15).AddMinutes(10),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 9
                },
                new Cleaner
                {
                    RoomId = 9,
                    BeginDate = DateTime.Today.AddDays(18).AddHours(15),
                    EndDate = DateTime.Today.AddDays(18).AddHours(15).AddMinutes(10),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 10
                },
                new Cleaner
                {
                    RoomId = 9,
                    BeginDate = DateTime.Today.AddDays(25).AddHours(15),
                    EndDate = DateTime.Today.AddDays(25).AddHours(15).AddMinutes(10),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 11
                },
                new Cleaner
                {
                    RoomId = 14,
                    BeginDate = DateTime.Parse("30.04.2000 14:00:00"),
                    EndDate = DateTime.Parse("30.04.2000 14:10:00"),
                    CleaningDone = true,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 12
                },
                new Cleaner
                {
                    RoomId = 12,
                    BeginDate = DateTime.Today.AddHours(12).AddMinutes(30),
                    EndDate = DateTime.Today.AddHours(12).AddMinutes(40),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 13
                },
                new Cleaner
                {
                    RoomId = 14,
                    BeginDate = DateTime.Parse("29.05.1982 11:15:00"),
                    EndDate = DateTime.Parse("29.05.1982 11:25:00"),
                    CleaningDone = true,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 20
                },
                new Cleaner
                {
                    RoomId = 2,
                    BeginDate = DateTime.Today.AddDays(1).AddHours(12).AddMinutes(30),
                    EndDate = DateTime.Today.AddDays(1).AddHours(13),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.thirtyMinutes,
                    TreatmentId = 22
                },
                new Cleaner
                {
                    RoomId = 3,
                    BeginDate = DateTime.Today.AddDays(1).AddHours(10),
                    EndDate = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(20),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.twentyMinutes,
                    TreatmentId = 27
                },
                new Cleaner
                {
                    RoomId = 10,
                    BeginDate = DateTime.Today.AddDays(3).AddHours(13).AddMinutes(30),
                    EndDate = DateTime.Today.AddDays(3).AddHours(13).AddMinutes(40),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 29
                },
                new Cleaner
                {
                    RoomId = 10,
                    BeginDate = DateTime.Today.AddDays(4).AddHours(13).AddMinutes(30),
                    EndDate = DateTime.Today.AddDays(4).AddHours(13).AddMinutes(40),
                    CleaningDone = false,
                    CleaningDuration = CleaningDuration.tenMinutes,
                    TreatmentId = 30
                },
            };
            cleaningAppointments.ForEach(c => _context.Cleaner.Add(c));
            _context.SaveChanges();
        }
    }
}