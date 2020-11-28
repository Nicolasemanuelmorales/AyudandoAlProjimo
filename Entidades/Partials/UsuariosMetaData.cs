using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class UsuariosMetaData
    {
        [MaxLength(50, ErrorMessage = "50 caracteres como Maximo")]
        public string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = "50 caracteres como Maximo")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La Fecha de Nacimiento es obligatorio")]
        [DataType(DataType.Date)]
        public System.DateTime FechaNacimiento { get; set; }

        [MaxLength(20, ErrorMessage = "20 caracteres como Maximo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [MaxLength(50, ErrorMessage = "50 caracteres como Maximo")]
        [EmailAddress(ErrorMessage = "Formato valido: ejemplo@ejemplo.com.ar")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [RegularExpression(@"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
        ErrorMessage = "Minimo 1 mayúscula, 1 número y 8 caracteres")]
        [MaxLength(20, ErrorMessage = "20 caracteres como Maximo")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(100, ErrorMessage = "100 caracteres como Maximo")]
        public string Foto { get; set; }

        [MaxLength(50, ErrorMessage = "50 caracteres como Maximo")]
        public string Token { get; set; }
    }
}
