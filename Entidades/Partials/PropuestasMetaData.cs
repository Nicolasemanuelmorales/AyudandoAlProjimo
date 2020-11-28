using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class PropuestasMetadata
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser como maximo 50 caracteres")
        , MinLength(2, ErrorMessage = "El nombre debe ser como minimo dos caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es requerida")]
        [MaxLength(280, ErrorMessage = "La descripcion debe ser como maximo 280 caracteres")
        , MinLength(2, ErrorMessage = "La descripcion debe ser como minimo 2 caracteres")]
        //[StringLength(50, ErrorMessage = "El nombre debe ser menor a 50 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La Fecha de Fin es requerida")]
        [DataType(DataType.DateTime, ErrorMessage = "La fecha es invalida")]
        public System.DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El telefono de contacto")]
        [RegularExpression("([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*)", ErrorMessage = "Telefono invalido")]
        public string TelefonoContacto { get; set; }
    }
}