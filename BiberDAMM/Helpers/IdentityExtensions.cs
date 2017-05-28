using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace BiberDAMM.Helpers
{
    public static class IdentityExtensions
    {
        // custom identity extensions for getting the usertype of the logged in user [KrabsJ]
        public static string GetUserType(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            var claimIdentity = identity as ClaimsIdentity;
            if (claimIdentity != null)
                return claimIdentity.FindFirstValue("UserType");
            return null;
        }

        //TODO Add funtions to get User-Surname and Lastname to Display in _LoginPartial ! [HansesM]
    }
}