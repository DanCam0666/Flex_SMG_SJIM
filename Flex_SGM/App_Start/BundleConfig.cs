using System.Web;
using System.Web.Optimization;

namespace Flex_SGM
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           // bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment-with-locales.js",            
                        "~/Addon/Charts/Chart.min.js",
                           "~/Addon/Charts/chartjs-plugin-datalabels.min.js",
                        "~/Addon/Gauge/gauge.min.js",
                        "~/Addon/bootstrap-datepicker/js/bootstrap-datepicker.js",
                     "~/Addon/bootstrap-datepicker/locales/bootstrap-datepicker.es.min.js",
                       "~/Addon/bootstrap-multiselect/bootstrap-multiselect.js",
                     "~/Scripts/date.js",
                     "~/Scripts/wow.js",
                      "~/Addon/chosen/chosen.jquery.min.js",
                      "~/Addon/DataTables/datatables.min.js",
                       "~/Addon/fontawesome-free-5.12.0-web/js/all.js",
                       "~/Addon/buildGM/js/mycustom.js",
                        "~/Addon/pnotify/PNotify.js",
                       "~/Addon/pnotify/PNotifyButtons.js",
                       "~/Addon/pnotify/PNotifyAnimate.js",
                        "~/Addon/pnotify/PNotifyConfirm.js",
                       "~/Addon/pnotify/PNotifyNonBlock.js",
                        "~/Addon/pnotify/NonBlock.es5.js",
                         "~/Addon/FilePond/filepond.min.js",
                         "~/Addon/TimeAgo/jquery.timeago.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.css",
                        "~/Addon/Charts/Chart.min.css",
                       "~/Content/bootstrap-datetimepicker.min.css",
                       "~/Addon/bootstrap-datepicker/css/bootstrap-datepicker.css",
                        "~/Addon/bootstrap-multiselect/bootstrap-multiselect.css",
                        "~/Addon/chosen/chosen.min.css",
                       "~/Addon/DataTables/datatables.min.css",
                       "~/Addon/pnotify/PNotifyBrightTheme.css",
                       "~/Addon/FilePond/filepond.min.css",
                       "~/Content/css.css"

                       //  "~/Content/FngStyle.css",
                       //  "~/Addon/buildGM/css/custom.css"
                       ));

            //  "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/fcj").Include(
//"~/Addon/DatetimePicker/moment.min.js",
          "~/Addon/fullcalendar/core/main.js",
           "~/Addon/fullcalendar/core/locales-all.js",
                    "~/Addon/fullcalendar/interaction/main.js",
                              "~/Addon/fullcalendar/daygrid/main.js",
                                        "~/Addon/fullcalendar/timegrid/main.js",
                                                  "~/Addon/fullcalendar/list/main.js",
"~/Addon/DatetimePicker/bootstrap-datetimepicker.min.js"


            ));

            bundles.Add(new StyleBundle("~/Content/fcc").Include(
                      "~/Addon/fullcalendar/core/main.css",
                       "~/Addon/fullcalendar/daygrid/main.css",
                        "~/Addon/fullcalendar/timegrid/main.css",
                         "~/Addon/fullcalendar/list/main.css",
                          "~/Addon/DatetimePicker/bootstrap-datetimepicker.min.css"
                       ));

        }
    }
}
