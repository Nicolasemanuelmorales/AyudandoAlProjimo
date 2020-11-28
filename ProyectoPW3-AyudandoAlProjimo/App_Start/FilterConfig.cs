using System.Web;
using System.Web.Mvc;

namespace ProyectoPW3_AyudandoAlProjimo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
