//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeekBooks.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Creditcard
    {
        public string CreditcardNumber { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public byte CCV { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<int> FK_CCUserID { get; set; }
    
        public virtual User User { get; set; }
    }
}
