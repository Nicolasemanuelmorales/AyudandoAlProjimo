using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoPW3_AyudandoAlProjimo.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult ErrorPage(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "error-500.png";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "error-404.jpg";
                    break;

                default:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "error.png";
                    break;
            }

            return View();
        }
    }
}