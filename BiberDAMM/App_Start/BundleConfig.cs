using System.Web.Optimization;

namespace BiberDAMM
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            //TODO Change Modenizr-Version [HansesM]
            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            //TODO Add Java-Script-Classes to Bundle to reducre HTTP Connections [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            //TODO Add Css-Stylesheets to Bundle to reducre HTTP Connections [HansesM]
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/style.css",
                "~/Content/fonts/font-awesome/css/font-awesome.min.css"));
        }
    }
}