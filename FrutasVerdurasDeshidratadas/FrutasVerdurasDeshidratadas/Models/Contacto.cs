using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Contacto
    {
        public int Id_Contacto { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Dirección de Correo electrónico incorrecta, por favor chéquela y corríjala.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [DataType(DataType.EmailAddress)]
        public string eMail { get; set; }

        [Display(Name = "Mensaje")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El texto del mensaje es requerido.")]
        public string TextoMensaje { get; set; }
        public System.DateTime FechaContacto { get; set; }
    }
}