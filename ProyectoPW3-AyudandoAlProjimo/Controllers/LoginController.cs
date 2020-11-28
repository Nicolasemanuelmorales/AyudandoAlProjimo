using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoPW3_AyudandoAlProjimo.Controllers
{
    public class LoginController : Controller
    {
        LoginService l = new LoginService();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return View("Login");
        }

        public ActionResult activar(String token)
        {
            l.ActivarCuenta(token);
            return View("login");
        }

        [HttpPost]
        public ActionResult ValidarUsuario(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                Usuarios usu = new Usuarios();

                usu = l.BuscarUsuario(u);

                if (usu.Email != null)
                {
                    if (l.BuscarUsuarioActivo(u).Activo == true)
                    { 
                        Session["usuario"] = usu.IdUsuario;
                        Session["Email"] = usu.Email;

                        if (usu.Foto != null)
                        {
                            Session["Foto"] = "/Content/img/" + usu.Foto.ToString();
                        }
                        else
                        {
                            Session["Foto"] = null;
                        }
                        
                        Session["Name"] = usu.Nombre;
                        Session["Apellido"] = usu.Apellido;
                        Session["fn"] = usu.FechaNacimiento.ToString("MM/dd/yyyy");

                        String NombreDeUsuario =usu.Nombre + "." + usu.Apellido;
                        if (usu.Nombre != null && usu.Apellido != null)
                        { 
                            if (usu.UserName == null)
                            {
                                if (l.buscarUsernames(NombreDeUsuario).Count >= 1)
                                {
                                    Session["Nombre"] = l.GuardarNombreDeUsuario2(usu, NombreDeUsuario).UserName;
                                }
                                else
                                {
                                    Session["Nombre"] = l.GuardarNombreDeUsuario(usu, NombreDeUsuario).UserName;
                                }
                            }
                            else
                            {
                                Session["Nombre"] = usu.UserName;
                            }
                        }
                        Session["Tipo"] = usu.TipoUsuario;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = " Actívelo desde el email recibido";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.Error = "Email y/o Contraseña inválidos";
                    return View("Login");
                }
            }
            return View("Login");
        }

        [HttpPost]
        public ActionResult ValidarUsuarioRegistrar(Usuarios u, FormCollection s)
        {
            if (l.ExisteCorreo(u).Count == 1)
            {
                ModelState.AddModelError("Email", "Email ya registrado");
            }

            DateTime horacorta = u.FechaNacimiento;
            TimeSpan ts = DateTime.Today - horacorta;
            int diferencia = ts.Days;

            if (diferencia <= 6570)
            {
                ModelState.AddModelError("FechaNacimiento", "Tiene que ser mayor a 18 años");
            }
            if (s["pass"].Length == 0)
            {
                ModelState.AddModelError("pass", "Campo obligatorio");
            }
           else if (!u.Password.Equals(s["pass"]))
            {
                ModelState.AddModelError("pass", "Las contraseñas no coinciden");
            }
            if (ModelState.IsValid)
            {
                LoginService l = new LoginService();
                l.RegistrarUsuario(u);
                l.enviarCorreo(u);
                return RedirectToAction("Index", "Home");
            }
            return View("Registrar");

        }

    }
}