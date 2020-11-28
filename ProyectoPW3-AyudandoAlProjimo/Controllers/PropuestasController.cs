using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using Entidades.Enums;
using Entidades.Auxiliares;
using Servicios;
using Entidades.Partials;
namespace ProyectoPW3_AyudandoAlProjimo.Controllers
{
    public class PropuestasController : Controller
    {
        static HttpPostedFileBase fotofija;
        private readonly PropuestaService _propuestaService = new PropuestaService();
        public ActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Crear(PropuestaAux p, HttpPostedFileBase Foto)
        {
            if (Foto == null)
            {
                ModelState.AddModelError("Foto", "Imagen requerida");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            p.IdUsuarioCreador = int.Parse(Session["usuario"].ToString());

            fotofija = Foto;
            if (p.TipoDonacion == (int)EnumTipoDonacion.Insumo)
            {
                p.pins = new List<PropuestasDonacionesInsumos>();
                Session["pinsumo"] = p;
                return RedirectToAction("CargarListaInsumos", "Propuestas", p);
            }
            _propuestaService.RegistrarPropuesta(p, Foto);
            return RedirectToAction("MisPropuestas", "Propuestas");
        }

        [HttpGet]
        public ActionResult CargarListaInsumos(PropuestaAux p)
        {
            //idpropuesta nombre cant
            return View(_propuestaService.CrearListaPropuestasInsumos(p.CantidadIns));
        }
        [HttpPost]
        public ActionResult CargarListaInsumos(List<PropuestasDonacionesInsumos> plist)
        {


            if (!ModelState.IsValid)
            {
                return View(plist);
            }
            PropuestaAux p = (PropuestaAux)Session["pinsumo"];

            foreach (var item in plist)
            {
                p.pins.Add(item);
            }
            _propuestaService.RegistrarPropuesta(p, fotofija);
            return RedirectToAction("MisPropuestas", "Propuestas");
        }
        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ViewBag.IdPropuesta = id;
            ViewBag.TamListaIns = _propuestaService.TamListaInsPorId(id);
            ViewBag.Tipo = (_propuestaService.GetPorId(id)).TipoDonacion;
            PropuestaAux p = _propuestaService.CargarPropuestaAux(id);
            return View(p);
        }
        [HttpPost]
        public ActionResult Modificar(PropuestaAux p, HttpPostedFileBase FotoN)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdPropuesta = p.IdPropuesta;
                ViewBag.TamListaIns = _propuestaService.TamListaInsPorId(p.IdPropuesta);
                ViewBag.Tipo = (_propuestaService.GetPorId(p.IdPropuesta)).TipoDonacion;
                return View(p);
            }

            _propuestaService.ModificarPropuesta(p, FotoN);

            return RedirectToAction("MisPropuestas", "Propuestas");


        }
        public ActionResult MisPropuestas()
        {
            if (Session["usuario"] != null)
            {
                return View("MisPropuestas", _propuestaService.BuscarPropuestas());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult Activas()
        {
            if (Session["usuario"] != null)
            {
                return View("MisPropuestas", _propuestaService.BuscarPropuestasActivas());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult Inactivas()
        {
            if (Session["usuario"] != null)
            {
                return View("MisPropuestas", _propuestaService.BuscarPropuestasInactivas());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        [HttpGet]
        public ActionResult ValoracionPropuesta(int IdUser, int IdPropuesta, int valoracion)
        {
            _propuestaService.CrearValoracion(IdUser, IdPropuesta, valoracion);
            string id2 = IdPropuesta.ToString();
            return RedirectToAction("DetallePropuesta", "Propuestas", new { id = id2 });
        }


        public ActionResult DetallePropuesta(String id)
        {

            PropuestaWrapper wrapper = new PropuestaWrapper();
            wrapper.idLogeado = Convert.ToInt32(Session["usuario"]);

            string IdUser = Session["usuario"].ToString();

            int idInt = Int32.Parse(id);

            wrapper.Propuesta = this._propuestaService.GetWithFKPorId(idInt);

            if (wrapper.Propuesta != null)
            {
                switch (wrapper.Propuesta.TipoDonacion)
                {
                    case (3):
                        int idDonacion = this._propuestaService.ObtenerIddonacionHorasTrabajo(idInt);
                        wrapper.PropuestasDonacionesHorasTrabajo = this._propuestaService.GetHorasTrabajoByPropuesta(idInt);
                        wrapper.GetDonacionesHorasTrabajo = this._propuestaService.GetDonacionesHorasTrabajo(idDonacion);
                        wrapper.DonacionesHorasFaltantes = wrapper.PropuestasDonacionesHorasTrabajo - wrapper.GetDonacionesHorasTrabajo;
                        break;
                    case (2):
                        List<int> idDonInsu = this._propuestaService.ObtenerIdDonacionesInsumos(idInt);
                        wrapper.PropuestasDonacionesInsumos = this._propuestaService.GetInsumosByPropuesta(idInt);
                        wrapper.GetDonacionesInsumos = this._propuestaService.GetDonacionesInsumos(idDonInsu);
                        wrapper.TotalesDonacionesInsumos = this._propuestaService.GetTotalesInsumos
                            (
                            wrapper.PropuestasDonacionesInsumos = this._propuestaService.GetInsumosByPropuesta(idInt),
                            wrapper.GetDonacionesInsumos = this._propuestaService.GetDonacionesInsumos(idDonInsu),
                            idDonInsu,
                            idInt
                            );
                        break;
                    case (1):
                        int idDonacion2 = this._propuestaService.ObtenerIddonacionMonetaria(idInt);
                        wrapper.PropuestasDonacionesMonetarias = this._propuestaService.GetMonetariasByPropuesta(idInt);
                        wrapper.GetDonacionesMonetarias = this._propuestaService.GetDonacionesMonetarias(idDonacion2);
                        wrapper.DonacionesMonetariasFaltantes = wrapper.PropuestasDonacionesMonetarias - wrapper.GetDonacionesMonetarias;
                        break;
                }
            }

            wrapper.Usuario = this._propuestaService.GetUsuarioPorPropuesta(idInt);

            wrapper.VotosNegativos = this._propuestaService.ValoracionesNegativas(idInt);
            wrapper.VotosPositivos = this._propuestaService.ValoracionesPositivas(idInt);

            this._propuestaService.CantidadTotalVotos(wrapper.VotosPositivos, wrapper.VotosNegativos, idInt);

            wrapper.porcentajePositivo = this._propuestaService.CalcularPorcentajePositivo(wrapper.VotosPositivos, wrapper.VotosNegativos);

            if (wrapper.porcentajePositivo == 50)
            {
                wrapper.porcentajeNegativo = 50;
            }
            else if (wrapper.porcentajePositivo == 100)
            {
                wrapper.porcentajeNegativo = 0;
            }
            else if (wrapper.porcentajePositivo == 200)
            {
                wrapper.porcentajeNegativo = wrapper.porcentajePositivo - 100;
                wrapper.porcentajePositivo = 0;
            }
            else
            {
                wrapper.porcentajeNegativo = 100 - wrapper.porcentajePositivo;
            }

            wrapper.voto = this._propuestaService.GetVotoPorId(IdUser, idInt);

            wrapper.Denuncia = this._propuestaService.PuedeDenunciar(idInt, IdUser);

            //wrapper.Propuesta = this._propuestaService.GetUserPorPropuesta(idInt);

            return View(wrapper);
        }
    }
}