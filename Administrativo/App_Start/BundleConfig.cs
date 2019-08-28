using System.Web;
using System.Web.Optimization;

namespace Administrativo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           bundles.Add(new StyleBundle("~/Content/css-application").Include(
                    "~/Content/AdminLTE/bootstrap.min.css",
                    "~/Content/AdminLTE/font-awesome.min.css",
                    "~/Content/AdminLTE/ionicons.min.css",
                    "~/Content/AdminLTE/morris/morris.css",
                    "~/Content/AdminLTE/jvectormap/jquery-jvectormap-1.2.2.css",
                    "~/Content/AdminLTE/fullcalendar/fullcalendar.css",
                    "~/Content/AdminLTE/daterangepicker/daterangepicker-bs3.css",
                    "~/Content/AdminLTE/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                    "~/Scripts/AdminLTE/plugins/chzn-select/chosen.css",
                    "~/Scripts/AdminLTE/plugins/chzn-select/docsupport/prism.css",
                    "~/Scripts/AdminLTE/plugins/chzn-select/docsupport/style.css",
                    "~/Content/AdminLTE/datatables/dataTables.bootstrap.css",
                    "~/Content/AdminLTE/AdminLTE.css",
                    "~/Content/AdminLTE/jQueryUI/jquery-ui.structure.min.css",
                    "~/Content/AdminLTE/jQueryUI/jquery-ui.theme.min.css",
                    "~/Content/AdminLTE/jQueryUI/jquery-ui.min.css",
                    "~/Content/ElFinder/css/colorbox.css",
                    "~/Content/AdminLTE/tagIt/jquery.tagit.css",
                    "~/Content/AdminLTE/tagIt/tagit.ui-zendesk.css",
                    "~/Content/AdminLTE/swal/sweetalert2.min.css",
                        "~/Content/bootstrap-datetimepicker.css",
                        "~/Content/bootstrap-datetimepicker.min.css",
                        "~/Content/JCrop/css/jquery.jcrop.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new ScriptBundle("~/bundles/scripts-application").Include(
                    "~/Scripts/jquery-{version}.js",
                    //"~/Scripts/AdminLTE/jquery.min.js",
                    "~/Scripts/AdminLTE/bootstrap.min.js",
                    "~/Scripts/globalize.min.js",
                    "~/Scripts/moment.min.js",
                    "~/Scripts/AdminLTE/plugins/daterangepicker/daterangepicker.js",
                    "~/Scripts/plugins/datepicker/bootstrap-datepicker.js",
                    "~/Scripts/plugins/datepicker/locales/bootstrap-datepicker.pt-BR.js",
                                         "~/Scripts/AdminLTE/plugins/daterangepicker/daterangepicker.js",
                        "~/Scripts/AdminLTE/plugins/datepicker/bootstrap-datepicker.js",
                        "~/Scripts/AdminLTE/plugins/datepicker/locales/bootstrap-datepicker.pt-BR.js",
                         "~/Scripts/bootstrap-datetimepicker.min.js",
                    "~/Scripts/AdminLTE/app.js",
                    "~/Scripts/bootstrap-modal-confirm.min.js",
                    "~/Scripts/jquery.mask.js",
                    "~/Scripts/jquery-ui-1.8.24.min.js",
                    "~/Scripts/scripts.js",
                    "~/Content/ElFinder/js/jquery.colorbox.js",
                    "~/Scripts/AdminLTE/plugins/swal/sweetalert2.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts-plugins").Include(
                    "~/Scripts/AdminLTE/plugins/Raphael.2.1.0/raphael-min.js",
                    "~/Scripts/AdminLTE/plugins/morris/morris.min.js",
                    "~/Scripts/AdminLTE/plugins/sparkline/jquery.sparkline.min.js",
                    "~/Scripts/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                    "~/Scripts/AdminLTE/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                    "~/Scripts/AdminLTE/plugins/fullcalendar/fullcalendar.min.js",
                    "~/Scripts/AdminLTE/plugins/jqueryKnob/jquery.knob.js",
                    "~/Scripts/AdminLTE/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                    "~/Scripts/AdminLTE/plugins/iCheck/icheck.min.js",
                    "~/Scripts/AdminLTE/plugins/chzn-select/chosen.jquery.js",
                    "~/Scripts/AdminLTE/plugins/input-mask/jquery.inputmask.js",
                    "~/Scripts/AdminLTE/plugins/ckeditor/ckeditor.js",
                    "~/Scripts/AdminLTE/plugins/chzn-select/docsupport/prism.js",
                    "~/Scripts/AdminLTE/plugins/TagIt/tag-it.min.js",
                    "~/Scripts/AdminLTE/plugins/swal/sweetalert2.min.js"
                ));
            /*    <script src="~/Scripts/plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="~/Scripts/plugins/datepicker/locales/bootstrap-datepicker.pt-BR.js"></script>*/
            bundles.Add(new ScriptBundle("~/bundles/ie9").Include(
                        "~/Scripts/html5shiv.js",
                        "~/Scripts/respond.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/galeria").Include(
                "~/Scripts/galerias.min.js"
                ));


            bundles.Add(new StyleBundle("~/Content/file-upload").Include(
                    "~/Content/FileUpload/jquery.fileupload.css",
                    "~/Content/FileUpload/jquery.fileupload-ui.css",
                    "~/Content/FileUpload/jquery.fileupload-noscript.css",
                    "~/Content/FileUpload/jquery.fileupload-ui-noscript.css"
                ));


            bundles.Add(new ScriptBundle("~/bundles/file-upload").Include(
                "~/Scripts/FileUpload/load-image.js",
                "~/Scripts/FileUpload/tmpl.js",
                "~/Scripts/FileUpload/canvas-to-blob.js",
                "~/Scripts/FileUpload/vendor/jquery.ui.widget.js",
                "~/Scripts/FileUpload/jquery.iframe-transport.js",
                "~/Scripts/FileUpload/jquery.fileupload.js",
                "~/Scripts/FileUpload/jquery.fileupload-process.js",
                "~/Scripts/FileUpload/jquery.fileupload-image.js",
                "~/Scripts/FileUpload/jquery.fileupload-validate.js",
                "~/Scripts/FileUpload/jquery.fileupload-ui.js",
                "~/Scripts/FileUpload/locale.js",
                "~/Scripts/FileUpload/main.js"
            ));


            
        }
    }
}