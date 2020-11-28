using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DonacionAux
    {

        public DateTime FechaDonacion { get; set; }
        public string Nombre { get; set; }
        public int TipoDonacion { get; set; }
        public int Estado { get; set; }
        public decimal TotalRecaudado { get; set; }
        public decimal MiDonacion { get; set; }
        public int IdUsuario { get; set; }
        public int IdPropuestaDIns { get; set; }
        public int IdPropuesta { get; set; }
        public List<PropuestasDonacionesInsumos> listin { get; set; }
    }
}
