using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using Entidades.Enums;
using Entidades.Auxiliares;
using Servicios;
using System.Net.Mail;
using WebApiDonaciones.Controllers;
namespace ProyectoPW3_AyudandoAlProjimo.Controllers
{
    public class DonacionesController : Controller
    {
        private PropuestaService _propuestaService = new PropuestaService();
        private DonacionService _DonacionService = new DonacionService();
        public ActionResult MiHistorialDonaciones()
        {
            ViewBag.Usuario = Session["usuario"];
            return View();
        }
        [HttpGet]
        public ActionResult RealizarDonacion(int id)
        {
            CrearDonacionAux cd = new CrearDonacionAux()
            {
                TipoDonacion= (_propuestaService.GetPorId(id)).TipoDonacion,
                IdPropuesta=id
            };
            if (cd.TipoDonacion== (int)EnumTipoDonacion.Monetaria)
            {
                cd.TotalMon = _propuestaService.TotalPropuestaMon(id);
                cd.FaltanteMon = (_propuestaService.TotalPropuestaMon(id) - _propuestaService.CalcularTotalDonadoPropuestaMon(id));
            }
            else if (cd.TipoDonacion == (int)EnumTipoDonacion.Insumo)
            {
                ViewBag.ListaIns = _propuestaService.GetPorId(cd.IdPropuesta).PropuestasDonacionesInsumos.ToList();
                foreach (var item in _propuestaService.GetPorId(id).PropuestasDonacionesInsumos.ToList())
                {
                   
                    cd.Faltantes.Add(_propuestaService.TotalPropuestaIns(item.IdPropuestaDonacionInsumo) - _propuestaService.CalcularTotalDonadoPropuestaIns(item.IdPropuestaDonacionInsumo));
                }
            }
            else if (cd.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
            {
                cd.Totalh=_propuestaService.TotalPropuestaHrs(id);
                cd.Profesion = _propuestaService.RetornarProfesionPorIdPropuesta(id);
                cd.Faltanteh=(_propuestaService.TotalPropuestaHrs(id) - _propuestaService.CalcularTotalDonadoPropuestaHrs(id));
            }
            return View(cd);
        }
        [HttpPost]
        public ActionResult RealizarDonacion(CrearDonacionAux cd)
        {
            if (cd.TipoDonacion==(int)EnumTipoDonacion.Monetaria)
            {
                if ((cd.Dinero>cd.FaltanteMon))
                {
                    ModelState.AddModelError("Dinero","El dinero ingresado es mayor al necesitado");
                }
                if (cd.ArchivoTransferencia == null)
                {
                    ModelState.AddModelError("ArchivoTransferencia", "El archivo de tranferencia es obligatorio");
                }
            }
            if (cd.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
            {
                if (cd.CantidadHoras > cd.Faltanteh)
                {
                    ModelState.AddModelError("CantidadHoras", "La cantidad de horas ingresada es mayor al necesitada");
                }
            }
            if (cd.TipoDonacion == (int)EnumTipoDonacion.Insumo)
            {
                for (int i = 0; i < cd.dlistins.Count; i++)
                {
                    if (cd.dlistins[i].Cantidad>cd.Faltantes[i])
                    {
                        ModelState.AddModelError("dlistins["+i+"].Cantidad", "La cantidad de insumos ingresado es mayor al necesitado");
                    }
                }
            }

            if (!ModelState.IsValid)
            {   
                if (cd.TipoDonacion==(int)EnumTipoDonacion.Insumo)
                {
                    foreach (var item in _propuestaService.GetPorId(cd.IdPropuesta).PropuestasDonacionesInsumos.ToList())
                    {
                        cd.Faltantes.Add(_propuestaService.TotalPropuestaIns(item.IdPropuestaDonacionInsumo) - _propuestaService.CalcularTotalDonadoPropuestaIns(item.IdPropuestaDonacionInsumo));
                    }
                    ViewBag.ListaIns = _propuestaService.GetPorId(cd.IdPropuesta).PropuestasDonacionesInsumos.ToList();
                }
                return View(cd);
            }
            
                _DonacionService.RealizarDonacion(cd);
            
                return RedirectToAction("Index", "Home");

        }
    }
}