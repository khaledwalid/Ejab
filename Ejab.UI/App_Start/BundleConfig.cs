using System.Web;
using System.Web.Optimization;

namespace Ejab.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
 "~/Content/css/bootstrap.rtl.css",
 "~/Content/css/font-awesome.min.css",
 "~/Content/css/hover.css",
   "~/Content/css/owl.carousel.css",
 "~/Content/css/owl.theme.css",
   "~/Content/images/favicon.ico",
 "~/Content/css/owl.theme.css",
   "~/Content/images/favicon.ico",
 "~/Content/css/style.css",
 "~/Content/css/responsive.css"

 ));
            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
             
"~/Content/Admin/css/bootstrap.rtl.css",
"~/Content/Admin/font-awesome/css/font-awesome.css",

"~/Content/Admin/css/plugins/morris/morris-0.4.3.min.css",
"~/Scripts/js/plugins/gritter/jquery.gritter.css",
"~/Content/Admin/css/animate.css",
"~/Content/Admin/css/style.css",
"~/Content/Admin/css/style_ar.css",
"~/Scripts/js/plugins/gritter/jquery.gritter.css",
"~/Content/Admin/css/plugins/morris/morris-0.4.3.min.css"

));

            // jquery
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                 "~/Scripts/js/jquery.js"));
            //boostrap
            bundles.Add(new ScriptBundle("~/Scripts/bootstrao").Include(
                 "~/Scripts/js/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Caresol").Include(
                   "~/Scripts/js/owl.carousel.js"));

            // Wow
            bundles.Add(new ScriptBundle("~/bundles/WOw").Include(
                      "~/Scripts/js/wow.js"));
            // html5lightbox
            bundles.Add(new ScriptBundle("~/bundles/html5lightbox").Include(
                      "~/Scripts/js/html5lightbox.js"));
            // numscroller
            bundles.Add(new ScriptBundle("~/bundles/numscroller").Include(
                      "~/Scripts/js/numscroller-1.0.js"));

            // function
            bundles.Add(new ScriptBundle("~/bundles/function").Include(
                    "~/Scripts/js/function.js"));
            /////
            // metisMenu
            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                    "~/Scripts/js/plugins/metisMenu/jquery.metisMenu.js"));
            // slimscroll
            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
                    "~/Scripts/js/plugins/slimscroll/jquery.slimscroll.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                "~/Scripts/js/inspinia.js"));
            // pace
            bundles.Add(new ScriptBundle("~/bundles/pace").Include(
                    "~/Scripts/js/plugins/pace/pace.min.js"));
            // jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                    "~/Scripts/js/plugins/jquery-ui/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/Scripts/jquery2.11").Include(
               "~/Scripts/js/jquery-2.1.1.js"));


#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
