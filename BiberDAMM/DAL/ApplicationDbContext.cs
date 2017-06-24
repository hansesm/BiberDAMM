using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BiberDAMM.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BiberDAMM.DAL
{
    /* Change PrimaryKey of identity package to int //KrabsJ
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    */


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin,
        CustomUserRole, CustomUserClaim>
    {
        //TODO Sinn Klären ? [HansesM]
        //internal object ContactType;

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }


        //Imports the needed Models to the database

        public DbSet<Bed> Beds { get; set; }
        public DbSet<Blocks> Blocks { get; set; }
        public DbSet<Cleaner> Cleaner { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ContactData> ContactDatas { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<HealthInsurance> HealthInsurances { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Stay> Stays { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentType> TreatmentTypes { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        //For security reasons disables on delete cascade to ensure data quality and minimize risk of data loss
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}