using System.Web.Optimization;

namespace BiberDAMM
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts used in this Project, all Bundels are loaded in _Layout.cshtml [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-3.1.1.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/app.js",
                "~/Scripts/jquery.dynamicDataTable.js",
                "~/Scripts/Chart.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/fullcalendar.js"
                ));

            //TODO Change Modenizr-Version [HansesM]
            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

       
            //Bootstrap used in this Project, all Bundels are loaded in _Layout.cshtml [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/respond.js")
                );

            //CSS used in this Project, all Bundels are loaded in _Layout.cshtml [HansesM]
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/css/style.css")
                .Include("~/Content/fonts/font-awesome/css/font-awesome.min.css")
                .Include("~/Content/dynamicDataTable.css")
                .Include("~/Content/css/fullcalendar.css")
                );

            //Sets bundle-opzimizations to false (minimalization etc.) [HansesM]
            BundleTable.EnableOptimizations = false;
        }
    }
}