﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MXLogsEntities : DbContext
    {
        public MXLogsEntities()
            : base("name=MXLogsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdTitleTemplateLog> AdTitleTemplateLogs { get; set; }
        public virtual DbSet<AgencyLog> AgencyLogs { get; set; }
        public virtual DbSet<BranchLog> BranchLogs { get; set; }
        public virtual DbSet<BrowserAgent> BrowserAgents { get; set; }
        public virtual DbSet<BuyPackageMessage> BuyPackageMessages { get; set; }
        public virtual DbSet<EmailLeadLog> EmailLeadLogs { get; set; }
        public virtual DbSet<GroupLog> GroupLogs { get; set; }
        public virtual DbSet<ImageLog> ImageLogs { get; set; }
        public virtual DbSet<LeadDeleiveryAddressLog> LeadDeleiveryAddressLogs { get; set; }
        public virtual DbSet<LeadDeliveryLog> LeadDeliveryLogs { get; set; }
        public virtual DbSet<OneTimePassword> OneTimePasswords { get; set; }
        public virtual DbSet<OverlayPreferenceLog> OverlayPreferenceLogs { get; set; }
        public virtual DbSet<PhoneTrackingLog> PhoneTrackingLogs { get; set; }
        public virtual DbSet<PropertyActivityLog> PropertyActivityLogs { get; set; }
        public virtual DbSet<PropertyDailyActivity> PropertyDailyActivities { get; set; }
        public virtual DbSet<PropertySubmitLog> PropertySubmitLogs { get; set; }
        public virtual DbSet<SignupRequest> SignupRequests { get; set; }
        public virtual DbSet<SuperAdminLog> SuperAdminLogs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
        public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }
        public virtual DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<WorldPayNotificationLog> WorldPayNotificationLogs { get; set; }
    
        public virtual ObjectResult<Chart_Get_Agency_PageViews_PhoneClicks_LeadActivities_Result> Chart_Get_Agency_PageViews_PhoneClicks_LeadActivities(Nullable<int> agencyID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> fromYear, Nullable<int> toYear)
        {
            var agencyIDParameter = agencyID.HasValue ?
                new ObjectParameter("AgencyID", agencyID) :
                new ObjectParameter("AgencyID", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var fromYearParameter = fromYear.HasValue ?
                new ObjectParameter("FromYear", fromYear) :
                new ObjectParameter("FromYear", typeof(int));
    
            var toYearParameter = toYear.HasValue ?
                new ObjectParameter("ToYear", toYear) :
                new ObjectParameter("ToYear", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Chart_Get_Agency_PageViews_PhoneClicks_LeadActivities_Result>("Chart_Get_Agency_PageViews_PhoneClicks_LeadActivities", agencyIDParameter, fromDateParameter, toDateParameter, fromYearParameter, toYearParameter);
        }
    
        public virtual int Chart_Get_Branch_PageViews_PhoneClicks_LeadActivities(Nullable<int> agencyID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> fromYear, Nullable<int> toYear)
        {
            var agencyIDParameter = agencyID.HasValue ?
                new ObjectParameter("AgencyID", agencyID) :
                new ObjectParameter("AgencyID", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var fromYearParameter = fromYear.HasValue ?
                new ObjectParameter("FromYear", fromYear) :
                new ObjectParameter("FromYear", typeof(int));
    
            var toYearParameter = toYear.HasValue ?
                new ObjectParameter("ToYear", toYear) :
                new ObjectParameter("ToYear", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Chart_Get_Branch_PageViews_PhoneClicks_LeadActivities", agencyIDParameter, fromDateParameter, toDateParameter, fromYearParameter, toYearParameter);
        }
    
        public virtual int Chart_Get_User_PageViews_PhoneClicks_LeadActivities(Nullable<int> agencyID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> fromYear, Nullable<int> toYear)
        {
            var agencyIDParameter = agencyID.HasValue ?
                new ObjectParameter("AgencyID", agencyID) :
                new ObjectParameter("AgencyID", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var fromYearParameter = fromYear.HasValue ?
                new ObjectParameter("FromYear", fromYear) :
                new ObjectParameter("FromYear", typeof(int));
    
            var toYearParameter = toYear.HasValue ?
                new ObjectParameter("ToYear", toYear) :
                new ObjectParameter("ToYear", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Chart_Get_User_PageViews_PhoneClicks_LeadActivities", agencyIDParameter, fromDateParameter, toDateParameter, fromYearParameter, toYearParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<SystemHealth_CountActivitiesByDateRange_Result> SystemHealth_CountActivitiesByDateRange(Nullable<System.DateTime> dateStart, Nullable<System.DateTime> dateEnd)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            var dateEndParameter = dateEnd.HasValue ?
                new ObjectParameter("DateEnd", dateEnd) :
                new ObjectParameter("DateEnd", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_CountActivitiesByDateRange_Result>("SystemHealth_CountActivitiesByDateRange", dateStartParameter, dateEndParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SystemHealth_CountLastSubmittedAds(Nullable<System.DateTime> dateStart, Nullable<byte> submitType)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            var submitTypeParameter = submitType.HasValue ?
                new ObjectParameter("SubmitType", submitType) :
                new ObjectParameter("SubmitType", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SystemHealth_CountLastSubmittedAds", dateStartParameter, submitTypeParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetActivityDailyReport_Result> SystemHealth_GetActivityDailyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetActivityDailyReport_Result>("SystemHealth_GetActivityDailyReport", dateStartParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetActivityMonthlyReport_Result> SystemHealth_GetActivityMonthlyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetActivityMonthlyReport_Result>("SystemHealth_GetActivityMonthlyReport", dateStartParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetBumpUpDailyReport_Result> SystemHealth_GetBumpUpDailyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetBumpUpDailyReport_Result>("SystemHealth_GetBumpUpDailyReport", dateStartParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetBumpUpHourlyReport_Result> SystemHealth_GetBumpUpHourlyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetBumpUpHourlyReport_Result>("SystemHealth_GetBumpUpHourlyReport", dateStartParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetPostEngineDailyReport_Result> SystemHealth_GetPostEngineDailyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetPostEngineDailyReport_Result>("SystemHealth_GetPostEngineDailyReport", dateStartParameter);
        }
    
        public virtual ObjectResult<SystemHealth_GetPostEngineHourlyReport_Result> SystemHealth_GetPostEngineHourlyReport(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("DateStart", dateStart) :
                new ObjectParameter("DateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemHealth_GetPostEngineHourlyReport_Result>("SystemHealth_GetPostEngineHourlyReport", dateStartParameter);
        }
    }
}
