using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class PropuestaWrapper
    {
        public Propuestas Propuesta { get; set; }
        public Usuarios Usuario { get; set; }

        public int PropuestasDonacionesHorasTrabajo { get; set; }
        public int GetDonacionesHorasTrabajo { get; set; }
        public int DonacionesHorasFaltantes { get; set; }

        public decimal PropuestasDonacionesMonetarias { get; set; }
        public decimal GetDonacionesMonetarias { get; set; }
        public decimal DonacionesMonetariasFaltantes { get; set; }

        public List<PropuestasDonacionesInsumos> PropuestasDonacionesInsumos { get; set; }
        public List<int> GetDonacionesInsumos { get; set; }
        public List<int> TotalesDonacionesInsumos { get; set; }
        public int Denuncia { get; set; }
        public decimal porcentajePositivo { get; set; }
        public decimal porcentajeNegativo { get; set; }
        public int VotosPositivos { get; set; }
        public int VotosNegativos { get; set; }
        public int voto { get; set; }

        public int idLogeado { get; set; }
        public PropuestaWrapper()
        {

        }

    }
}
