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
    
    public partial class ImageDownloader_GetPropertySubmitsToDownload_Result
    {
        public int PropertySubmitID { get; set; }
        public Nullable<byte> SubmitStatus { get; set; }
        public short SubmitPriority { get; set; }
        public bool ShowPrice { get; set; }
        public bool ShowOnSchedule { get; set; }
        public bool ShowAddress { get; set; }
        public bool ShowMap { get; set; }
        public bool ShowStreetview { get; set; }
        public System.DateTime DateListing { get; set; }
        public string AdID { get; set; }
        public string AdGuid { get; set; }
        public string AdStatus { get; set; }
        public string AdURL { get; set; }
        public short PageNumber { get; set; }
        public Nullable<System.DateTime> DateCreatedAd { get; set; }
        public Nullable<System.DateTime> DatePostedAd { get; set; }
        public Nullable<System.DateTime> DateUpdatedAd { get; set; }
        public Nullable<System.DateTime> DateTopAd7Start { get; set; }
        public Nullable<System.DateTime> DateTopAd7End { get; set; }
        public Nullable<System.DateTime> DateTopAd31Start { get; set; }
        public Nullable<System.DateTime> DateTopAd31End { get; set; }
        public Nullable<System.DateTime> DateHighlightStart { get; set; }
        public Nullable<System.DateTime> DateHighlightEnd { get; set; }
        public Nullable<System.DateTime> DateHomepageGalleryStart { get; set; }
        public Nullable<System.DateTime> DateHomepageGalleryEnd { get; set; }
        public Nullable<System.DateTime> DateUrgentStart { get; set; }
        public Nullable<System.DateTime> DateUrgentEnd { get; set; }
        public Nullable<System.DateTime> DateDeletedAd { get; set; }
        public Nullable<System.DateTime> DateAdStart { get; set; }
        public Nullable<System.DateTime> DateAdEnd { get; set; }
        public Nullable<System.DateTime> DateScheduledBumpUp { get; set; }
        public int PendingBumpUpBy { get; set; }
        public string EbayImages { get; set; }
        public string Images { get; set; }
        public int LocationID { get; set; }
        public int PropertyID { get; set; }
        public Nullable<System.DateTime> DateLastPolled { get; set; }
        public byte ProcessStatus { get; set; }
        public string FeedImageURLs { get; set; }
        public bool IsImportBlockedImage { get; set; }
        public short FeedImageURLUpdated { get; set; }
        public bool ShowAgentProfileImage { get; set; }
        public Nullable<System.DateTime> DateLastBumpUp { get; set; }
        public Nullable<byte> FeatureActionPending { get; set; }
        public bool HasMigrated { get; set; }
    }
}
