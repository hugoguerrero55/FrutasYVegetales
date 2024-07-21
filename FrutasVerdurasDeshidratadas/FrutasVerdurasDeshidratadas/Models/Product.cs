using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrutasVerdurasDeshidratadas.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public string ProdDetailImage { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public Nullable<int> ActualStock { get; set; }
        public Nullable<int> MinStock { get; set; }
        public Nullable<int> MaxStock { get; set; }
    }
}