//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Begium.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agency
    {
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string PrimaryContact { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string AgencyNumber { get; set; }
    }
}