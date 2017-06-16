﻿using System.Web.Optimization;

namespace BiberDAMM
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts used in this Project, all Bundels are loaded in _Layout.cshtml [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery-3.1.1.js",
                "~/Scripts/app.js",
                "~/Scripts/jquery.dynamicDataTable.js",
                "~/Scripts/Chart.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/fullcalendar.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/timetable.js"
                ));

            //JQuery UI Bundle [HansesM]
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery-ui-timepicker-addon.js"
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
                .Include("~/Content/css/style.css",
                "~/Content/fonts/font-awesome/css/font-awesome.min.css",
                "~/Content/dynamicDataTable.css",
                "~/Content/css/fullcalendar.css",
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.theme.css",
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/css/jquery-ui-timepicker-addon.css",
                "~/Content/css/timetablejs.css"
                ));

            //Sets bundle-opzimizations to false (minimalization etc.) [HansesM]
            BundleTable.EnableOptimizations = false;
        }
    }
}