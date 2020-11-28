using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System;
using System.Web;


namespace Servicios
{
    public class DenunciasService
    {
        Entities asd = new Entities();
        public void DesestimarDenuncia(int u)
        {
            var usua = (from p in asd.Denuncias
                        where p.IdDenuncia == u
                        select p).ToList();

            Denuncias usu2 = new Denuncias();
            foreach (var a in usua)
            {
                usu2 = a;
            }
            usu2.Estado = 0;
            asd.SaveChanges();

            var usua3 = (from p in asd.Denuncias
                        where p.IdPropuesta == usu2.IdPropuesta
                        && p.Estado==0
                        select p).ToList();

            int cont = usua3.Count();

            if (cont < 5)
            {
                var usua5 = (from p in asd.Propuestas
                             where p.IdPropuesta == usu2.IdPropuesta
                             select p).ToList();

                Propuestas usu4 = new Propuestas();
                foreach (var a in usua5)
                {
                    usu4 = a;
                }
                usu4.Estado = 0;
                asd.SaveChanges();
            }
        }

        public void AceptarDenuncia(int u)
        {
            var usua = (from p in asd.Denuncias
                        where p.IdDenuncia == u
                        select p).ToList();

            Denuncias usu2 = new Denuncias();
            foreach (var a in usua)
            {
                a.Estado = 0;
                usu2 = a;
            }

            var usua3 = (from p in asd.Propuestas
                         where p.IdPropuesta == usu2.IdPropuesta
                         select p).ToList();

            Propuestas usu4 = new Propuestas();
            foreach (var a in usua3)
            {
                usu4 = a;
            }

            usu4.Estado = 1;

            var usua5 = (from p in asd.Denuncias
                         where p.IdPropuesta == usu4.IdPropuesta
                         select p).ToList();

            Denuncias usu6 = new Denuncias();
            foreach (var a in usua5)
            {
                usu6 = a;
                usu6.Estado = 0;
                asd.SaveChanges();
            }
            asd.SaveChanges();
        }

        public void Denunciar(Denuncias denuncia)
        {

            using (var ctx = new Entities())
            {
                Denuncias den = new Denuncias();

                den.IdUsuario = denuncia.IdUsuario;
                den.IdPropuesta = denuncia.IdPropuesta;
                den.FechaCreacion = DateTime.Today;
                den.IdMotivo = denuncia.IdMotivo;
                den.Estado = 1;
                den.Comentarios = denuncia.Comentarios;

                ctx.Denuncias.Add(den);
                ctx.SaveChanges();
                DenunciasService pp = new DenunciasService();
                pp.VerificarDenuncias(denuncia);


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
    }

}
