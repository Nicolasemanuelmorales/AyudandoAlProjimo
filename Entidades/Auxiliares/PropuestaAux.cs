using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entidades.Auxiliares
{
    public  class PropuestaAux:Propuestas
    {   
        public PropuestaAux()
        {
            CantidadHoras = 0;
            Profesion = "0";
            CantidadIns = 0;
            pins = new List<PropuestasDonacionesInsumos>();
            Dinero = 0;
            CBU = "0";
        }
        //Horas de Trabajo
        [Required(ErrorMessage = "La cantidad de horas es requerida")]
        public int CantidadHoras { get; set; }

        [MaxLength(50)
       , MinLength(0)]
        public string Profesion { get; set; }
        //Horas de Trabajo
        //Insumos

        [Required(ErrorMessage = "La cantidad de insumos es requerida")]
        public int CantidadIns { get; set; }
        public List<PropuestasDonacionesInsumos> pins { get; set; }
        //Insumos
        //Monetaria
        [Required(ErrorMessage = "El dinero es requerido")]
        public decimal Dinero { get; set; }
        [Required(ErrorMessage = "El CBU es requerido")]
        public string CBU { get; set; }
        //Monetaria

        [Required(ErrorMessage = "El telefono de es requerido")]
        [RegularExpression("([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*)", ErrorMessage = "Telefono invalido")]
        public string Telefono1{ get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser como maximo 50 caracteres")
       , MinLength(2, ErrorMessage = "El nombre debe ser como minimo dos caracteres")]
        public string NombreRef1 { get; set; }
        [Required(ErrorMessage = "El telefono de es requerido")]
        [RegularExpression("([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*)", ErrorMessage = "Telefono invalido")]
        public string Telefono2 { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser como maximo 50 caracteres")
      , MinLength(2, ErrorMessage = "El nombre debe ser como minimo dos caracteres")]
        public string NombreRef2 { get; set; }
    }
}
