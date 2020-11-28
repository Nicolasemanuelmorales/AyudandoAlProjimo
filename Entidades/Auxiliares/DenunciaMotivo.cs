using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DenunciaMotivo
    {
        public int IdDenuncia { get; set; }
        public int IdPropuesta { get; set; }
        public string Comentarios { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public String DescripcionMotivo { get; set; }
    }
}
