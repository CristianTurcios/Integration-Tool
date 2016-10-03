using System.Web;
using System.Web.Optimization;

namespace IntegrationTool
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/IntegrationToolAppControllers")
                .IncludeDirectory("~/Content/Scripts/Controllers/", "*.js"));

            bundles.Add(new ScriptBundle("~/Content/IntegrationToolAppDirectives")
                .IncludeDirectory("~/Content/Scripts/Directives/", "*.js"));

            bundles.Add(new ScriptBundle("~/Content/IntegrationToolAppServices")
                .IncludeDirectory("~/Content/Scripts/Services/", "*.js"));

            bundles.Add(new ScriptBundle("~/Content/IntegrationToolApp")
                .Include("~/Content/Scripts/IntegrationToolApp.js"));

            bundles.Add(new ScriptBundle("~/Content/Plugins")
                .IncludeDirectory("~/Content/Scripts/Plugins/", "*.js"));

            bundles.Add(new StyleBundle("~/Content/Styles")
                .IncludeDirectory("~/Content/Stylesheets/Styles/", "*.css"));

            bundles.Add(new StyleBundle("~/Content/CSSBootstrap")
                .Include("~/Content/Bootstrap/css/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/Content/JSBootstrap")
                .Include("~/Content/Bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/CSSTemplate")
                .Include("~/Content/Template/css/template.min.css")
                .Include("~/Content/Template/css/skins/_all-skins.min.css"));

            bundles.Add(new StyleBundle("~/Content/JSTemplate")
                .Include("~/Content/Template/js/app.min.js")
                .Include("~/Content/Template/js/demo.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}