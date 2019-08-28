using System.Web;
using System.Web.Optimization;

namespace Site
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/player").Include(
            "~/scripts/jquery.js",
            "~/scripts/player.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/scripts/jquery.js",
                "~/scripts/pace.js",
                "~/scripts/slick.js",
                "~/scripts/jquery.unobtrusive-ajax.js",
                "~/scripts/sweetalert2.min.js",
                "~/scripts/chzn-select/chosen.jquery.js",
                "~/scripts/chzn-select/docsupport/prism.js",
                "~/scripts/scripts.js",
                "~/scripts/layout.js",
                "~/scripts/jquery.iframetracker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css-player").Include(
            "~/Content/player/animation.css",
            "~/Content/player/fontello-codes.css",
            "~/Content/player/fontello-embedded.css",
            "~/Content/player/fontello-ie7-codes.css",
            "~/Content/player/fontello-ie7.css",
            "~/Content/player/fontello.css",
            "~/Content/player/player.css"
            ));

            bundles.Add(new StyleBundle("~/Content/css-application").Include(
                      "~/Content/jquery.fancybox.css",
                      "~/Content/sweetalert2.min.css",
                      "~/Content/chzn-select/chosen.css",
                      "~/Content/chzn-select/docsupport/prism.css",
                      "~/Content/banner-topo.css",
                      "~/Content/copa.css",
                      //"~/Content/chzn-select/docsupport/style.css",
                      "~/Content/styles.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/seubolso").Include(
                        "~/scripts/seubolso/js/materialize.min.js",
                        "~/scripts/seubolso/js/Chart.min.js",
                        "~/scripts/slick.js",
                        "~/scripts/seubolso/js/jquery.maskMoney.min.js",
                        "~/scripts/seubolso/js/jquery.mCustomScrollbar.min.js",
                        "~/scripts/seubolso/js/main.js",
                        "~/scripts/seubolso/js/seubolso.js",
                        "~/scripts/seubolso/js/aposentadoria.js",
                        "~/scripts/seubolso/js/conversor.js",
                        "~/scripts/money.min.js"));
            bundles.Add(new StyleBundle("~/Content/seubolso").Include(
                      "~/Content/styles_seubolso.css",
                      "~/Content/jquery.mCustomScrollbar.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/slick-theme.min.css",
                      "~/Content/slick.min.css"));
        }
    }
}
