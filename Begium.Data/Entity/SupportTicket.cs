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
    
    public partial class SupportTicket
    {
        public int SupportTicketID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int AgencyID { get; set; }
        public int UserID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}