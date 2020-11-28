using Entidades;
using Entidades.Auxiliares;
using Entidades.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Servicios
{
    public class PropuestaService
    {
        Entities asd = new Entities();
        public void RegistrarPropuesta(PropuestaAux p, HttpPostedFileBase Foto)
        {
            Propuestas pr = new Propuestas();
            pr.Descripcion = p.Descripcion;
            pr.Estado = 0;
            pr.FechaCreacion = DateTime.Today;
            pr.FechaFin = p.FechaFin;


            var filename = Path.GetFileName(Foto.FileName);
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/"), filename);
            Foto.SaveAs(path);
            pr.Foto = filename;

            pr.PropuestasReferencias.Add(
                new PropuestasReferencias()
                {
                    Nombre = p.NombreRef1,
                    Telefono = p.Telefono1
                }
                );
            pr.PropuestasReferencias.Add(
                new PropuestasReferencias()
                {
                    Nombre = p.NombreRef2,
                    Telefono = p.Telefono2
                }
                );
            pr.IdUsuarioCreador = p.IdUsuarioCreador;
            pr.Nombre = p.Nombre;
            pr.TelefonoContacto = p.TelefonoContacto;
            pr.TipoDonacion = p.TipoDonacion;

            if (p.TipoDonacion == (int)EnumTipoDonacion.Monetaria)
            {
                PropuestasDonacionesMonetarias d = new PropuestasDonacionesMonetarias();
                d.Dinero = p.Dinero;
                d.CBU = p.CBU;
                pr.PropuestasDonacionesMonetarias.Add(d);
            }
            if (p.TipoDonacion == (int)EnumTipoDonacion.Insumo)
            {
                foreach (var item in p.pins)
                {
                    pr.PropuestasDonacionesInsumos.Add(item);
                }

            }
            if (p.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
            {
                PropuestasDonacionesHorasTrabajo d = new PropuestasDonacionesHorasTrabajo();
                d.CantidadHoras = p.CantidadHoras;
                d.Profesion = p.Profesion;
                pr.PropuestasDonacionesHorasTrabajo.Add(d);
            }
            using (Entities ctx = new Entities())
            {
                ctx.Propuestas.Add(pr);
                ctx.SaveChanges();
            }
        }

        public PropuestaAux CargarPropuestaAux(int id)
        {
            using (var ctx=new Entities())
            {
                Propuestas p = (from pro in ctx.Propuestas
                                  where pro.IdPropuesta==id
                                  select pro).First();
                PropuestaAux paux = new PropuestaAux
                {
                    Descripcion = p.Descripcion,
                    FechaFin = p.FechaFin,
                    IdPropuesta = p.IdPropuesta,
                    IdUsuarioCreador = p.IdUsuarioCreador,
                    Nombre = p.Nombre,
                    TelefonoContacto = p.TelefonoContacto,
                    TipoDonacion = p.TipoDonacion
                };
                paux.Telefono1 = p.PropuestasReferencias.First().Telefono;
                paux.NombreRef1= p.PropuestasReferencias.First().Nombre;
                paux.Telefono2 = p.PropuestasReferencias.Skip(1).First().Telefono;
                paux.NombreRef2= p.PropuestasReferencias.Skip(1).First().Nombre;

                if (p.TipoDonacion==(int)EnumTipoDonacion.HorasTrabajo)
                {
                    paux.CantidadHoras= p.PropuestasDonacionesHorasTrabajo.First().CantidadHoras;
                    paux.Profesion = p.PropuestasDonacionesHorasTrabajo.First().Profesion;
                }
                else if (p.TipoDonacion == (int)EnumTipoDonacion.Monetaria)
                {
                    paux.Dinero = p.PropuestasDonacionesMonetarias.First().Dinero;
                    paux.CBU = p.PropuestasDonacionesMonetarias.First().CBU;
                }
                else if (p.TipoDonacion == (int)EnumTipoDonacion.Insumo)
                {
                    foreach (var item in p.PropuestasDonacionesInsumos.ToList())
                    {
                        paux.pins.Add(item);
                    }
                }
                return paux;


            }
        }

        public int Permitirpropuesta(int id)
        {
            using (var ctx=new Entities())
            {
                var cant = (from p in ctx.Propuestas
                            where p.IdUsuarioCreador == id
                            && p.Estado==0
                            select p).ToList();
                var usu = (from u in ctx.Usuarios
                           where u.IdUsuario==id
                           select u).First();
                if (usu.Nombre == null || usu.Apellido == null || usu.FechaNacimiento == null || usu.Foto == null ||
                        usu.Nombre == "" || usu.Apellido == "" || usu.Foto == "")
                {
                    return 1;
                }
                else if (cant.Count>=3)
                {
                    
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PropuestaUsuario> BuscarPropuestas()
        {
            int id = Int32.Parse(HttpContext.Current.Session["usuario"].ToString());
            var usua2 = (from p in asd.Propuestas
                         join f in asd.Usuarios on p.IdUsuarioCreador equals f.IdUsuario
                         where f.IdUsuario == id
                         orderby p.FechaCreacion
                         select new
                         {
                             idPropuesta = p.IdPropuesta,
                             nombre = p.Nombre,
                             usuario = f.Nombre,
                             foto = p.Foto,
                             fechaFin = p.FechaFin,
                             porcentajePositivo = p.Valoracion,

                         }).ToList();

            List<PropuestaUsuario> propu = new List<PropuestaUsuario>();

            foreach (var a in usua2)
            {
                PropuestaUsuario p = new PropuestaUsuario();
                p.fechaFin = a.fechaFin;
                p.foto = a.foto;
                p.idPropuesta = a.idPropuesta;
                p.nombre = a.nombre;
                p.usuario = a.usuario;
                p.porcentajePositivo = Convert.ToDecimal(a.porcentajePositivo);
                p.generarPorcentajeNegativo();

                propu.Add(p);
            }
            return propu;
        }

        public void ComprobarFinPropuestaIns(int idPropuesta)
        {
            using (var ctx=new Entities())
            {
                int c = 0;
                Propuestas p = (from pro in ctx.Propuestas
                                where pro.IdPropuesta == idPropuesta
                                select pro).First();
                int c2 = p.PropuestasDonacionesInsumos.ToList().Count;
                foreach (var item in p.PropuestasDonacionesInsumos.ToList())
                {
                    if (item.Cantidad==CalcularTotalDonadoPropuestaIns(item.IdPropuestaDonacionInsumo))
                    {
                        c++;
                    }
                }
                if (c==c2)
                {
                    p.Estado = 1;
                    ctx.SaveChanges();
                }
            }
        }

        public List<PropuestaUsuario> BuscarPropuestasActivas()
        {
            int id = Int32.Parse(HttpContext.Current.Session["usuario"].ToString());
            var usua2 = (from p in asd.Propuestas
                         join f in asd.Usuarios on p.IdUsuarioCreador equals f.IdUsuario
                         where f.IdUsuario == id && p.FechaFin >= DateTime.Today
                         orderby p.FechaCreacion
                         select new
                         {
                             idPropuesta = p.IdPropuesta,
                             nombre = p.Nombre,
                             usuario = f.Nombre,
                             foto = p.Foto,
                             fechaFin = p.FechaFin,
                             porcentajePositivo = p.Valoracion,

                         }).ToList();

            List<PropuestaUsuario> propu = new List<PropuestaUsuario>();

            foreach (var a in usua2)
            {
                PropuestaUsuario p = new PropuestaUsuario();
                p.fechaFin = a.fechaFin;
                p.foto = a.foto;
                p.idPropuesta = a.idPropuesta;
                p.nombre = a.nombre;
                p.usuario = a.usuario;
                p.porcentajePositivo = Convert.ToDecimal(a.porcentajePositivo);
                p.generarPorcentajeNegativo();

                propu.Add(p);
            }
            return propu;
        }

        public void CambiarEstadoPropuestaPorId(int idPropuesta)
        {
            using (var ctx = new Entities())
            {
                Propuestas p = (from pro in ctx.Propuestas
                                where pro.IdPropuesta == idPropuesta
                                select pro).First();
                p.Estado = 1;
                ctx.SaveChanges();
            }
        }

        public List<PropuestaUsuario> BuscarPropuestasInactivas()
        {
            int id = Int32.Parse(HttpContext.Current.Session["usuario"].ToString());
            var usua2 = (from p in asd.Propuestas
                         join f in asd.Usuarios on p.IdUsuarioCreador equals f.IdUsuario
                         where f.IdUsuario == id && p.FechaFin <= DateTime.Today
                         orderby p.FechaCreacion
                         select new
                         {
                             idPropuesta = p.IdPropuesta,
                             nombre = p.Nombre,
                             usuario = f.Nombre,
                             foto = p.Foto,
                             fechaFin = p.FechaFin,
                             porcentajePositivo = p.Valoracion,

                         }).ToList();

            List<PropuestaUsuario> propu = new List<PropuestaUsuario>();

            foreach (var a in usua2)
            {
                PropuestaUsuario p = new PropuestaUsuario();
                p.fechaFin = a.fechaFin;
                p.foto = a.foto;
                p.idPropuesta = a.idPropuesta;
                p.nombre = a.nombre;
                p.usuario = a.usuario;
                p.porcentajePositivo = Convert.ToDecimal(a.porcentajePositivo);
                p.generarPorcentajeNegativo();

                propu.Add(p);
            }
            return propu;
        }

        public int TamListaInsPorId(int idPropuesta)
        {
            using (Entities ctx = new Entities())
            {
                int cant = (from p in ctx.Propuestas
                            join pi in ctx.PropuestasDonacionesInsumos
                            on p.IdPropuesta equals pi.IdPropuesta
                            where pi.IdPropuesta == idPropuesta
                            select pi
                         ).Count();
                return cant;
            }
        }

        public List<PropuestasDonacionesInsumos> CrearListaPropuestasInsumos(int cant)
        {
            List<PropuestasDonacionesInsumos> list = new List<PropuestasDonacionesInsumos>();
            for (int i = 0; i < cant; i++)
            {
                list.Add(
                    new PropuestasDonacionesInsumos()
                    {
                    }
                    );
            }
            return list;
        }

        public int TotalPropuestaIns(int id)
        {
            int contTotal = 0;
            Entities ctx = new Entities();
            var plist = (from p in ctx.PropuestasDonacionesInsumos
                         where p.IdPropuestaDonacionInsumo == id
                         select p.Cantidad
                             ).ToList();
            if (plist.Count > 0)
            {
                foreach (int item in plist)
                {
                    contTotal += item;
                }
                return contTotal;
            }
            else return 0;
        }
        public int TotalPropuestaHrs(int id)
        {
            Entities ctx = new Entities();
            var v = (from p in ctx.PropuestasDonacionesHorasTrabajo
                     where p.IdPropuesta == id
                     select p.CantidadHoras
                             ).First();

            return v;
        }
        public decimal TotalPropuestaMon(int id)
        {
            Entities ctx = new Entities();
            var v = (from p in ctx.PropuestasDonacionesMonetarias
                     where p.IdPropuesta == id
                     select p.Dinero
                             ).First();

            return v;
        }
        public int CalcularTotalDonadoPropuestaIns(int id)
        {
            Entities ctx = new Entities();
            int contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesInsumos
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesInsumos
                          on p_in.IdPropuestaDonacionInsumo equals d_in.IdPropuestaDonacionInsumo
                         where p_in.IdPropuestaDonacionInsumo == id
                         select d_in.Cantidad
                             ).ToList();

            
                foreach (int item in dlist)
                {
                    contTotal += item;
                }
                return contTotal;
            
        }
        public int CalcularTotalDonadoPropuestaHrs(int id)
        {
            Entities ctx = new Entities();

            int contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesHorasTrabajo
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesHorasTrabajo
                          on p_in.IdPropuestaDonacionHorasTrabajo equals d_in.IdPropuestaDonacionHorasTrabajo
                         where p.IdPropuesta == id
                         select d_in.Cantidad
                             ).ToList();

            if (dlist.Count > 0)
            {
                foreach (int item in dlist)
                {
                    contTotal += item;
                }
                return contTotal;
            }
            else return 0;
        }
        public decimal CalcularTotalDonadoPropuestaMon(int id)
        {
            Entities ctx = new Entities();

            decimal contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesMonetarias
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesMonetarias
                          on p_in.IdPropuestaDonacionMonetaria equals d_in.IdPropuestaDonacionMonetaria
                         where p.IdPropuesta == id
                         select d_in.Dinero
                          ).ToList();
            if (dlist.Count > 0)
            {
                foreach (decimal item in dlist)
                {
                    contTotal += item;
                }
                return contTotal;
            }
            else return 0;
        }
        public int IdPropuestaMonetaria(int id)
        {
            using (var ctx = new Entities())
            {
                int ide = (from p in ctx.Propuestas
                           join p_m in ctx.PropuestasDonacionesMonetarias
                           on p.IdPropuesta equals p_m.IdPropuesta
                           select p_m.IdPropuestaDonacionMonetaria).First();
                return ide;
            }


        }
        public int IdPropuestaInsumos(int id)
        {
            using (var ctx = new Entities())
            {
                int ide = (from p in ctx.Propuestas
                           join p_m in ctx.PropuestasDonacionesInsumos
                           on p.IdPropuesta equals p_m.IdPropuesta
                           select p_m.IdPropuestaDonacionInsumo).First();
                return ide;
            }


        }
        public int IdPropuestaHoras(int id)
        {
            using (var ctx = new Entities())
            {
                int ide = (from p in ctx.Propuestas
                           join p_m in ctx.PropuestasDonacionesHorasTrabajo
                           on p.IdPropuesta equals p_m.IdPropuesta
                           select p_m.IdPropuestaDonacionHorasTrabajo).First();
                return ide;
            }


        }
        public string RetornarProfesionPorIdPropuesta(int id)
        {
            using (var ctx = new Entities())
            {
                string pro = (from p in ctx.Propuestas
                              join p_m in ctx.PropuestasDonacionesHorasTrabajo
                              on p.IdPropuesta equals p_m.IdPropuesta
                              where p.IdPropuesta == id
                              select p_m.Profesion).First();
                return pro;
            }
        }
        public void ModificarPropuesta(PropuestaAux p, HttpPostedFileBase Foto)
        {

            using (var ctx = new Entities())
            {
                //1->Monetaria   2->Insumos   3->HorasDeTrabajo

                Propuestas pr = (from prop in ctx.Propuestas
                                 where prop.IdPropuesta == p.IdPropuesta
                                 select prop).First();

                pr.Descripcion = p.Descripcion;
                pr.FechaFin = p.FechaFin;
                pr.Nombre = p.Nombre;
                pr.TelefonoContacto = p.TelefonoContacto;
                pr.PropuestasReferencias.First().Telefono=p.Telefono1;
                pr.PropuestasReferencias.First().Nombre=p.NombreRef1;
                pr.PropuestasReferencias.Skip(1).First().Telefono=p.Telefono1;
                pr.PropuestasReferencias.Skip(1).First().Nombre=p.Telefono2;

                if (p.TipoDonacion == (int)EnumTipoDonacion.Monetaria)
                {
                    pr.PropuestasDonacionesMonetarias.First().CBU = p.CBU;
                    pr.PropuestasDonacionesMonetarias.First().Dinero = p.Dinero;
                }
                if (p.TipoDonacion == (int)EnumTipoDonacion.Insumo)
                {
                    for (int i = 0; i < p.pins.Count; i++)
                    {
                        pr.PropuestasDonacionesInsumos.ToList()[i].Cantidad = p.pins[i].Cantidad;
                        pr.PropuestasDonacionesInsumos.ToList()[i].Nombre = p.pins[i].Nombre;
                    }

                }
                if (p.TipoDonacion == (int)EnumTipoDonacion.HorasTrabajo)
                {
                    pr.PropuestasDonacionesHorasTrabajo.First().Profesion = p.Profesion;
                    pr.PropuestasDonacionesHorasTrabajo.First().CantidadHoras = p.CantidadHoras;
                }
                if (Foto!=null)
                {
                    var filename = Path.GetFileName(Foto.FileName);
                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/"), filename);
                    Foto.SaveAs(path);
                    pr.Foto = filename;
                }


                ctx.SaveChanges();

            }
        }
        public Propuestas GetPorId(int id)
        {
            using (Entities ctx = new Entities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var p = (from pr in ctx.Propuestas.Include("PropuestasDonacionesHorasTrabajo").Include("PropuestasDonacionesInsumos").Include("PropuestasDonacionesMonetarias")
                         where pr.IdPropuesta == id
                         select pr).FirstOrDefault();
                return p;
            }
        }


        public Propuestas GetWithFKPorId(int id)
        {
            Propuestas p = (from pr in asd.Propuestas
                            where pr.IdPropuesta == id
                            select pr).FirstOrDefault();
            return p;
        }
        public int ObtenerIddonacionHorasTrabajo(int id)
        {
            int p = (from pr in asd.PropuestasDonacionesHorasTrabajo
                     where pr.IdPropuesta == id
                     select pr.IdPropuestaDonacionHorasTrabajo).FirstOrDefault();
            return p;
        }
        public int GetHorasTrabajoByPropuesta(int id)
        {
            using (var ctx = new Entities())
            {
                int p = (from don in ctx.PropuestasDonacionesHorasTrabajo
                         where don.IdPropuesta == id
                         select don.CantidadHoras).FirstOrDefault();
                return p;
            }

        }
        public int GetDonacionesHorasTrabajo(int id)
        {
            var p = (from pr in asd.DonacionesHorasTrabajo
                     where pr.IdPropuestaDonacionHorasTrabajo == id
                     select pr.Cantidad).Count();

            int p2;

            if (p == 0)
            {
                p2 = 0;
            }
            else
            {
                p2 = (from pr in asd.DonacionesHorasTrabajo
                         where pr.IdPropuestaDonacionHorasTrabajo == id
                         select pr.Cantidad).Sum();
            }
            return p2;
        }


        public int ObtenerIddonacionMonetaria(int id)
        {
            int p = (from pr in asd.PropuestasDonacionesMonetarias
                     where pr.IdPropuesta == id
                     select pr.IdPropuestaDonacionMonetaria).FirstOrDefault();
            return p;
        }
        public decimal GetMonetariasByPropuesta(int id)
        {
            using (var ctx = new Entities())
            {
                decimal p = (from don in ctx.PropuestasDonacionesMonetarias
                             where don.IdPropuesta == id
                             select don.Dinero).FirstOrDefault();
                return p;
            }

        }
        public decimal GetDonacionesMonetarias(int id)
        {

            var p = (from pr in asd.DonacionesMonetarias
                     where pr.IdPropuestaDonacionMonetaria == id
                     select pr.Dinero).Count();

            decimal p2;

            if (p == 0)
            {
                p2 = 0;
            }
            else
            {
                p2 = (from pr in asd.DonacionesMonetarias
                          where pr.IdPropuestaDonacionMonetaria == id
                          select pr.Dinero).Sum();
            }

            return p2;
        }


        public List<PropuestasDonacionesInsumos> GetInsumosByPropuesta(int id)
        {
            List<PropuestasDonacionesInsumos> p = (from pr in asd.PropuestasDonacionesInsumos
                                                   where pr.IdPropuesta == id
                                                   select pr).ToList();


            return p;
        }
        public List<int> ObtenerIdDonacionesInsumos(int id)
        {
            List<int> p = (from d in asd.PropuestasDonacionesInsumos
                           where d.IdPropuesta == id
                           select d.IdPropuestaDonacionInsumo).ToList();
            return p;
        }
        public List<int> GetDonacionesInsumos(List<int> id)
        {
            List<int> h = new List<int>();

            foreach (var a in id)
            {
                int p = (from don in asd.DonacionesInsumos
                         where don.IdPropuestaDonacionInsumo == a
                         select don.Cantidad).Count();


                if (p != 0)
                {
                    int p2 = (from don in asd.DonacionesInsumos
                              where don.IdPropuestaDonacionInsumo == a
                              select don.Cantidad).Sum();

                    h.Add(p2);
                }
                else
                {
                    h.Add(0);
                }
            }

            return h;

        }
        public List<int> GetTotalesInsumos(List<PropuestasDonacionesInsumos> prop, List<int> donacion, List<int> id, int idProp)
        {
            List<int> total = new List<int>();
            int i = -1;
            int asdd = -1;

            foreach (var a in id)
            {
                int p = (from don in asd.DonacionesInsumos
                         where don.IdPropuestaDonacionInsumo == a
                         select don.Cantidad).Count();

                if (p != 0)
                {
                    i++;

                    int p2 = (from don in asd.DonacionesInsumos
                              where don.IdPropuestaDonacionInsumo == a
                              select don.Cantidad).Sum();

               List<int> p3 = (from pepe in asd.PropuestasDonacionesInsumos
                              where pepe.IdPropuesta == idProp
                              select pepe.Cantidad).ToList();



                    total.Add(p3[i] - p2);

                }
                else
                {
                    asdd++;

                    List <int> p3 = (from pepe in asd.PropuestasDonacionesInsumos
                              where pepe.IdPropuesta == idProp
                              select pepe.Cantidad).ToList();

                    total.Add(p3[asdd]);
                }
            }

            return total;
        }
        public Usuarios GetUsuarioPorPropuesta(int id)
        {
            using (var ctx = new Entities())
            {
                Usuarios p = (from pr in ctx.Usuarios
                              join prop in ctx.Propuestas
                              on pr.IdUsuario equals prop.IdUsuarioCreador
                              where prop.IdPropuesta == id
                              select pr).FirstOrDefault();

                return p;
            }
        }

        public int ValoracionesPositivas(int id)
        {
            using (var ctx = new Entities())
            {
                int idval = (from pv in ctx.PropuestasValoraciones
                             where pv.IdPropuesta == id
                             && pv.Valoracion == true
                             select pv.Valoracion).Count();
                return idval;
            }
        }
        public int ValoracionesNegativas(int id)
        {
            using (var ctx = new Entities())
            {
                int idval = (from pv in ctx.PropuestasValoraciones
                             where pv.IdPropuesta == id
                             && pv.Valoracion == false
                             select pv.Valoracion).Count();
                return idval;
            }
        }
        public decimal CalcularPorcentajePositivo(int positivo, int negativo)
        {
            if (negativo != 0 && positivo != 0)
            {
                var total = positivo + negativo;
                total = positivo * 100 / total;
                return total;
            }
            else if (negativo == 0 && positivo != 0)
            {
                return 100;
            }
            else if (negativo != 0 && positivo == 0)
            {
                return 200;
            }
            else
            {
                return 50;
            }
        }
        public void CantidadTotalVotos(int positivo, int negativo, int id)
        {
            Propuestas prop = (from p in asd.Propuestas
                               where p.IdPropuesta == id
                               select p).FirstOrDefault();

            int total2 = negativo + positivo;
            int total;

            if (total2 != 0)
            {
                total = positivo * 100 / total2;
            }
            else
            {
                total = 50;
            }

            prop.Valoracion = total;

            asd.SaveChanges();
        }

        public void CrearValoracion(int IdUser, int IdPropuesta, int valoracion)
        {
            PropuestasValoraciones valor = new PropuestasValoraciones();

            using (var ctx = new Entities())
            {
                int verSiVoto = (from vot in ctx.PropuestasValoraciones
                                 where vot.IdUsuario == IdUser
                                 && vot.IdPropuesta == IdPropuesta
                                 select vot.IdUsuario).Count();

                PropuestasValoraciones valorCambio = (from vot in ctx.PropuestasValoraciones
                                                      where vot.IdUsuario == IdUser
                                                      && vot.IdPropuesta == IdPropuesta
                                                      select vot).FirstOrDefault();

                if (verSiVoto != 0)
                {
                    if (valoracion == 1)
                    {
                        valorCambio.Valoracion = true;
                    }
                    else
                    {
                        valorCambio.Valoracion = false;
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    valor.IdUsuario = IdUser;
                    valor.IdPropuesta = IdPropuesta;
                    if (valoracion == 1)
                    {
                        valor.Valoracion = true;
                    }
                    else
                    {
                        valor.Valoracion = false;
                    }
                    ctx.PropuestasValoraciones.Add(valor);
                    ctx.SaveChanges();
                }

            }
        }
        public int PuedeDenunciar(int idPropuesta, string IdUser)
        {

            using (var ctx = new Entities())
            {
                int idInt2 = Convert.ToInt32(IdUser);

                int valor = (from d in ctx.Denuncias
                             where d.IdUsuario == idInt2
                             && d.IdPropuesta == idPropuesta
                             select d.IdUsuario).Count();

                return valor;
            }
        }
        


        public void VerificarDenuncias(Denuncias denuncia)
        {
            int contador = (from p in asd.Denuncias
                            where p.IdPropuesta == denuncia.IdPropuesta
                            && p.Estado == 1
                            select p).Count();




            Propuestas prop = (from p2 in asd.Propuestas
                               where p2.IdPropuesta == denuncia.IdPropuesta
                               select p2).FirstOrDefault();


            if (contador >= 5)
            {
                prop.Estado = 1;
                asd.SaveChanges();
            }
        }
        public int GetVotoPorId(String id, int idPropuesta)
        {
            int idInt = Convert.ToInt32(id);

            using (var ctx = new Entities())
            {
                int valor = (from pv in ctx.PropuestasValoraciones
                             where pv.IdUsuario == idInt
                             && pv.IdPropuesta == idPropuesta
                             select pv.Valoracion).Count();

                Boolean valor2 = (from pv in ctx.PropuestasValoraciones
                                  where pv.IdUsuario == idInt
                                  && pv.IdPropuesta == idPropuesta
                                  select pv.Valoracion).FirstOrDefault();

                if (valor != 0 && valor2 == true)
                {
                    return 1;
                }
                else if (valor != 0 && valor2 == false)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

    }
}
