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
    
    public partial class LeadDeliveryLog
    {
        public int LeadDeliveryLogID { get; set; }
        public int LeadID { get; set; }
        public Nullable<System.DateTime> LogTime { get; set; }
        public string LogMessage { get; set; }
        public string ExternalLeadID { get; set; }
        public string ExternalLeadType { get; set; }
    }
}
