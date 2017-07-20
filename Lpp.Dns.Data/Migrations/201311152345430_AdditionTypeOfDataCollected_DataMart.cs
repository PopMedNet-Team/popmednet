namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionTypeOfDataCollected_DataMart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "EnrollmentClaims", c => c.Boolean());
            AddColumn("dbo.DataMarts", "DemographicsClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsClaims", c => c.Boolean());
            AddColumn("dbo.DataMarts", "VitalSignsClaims", c => c.Boolean());
            AddColumn("dbo.DataMarts", "OtherClaims", c => c.String(maxLength: 80));

            //re-create the view with the additional columns
            Sql("DROP VIEW DNS3_DataMarts");

            Sql(@"CREATE VIEW [dbo].[DNS3_DataMarts]
AS
SELECT     d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, 
               d.ContactPhone, d.SpecialRequirements, d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.[Description], d.Acronym, 
               d.StartDate, d.EndDate, d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, 
               d.InpatientClaims, d.OutpatientClaims, d.OutpatientPharmacyClaims, d.Registries, d.DataModel, d.OtherDataModel,
			   d.[SID], d.IsLocal, d.EnrollmentClaims, d.DemographicsClaims, d.LaboratoryResultsClaims, d.VitalSignsClaims, d.OtherClaims,
		   CAST( CASE WHEN ISNULL(d.isdeleted,0) = 1 OR ISNULL(o.isdeleted,0) = 1 THEN 1 ELSE 0 END AS bit) AS EffectiveIsDeleted
    FROM         dbo.DataMarts d
      INNER JOIN Organizations o ON d.organizationid = o.organizationid");
                
        }
        
        public override void Down()
        {
            //re-create the view without the new columns
            Sql("DROP VIEW DNS3_DataMarts");

            Sql(@"CREATE VIEW [dbo].[DNS3_DataMarts]
AS
SELECT     d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, 
               d.ContactPhone, d.SpecialRequirements, d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.[Description], d.Acronym, 
               d.StartDate, d.EndDate, d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, 
               d.InpatientClaims, d.OutpatientClaims, d.OutpatientPharmacyClaims, d.Registries, d.DataModel, d.OtherDataModel,
			   d.[SID], d.IsLocal,
		   CAST( CASE WHEN ISNULL(d.isdeleted,0) = 1 OR ISNULL(o.isdeleted,0) = 1 THEN 1 ELSE 0 END AS bit) AS EffectiveIsDeleted
    FROM         dbo.DataMarts d
      INNER JOIN Organizations o ON d.organizationid = o.organizationid");

            DropColumn("dbo.DataMarts", "OtherClaims");
            DropColumn("dbo.DataMarts", "VitalSignsClaims");
            DropColumn("dbo.DataMarts", "LaboratoryResultsClaims");
            DropColumn("dbo.DataMarts", "DemographicsClaims");
            DropColumn("dbo.DataMarts", "EnrollmentClaims");
        }
    }
}
