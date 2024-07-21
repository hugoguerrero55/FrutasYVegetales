using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Distribuidor
    {
        public int Id_Distribuidor { get; set; }
        public System.DateTime Fecha_Registro { get; set; }
        public int Id_EstatusDist { get; set; }

        [Display(Name = "Estado de la República")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del Estado es requerido.")]
        public string Estado { get; set; }

        [Display(Name = "Cuidad ó Município")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La Cuidad o Municipio es requerida.")]
        public string Ciudad_Municipio { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre completo es requerido.")]
        public string Nombres { get; set; }

        [Display(Name = "Calle y Número")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La calle y número son requeridos.")]
        public string CalleNumero { get; set; }

        [Display(Name = "Colónia")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "la colónia es requerida.")]
        public string Colonia { get; set; }

        [Display(Name = "Código Postal")]
        [MinLength(5, ErrorMessage = "El debe estar compuesto por un máximo de 5 números.")]
        //public Nullable<int> CP { get; set; }
        public string CP { get; set; }

        [Display(Name = "Teléfono")]
        [MinLength(10, ErrorMessage = "El teléfono debe ser de 10 números junto con la clve lada")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono es requerido.")]
        public string Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El eMail es requerido.")]
        public string eMail { get; set; }
        public bool EsProspecto { get; set; }
        public string FolioSolicitud { get; set; }
    }
}