using System.Web;
using System.Web.Optimization;

namespace Lpp.Dns.Resources
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/basescripts").Include(
                        "~/Scripts/modernizr-2.8.2.min.js",
                        "~/Scripts/es6-shim.js",
                        "~/Scripts/jquery-2.1.1.min.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/jquery.signalR-2.1.0.min.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/knockout-3.1.0.js",
                        "~/Scripts/knockout.validation.debug.js",
                        "~/Scripts/knockout.mapping-latest.debug.js",
                        "~/Scripts/knockout-bootstrap.min.js",
                        "~/Scripts/CustomKnockoutBindings.js",
                        "~/Scripts/linq.js",
                        "~/Scripts/jquery.linq.js",
                        "~/Scripts/history.js",
                        "~/Scripts/history.adapter.jquery.js",
                        "~/Scripts/purl.js"));

            bundles.Add(new StyleBundle("~/Content/basecss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/kendo.common-bootstrap.min.css",
                      "~/Content/kendo.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/durandal.css"));

            bundles.Add(new ScriptBundle("~/scripts/kendoall").Include(
                    "~/Scripts/kendo/kendo.web.min.js",
                    "~/Scripts/kendo/kendo.data.webapi.js",
                    "~/Scripts/knockout-kendo.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jqueryui").Include(
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/knockout-jqueryui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/dnsapi").Include(
                    "~/Scripts/Lpp.dns.Interfaces.js",
                    "~/Scripts/Lpp.dns.ViewModels.js",
                    "~/Scripts/Lpp.dns.WebApi.js"
                ));
        }
    }
}
