using System.Web.Mvc;
using System.Web.Routing;

namespace BiberDAMM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                //section changed: set default action to login
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                new {controller = "Account", action = "Login", id = UrlParameter.Optional}
            );
        }
    }
}