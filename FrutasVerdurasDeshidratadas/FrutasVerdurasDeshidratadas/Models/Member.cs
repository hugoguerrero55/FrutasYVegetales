using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public partial class Member
    {
        public int _memberId { get; set; }

        [Display(Name = "Nombre(s)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre(s) es requerido")]
        public string _firstName { get; set; }

        [Display(Name = "Apellidos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido(s) es(son) requerido(s)")]
        public string _lastName { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",ErrorMessage = "Dirección de Correo electrónico incorrecta, por favor chéquela y corríjala.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [DataType(DataType.EmailAddress)]
        public string _emailId { get; set; }

        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña es requerida")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Se necesitan al menos 6 caracteres")]
        public string _password { get; set; }

        [Display(Name = "Confirme contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare("_password", ErrorMessage = "La confirmación de la contraseña debe coincidir con el campo 'Contraseña'")]
        public string _confirmPassword { get; set; }

        public bool _isActive { get; set; }

        public bool _isDelete { get; set; }
        public DateTime _createdOn { get; set; }
        public DateTime _modifiedOn { get; set; }
    }
}