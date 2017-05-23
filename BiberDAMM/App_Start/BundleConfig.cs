using System.Web.Optimization;

namespace BiberDAMM
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js", "~/Scripts/jquery.validate*", "~/Content/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/app.js",
                "~/Scripts/jquery.dynamicDataTable.js"
                ));

            //TODO Change Modenizr-Version [HansesM]
            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            //TODO Add Java-Script-Classes to Bundle to reducre HTTP Connections [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/respond.js")
                );

            //TODO Add Css-Stylesheets to Bundle to reducre HTTP Connections [HansesM]
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/css/style.css")
                .Include("~/Content/fonts/font-awesome/css/font-awesome.min.css")
                .Include("~/Content/dynamicDataTable.css")
                );

            BundleTable.EnableOptimizations = false;
        }
    }
}