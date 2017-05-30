using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using BiberDAMM.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace BiberDAMM.Models
{
    public enum UserType
    {
        Reinigungskraft,
        Therapeut,
        Pflegekraft,
        Arzt,
        Administrator
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    // Change PrimaryKey of identity package to int [public class ApplicationUser : IdentityUser] //KrabsJ
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Vorname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Benutzertyp")]
        public UserType UserType { get; set; }

        [Display(Name = "Aktiviert")]
        public bool Active { get; set; }

        public bool InitialPassword { get; set; }

        public virtual ICollection<Stay> Stays { get; set; }

        public virtual ICollection<Treatment> Treatments { get; set; }

        //Change PrimaryKey of identity package to int [public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)] //KrabsJ
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            // custom claim for getting the usertype of the logged in user [KrabsJ]
            userIdentity.AddClaim(new Claim("UserType", UserType.ToString()));

            // custom claim for getting the displayName (shown in _LoginPartial) of the logged in user [KrabsJ]
            // the displayName should consist of surname and lastname if the title of the user is null or empty
            // if the user has got a title the displayName should consist of title and lastname
            string displayName;
            if (String.IsNullOrEmpty(Title))
            {
                displayName = Surname + " " + Lastname;
            }
            else
            {
                displayName = Title + " " + Lastname;
            }
            userIdentity.AddClaim(new Claim("DisplayName", displayName));

            // custom claim for getting the value of the initialPassword flag of the logged in user [KrabsJ]
            userIdentity.AddClaim(new Claim("InitialPassword", InitialPassword.ToString()));

            return userIdentity;
        }
    }

    //Change PrimaryKey of identity package to int [new custom classes of identity package are needed here] //KrabsJ
    public class CustomUserRole : IdentityUserRole<int>
    {
    }

    public class CustomUserClaim : IdentityUserClaim<int>
    {
    }

    public class CustomUserLogin : IdentityUserLogin<int>
    {
    }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole()
        {
        }

        public CustomRole(string name)
        {
            Name = name;
        }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole,
        CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context) : base(context)
        {
        }
    }
}