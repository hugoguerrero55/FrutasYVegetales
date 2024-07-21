using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Envios
    {
        public Nullable<int> MemberId { get; set; }

        [Display(Name = "Dirección Calle/Número/Colonia")]
        public string AddressLine { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Municipio")]
        public string Municipio { get; set; }

        [Display(Name = "Código Postal")]
        public string ZipCode { get; set; }

        [Display(Name = "Folio-Orden")]
        public string OrderId { get; set; }

        [Display(Name = "Importe Total")]
        public Nullable<decimal> AmountPaid { get; set; }

        [Display(Name = "Número de Transacción")]
        public string TransactionID { get; set; }
        public Nullable<System.DateTime> OrderPlacedDate { get; set; }
    }
}