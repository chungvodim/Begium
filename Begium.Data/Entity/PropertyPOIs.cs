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
    
    public partial class PropertyPOIs
    {
        public int PropertyPOIID { get; set; }
        public int PropertyID { get; set; }
        public string Schools { get; set; }
        public string Hospitals { get; set; }
        public string Restaurants { get; set; }
        public string Vicinity { get; set; }
        public string Transit { get; set; }
        public string RecentSales { get; set; }
        public bool IsExcludeSchool { get; set; }
        public bool IsExcludeHospital { get; set; }
        public bool IsExcludeRestaurant { get; set; }
        public bool IsExcludeVicinity { get; set; }
    }
}
