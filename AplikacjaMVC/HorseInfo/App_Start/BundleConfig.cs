using System.Web;
using System.Web.Optimization;

namespace HorseInfo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/jquery.mobile-{version}.js",
                        "~/Scripts/jquery.mobile-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery.mobile-{version}.css",
                     "~/Content/jquery.mobile-{version}.min.css",
                      "~/Content/jQueryTheme/jquery-ui.css",
                      "~/Content/jQueryTheme/jquery-ui.min.css",
                      "~/Content/jQueryTheme/jquery-ui.structure.css",
                      "~/Content/jQueryTheme/jquery-ui.structure.min.css",
                      "~/Content/jQueryTheme/jquery-ui.theme.css",
                      "~/Content/jQueryTheme/jquery-ui.theme.min.css",
                      "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
