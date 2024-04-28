//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChuongTrinhQuanLyKhachSan
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.Payment = new HashSet<Payment>();
        }
    
        public int bookid { get; set; }
        public Nullable<int> staffid { get; set; }
        public Nullable<int> cusid { get; set; }
        public Nullable<int> roomid { get; set; }
        public Nullable<System.DateTime> checkin { get; set; }
        public Nullable<System.DateTime> checkout { get; set; }
        public string bookstatus { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Room Room { get; set; }
        public virtual Staff Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }
    }
}