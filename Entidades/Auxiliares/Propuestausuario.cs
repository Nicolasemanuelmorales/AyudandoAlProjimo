using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Auxiliares
{
    public class PropuestaUsuario
    {

        public string nombre { get; set; }
        public string usuario { get; set; }
        public string foto { get; set; }
        public DateTime fechaFin { get; set; }
        public decimal porcentajePositivo { get; set; }
        public decimal porcentajeNegativo { get; set; }
        public int idPropuesta { get; set; }



        public void generarPorcentajeNegativo()
        {

            this.porcentajeNegativo = 100 - this.porcentajePositivo;
        }


    }
}
