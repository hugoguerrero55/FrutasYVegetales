using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Cart
    {
        public int Id_ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public int MemberId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int CartStatusId { get; set; }
        public Nullable<int> ShippingDetailId { get; set; }
    }
}