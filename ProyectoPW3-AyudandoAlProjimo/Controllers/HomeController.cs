using System;
using Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using System.IO;

namespace ProyectoPW3_AyudandoAlProjimo.Controllers
{
    public class HomeController : Controller
{
        HomerService l = new HomerService();
        PropuestaService _p = new PropuestaService();
        public ActionResult Index()
        {
            if (Session["usuario"] != null)
            {
                Session["PermitirPropuesta"] = _p.Permitirpropuesta((int)Session["usuario"]);
                return View("Index", l.BuscarPropuestas());
            }
            else
            {
                return View("Index", l.BuscarPropuestas());
            }
        }
        [HttpPost]
        public ActionResult Buscador(FormCollection f)
        {
           return View("Index", l.BuscadorDePropuestas(f["buscar"].ToString()));
        }
        public ActionResult Denuncias()
        {
            if (Session["usuario"] != null)
            {
                List<DenunciaMotivo> denu = new List<DenunciaMotivo>();
                denu = l.BuscarDenuncias();
                return View(denu);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult MiPerfil()
        {
            if (Session["usuario"] != null)
            {
                Session["PermitirPropuesta"] = _p.Permitirpropuesta((int)Session["usuario"]);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult EditarPerfil()
        {
            if (Session["usuario"] != null)
            {
                Session["PermitirPropuesta"] = _p.Permitirpropuesta((int)Session["usuario"]);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CambiarContraseña()
        {
            if (Session["usuario"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login","Login");
            }      
        }

        [HttpPost]
        public ActionResult ChangePassword(Usuarios u, FormCollection form)
        {
            ModelState["Email"].Errors.Clear();
            if (l.BuscarUsuarioPorIdYPassword().Password != form["pass1"])
            {
                ModelState.AddModelError("pass1", "Contraseña incorrecta");
            }
            if (form["pass"].Length == 0)
            {
                ModelState.AddModelError("pass", "Campo obligatorio");
            }
            else if (!u.Password.Equals(form["pass"]))
            {
                ModelState.AddModelError("pass", "Las contraseñas no coinciden");
            }
            if (ModelState.IsValid)
            {
                l.ActualizarPassword(form["pass"]);
                return View("MiPerfil");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View("CambiarContraseña");
        }

    [HttpPost]
        public ActionResult ConfirmarPerfil(Usuarios u)
        {
            DateTime horacorta = u.FechaNacimiento;
            TimeSpan ts = DateTime.Today - horacorta;
            int diferencia = ts.Days;

            if (u.Nombre == "" | u.Nombre == null | u.Apellido == "" | u.Apellido == null | diferencia <= 6570)
            {
                if (u.Nombre == "" | u.Nombre == null)
                {
                    ViewBag.Nombre = "El Nombre es obligatorio";
                }

                if (diferencia <= 6570)
                {
                    ViewBag.fn = "Tiene que ser mayor a 18 años";
                }

                if (u.Apellido == "" | u.Apellido == null)
                {
                    ViewBag.Apellido = "El Apellido es obligatorio";
                }
                return View("EditarPerfil");
            }

            l.ActualizarPerfil(u);

            Session["Name"] = u.Nombre;
            Session["Apellido"] = u.Apellido;
            Session["fn"] = u.FechaNacimiento.ToString("MM/dd/yyyy");
            Session["PermitirPropuesta"] = _p.Permitirpropuesta((int)Session["usuario"]);
            return View("MiPerfil");
        }

        [HttpPost]
        public ActionResult CambiarImagen(HttpPostedFileBase imagen)
        {
            if (Session["usuario"] != null)
            {
                if (imagen != null)
                {
                    String filename = Path.GetFileName(imagen.FileName);
                    l.CargarImagen(imagen);
                    Session["Foto"] = "/Content/img/" + filename;
                    Session["PermitirPropuesta"] = _p.Permitirpropuesta((int)Session["usuario"]);
                    return View("MiPerfil");
                }
                return View("MiPerfil");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

    }
}