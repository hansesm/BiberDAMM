using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiberDAMM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Security.Claims;

namespace BiberDAMM.Helpers
{
    public static class IdentityExtensions
    {
        // custom identity extensions for getting the usertype of the logged in user [KrabsJ]
        public static string GetUserType(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var claimIdentity = identity as ClaimsIdentity;
            if (claimIdentity != null)
            {
                return claimIdentity.FindFirstValue("UserType");
            }
            return null;
        }
    }
}