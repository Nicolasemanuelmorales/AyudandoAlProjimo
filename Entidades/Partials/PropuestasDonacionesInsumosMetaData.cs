using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class PropuestasDonacionesInsumosMetaData
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser como maximo 50 caracteres")
        , MinLength(2, ErrorMessage = "El nombre debe ser como minimo dos caracteres")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "La cantidad de insumos es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de insumos debe ser mayor a 1")]
        public int Cantidad { get; set; }
    }
}
