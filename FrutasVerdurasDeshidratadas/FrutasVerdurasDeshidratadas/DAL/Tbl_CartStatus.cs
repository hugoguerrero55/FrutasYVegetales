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
    
    public partial class Tbl_CartStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_CartStatus()
        {
            this.Tbl_ShoppingCart = new HashSet<Tbl_ShoppingCart>();
        }
    
        public int CartStatusId { get; set; }
        public string CartStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ShoppingCart> Tbl_ShoppingCart { get; set; }
    }
}