using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                UserName = "peter@gmx.de",
                Email = "peter@gmx.de"
            };

            manager.Create(user, "BiberDamm!");
        }
    }
}