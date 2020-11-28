using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.Auxiliares;
using Entidades.Enums;
namespace Servicios
{
    public class DonacionService
    {
        private PropuestaService _propuestaService = new PropuestaService();
        public List<DonacionAux> MisDonacionesId(int id)
        {
            Entities ctx = new Entities();
            List<DonacionAux> donacions = new List<DonacionAux>();

            #region Donaciones por Id Consulta
            var Donaciones = (from p in ctx.Propuestas
                              join p_mon in ctx.PropuestasDonacionesMonetarias
                               on p.IdPropuesta equals p_mon.IdPropuesta
                              join d_mon in ctx.DonacionesMonetarias
                               on p_mon.IdPropuestaDonacionMonetaria equals d_mon.IdPropuestaDonacionMonetaria
                              where d_mon.IdUsuario == id
                              select new DonacionAux
                              {
                                  Estado = p.Estado,
                                  FechaDonacion = d_mon.FechaCreacion,
                                  IdUsuario = d_mon.IdUsuario,
                                  MiDonacion = d_mon.Dinero,
                                  Nombre = p.Nombre,
                                  TipoDonacion = p.TipoDonacion,
                                  IdPropuesta = p.IdPropuesta
                              }
                             ).ToList();

            var DonacionesI = (from p in ctx.Propuestas
                               join p_in in ctx.PropuestasDonacionesInsumos
                                on p.IdPropuesta equals p_in.IdPropuesta
                               join d_in in ctx.DonacionesInsumos
                                on p_in.IdPropuestaDonacionInsumo equals d_in.IdPropuestaDonacionInsumo
                               where d_in.IdUsuario == id
                               select new DonacionAux
                               {
                                   Estado = p.Estado,
                                   FechaDonacion= (DateTime)d_in.FechaCreacion,
                                   IdUsuario = d_in.IdUsuario,
                                   MiDonacion = d_in.Cantidad,
                                   Nombre = p.Nombre,
                                   IdPropuestaDIns=p_in.IdPropuestaDonacionInsumo,
                                   TipoDonacion = p.TipoDonacion,
                                   IdPropuesta = p.IdPropuesta
                               }
                             ).ToList();
            var DonacionesHrs = (from p in ctx.Propuestas
                                 join p_hrs in ctx.PropuestasDonacionesHorasTrabajo
                                  on p.IdPropuesta equals p_hrs.IdPropuesta
                                 join d_hrs in ctx.DonacionesHorasTrabajo
                                  on p_hrs.IdPropuestaDonacionHorasTrabajo equals d_hrs.IdPropuestaDonacionHorasTrabajo
                                 where d_hrs.IdUsuario == id
                                 select new DonacionAux
                                 {
                                     Estado = p.Estado,
                                     FechaDonacion = (DateTime)d_hrs.FechaCreacion,
                                     IdUsuario = d_hrs.IdUsuario,
                                     MiDonacion = d_hrs.Cantidad,
                                     Nombre = p.Nombre,
                                     TipoDonacion = p.TipoDonacion,
                                     IdPropuesta = p.IdPropuesta
                                 }
                             ).ToList();
            #endregion

            donacions.AddRange(Donaciones);
            donacions.AddRange(DonacionesI);
            donacions.AddRange(DonacionesHrs);

            return CargarDonacionesTotalesALista(donacions);
        }

        public List<DonacionAux> CargarDonacionesTotalesALista(List<DonacionAux> list)
        {
            foreach (DonacionAux item in list)
            {//1->Monetaria   2->Insumos   3->HorasDeTrabajo
                if (item.TipoDonacion == (int)EnumTipoDonacion.Monetaria)
                {
                    item.TotalRecaudado = _propuestaService.CalcularTotalDonadoPropuestaMon(item.IdPropuesta);
                }
                else if (item.TipoDonacion == (int)EnumTipoDonacion.Insumo)
                {
                    item.TotalRecaudado = _propuestaService.CalcularTotalDonadoPropuestaIns(item.IdPropuestaDIns);
                }
                else if (item.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
                {
                    item.TotalRecaudado = _propuestaService.CalcularTotalDonadoPropuestaHrs(item.IdPropuesta);
                }
            }

            return list.OrderByDescending(model=>model.FechaDonacion).ToList();
        }

        public void GuardarDonacionMonetaria(DonacionesMonetarias dm)
        {
            using (var ctx = new Entities())
            {
                PropuestasDonacionesMonetarias pm = (from p in ctx.PropuestasDonacionesMonetarias
                                                     where p.IdPropuestaDonacionMonetaria == dm.IdPropuestaDonacionMonetaria
                                                     select p
                                                    ).First();
                decimal c = (pm.Dinero- _propuestaService.CalcularTotalDonadoPropuestaMon(pm.IdPropuesta));
                if (c==dm.Dinero)
                {
                    _propuestaService.CambiarEstadoPropuestaPorId(pm.IdPropuesta);
                }
                pm.DonacionesMonetarias.Add(dm);
                ctx.SaveChanges();
            }

        }
        public void GuardarDonacionInsumos(DonacionesInsumos dm)
        {
            using (var ctx = new Entities())
            {
                var pm = (from p in ctx.PropuestasDonacionesInsumos
                          where p.IdPropuestaDonacionInsumo == dm.IdPropuestaDonacionInsumo
                          select p
                                                    ).First();
                
                pm.DonacionesInsumos.Add(dm);
                ctx.SaveChanges();
                _propuestaService.ComprobarFinPropuestaIns(pm.IdPropuesta);
            }

        }
        public void GuardarDonacionHoras(DonacionesHorasTrabajo dm)
        {
            using (var ctx = new Entities())
            {
                PropuestasDonacionesHorasTrabajo pm = (from p in ctx.PropuestasDonacionesHorasTrabajo
                                                       where p.IdPropuestaDonacionHorasTrabajo == dm.IdPropuestaDonacionHorasTrabajo
                                                       select p
                                                    ).First();
                int c = (pm.CantidadHoras - _propuestaService.CalcularTotalDonadoPropuestaHrs(pm.IdPropuesta));
                if ( c == dm.Cantidad)
                {
                   _propuestaService.CambiarEstadoPropuestaPorId(pm.IdPropuesta);
                }
                pm.DonacionesHorasTrabajo.Add(dm);
                ctx.SaveChanges();
            }

        }
        public void RealizarDonacion(CrearDonacionAux cd)
        {

            if (cd.TipoDonacion == (int)EnumTipoDonacion.Monetaria)
            {
                DonacionesMonetarias dm = new DonacionesMonetarias();
                dm.FechaCreacion = DateTime.Today;
                dm.Dinero = cd.Dinero;

                var filename = Path.GetFileName(cd.ArchivoTransferencia.FileName);
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/archivostransferencia/"), filename);
                cd.ArchivoTransferencia.SaveAs(path);
                dm.ArchivoTransferencia = filename;

                dm.IdPropuestaDonacionMonetaria = _propuestaService.IdPropuestaMonetaria(cd.IdPropuesta);
                dm.IdUsuario = cd.IdUsuario;
                GuardarDonacionMonetaria(dm);
            }
            else if (cd.TipoDonacion == (int)EnumTipoDonacion.Insumo)
            {
                foreach (var item in cd.dlistins)
                {
                    if (item.Cantidad!=0)
                    {
                        DonacionesInsumos di = new DonacionesInsumos();
                        di.FechaCreacion = DateTime.Today;
                        di.Cantidad = item.Cantidad;
                        di.IdPropuestaDonacionInsumo = item.IdPropuestaDonacionInsumo;
                        di.IdUsuario = item.IdUsuario;
                        di.FechaCreacion = DateTime.Today;
                        GuardarDonacionInsumos(di);
                    }
                }
            }
            else if (cd.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
            {
                DonacionesHorasTrabajo dh = new DonacionesHorasTrabajo();
                dh.FechaCreacion = DateTime.Today;
                dh.Cantidad = cd.CantidadHoras;
                dh.IdPropuestaDonacionHorasTrabajo = _propuestaService.IdPropuestaHoras(cd.IdPropuesta);
                dh.IdUsuario = cd.IdUsuario;
                GuardarDonacionHoras(dh);
            }


        }
    }
}
