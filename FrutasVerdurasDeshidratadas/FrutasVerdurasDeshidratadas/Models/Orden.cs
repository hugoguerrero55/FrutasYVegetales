using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Orden
    {

        public Nullable<int> MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string OrderId { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string TransactionID { get; set; }
        public Nullable<System.DateTime> OrderPlacedDate { get; set; }
        public string Municipio { get; set; }

    }
}