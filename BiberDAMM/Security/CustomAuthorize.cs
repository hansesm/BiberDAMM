using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BiberDAMM.Security
{
    /* this class overrides the standard implementation of the "HandleUnauthorizedRequest" action of the authorize attribute
       it creates the following behavior:
       - the user is not authenticated and tries to access an action with the "CustomAuthorize" attribute --> the user will be redirect to the loginpage
       - the user is authenticated and tries to access an action he is not authorized for (using the attribute [CustomAuthorize(Roles = "specialRole")]) --> the user will be redirect to the accessdenied-page
       [KrabsJ]
    */
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied", action = "Index" }));
            }
        }
    }
}