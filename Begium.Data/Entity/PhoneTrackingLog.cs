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
    
    public partial class PhoneTrackingLog
    {
        public int PhoneTrackingLogID { get; set; }
        public int PhoneTrackingID { get; set; }
        public int BranchID { get; set; }
        public string ForwardFromNumber { get; set; }
        public string ForwardToNumber { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int UserID { get; set; }
        public bool IsSuperAdmin { get; set; }
        public byte PhoneTrackingAction { get; set; }
        public string Description { get; set; }
        public string PhoneFeatures { get; set; }
        public int AgencyUserID { get; set; }
    }
}
