//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrutasVerdurasDeshidratadas.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_PayPal_Config
    {
        public int Id_Paypal_Config { get; set; }
        public int Id_Parner { get; set; }
        public string mode { get; set; }
        public string connectionTimeout { get; set; }
        public int requestRetries { get; set; }
        public string divisa { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    
        public virtual Tbl_Partners Tbl_Partners { get; set; }
    }
}
