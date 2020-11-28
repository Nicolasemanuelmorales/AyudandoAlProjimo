using System.Web;
using System.Web.Optimization;

namespace ProyectoPW3_AyudandoAlProjimo
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery3.4.1").Include(
                    "~/Scripts/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/miperfil").Include(
                   "~/Content/js/miperfil.js"));

            bundles.Add(new ScriptBundle("~/bundles/crearpropuesta").Include(
                 "~/Content/js/crearpropuesta.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                        "~/Content/js/sb-admin-2.min.js"));

            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/vendor/fontawesome-free/css/all.min.css",
                "~/Content/css/sb-admin-2.min.css",
                      "~/Content/css/estilos.css"
                      ));
        }
    }
}
