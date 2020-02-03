namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlagsForDataTypesUpdateView : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[DNS3_DataMarts]
AS
SELECT        d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, d.ContactPhone, d.SpecialRequirements, 
                         d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.Description, d.Acronym, d.DataMartDescription, d.StartDate, d.EndDate, 
                         d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, d.InpatientClaims, d.OutpatientClaims, 
                         d.OutpatientPharmacyClaims, d.Registries, d.DataModel, d.OtherDataModel, d.SID, d.IsLocal, d.EnrollmentClaims, d.DemographicsClaims, d.LaboratoryResultsClaims, d.VitalSignsClaims, d.OtherClaims, 
                         CAST(CASE WHEN ISNULL(d .isdeleted, 0) = 1 OR
                         ISNULL(o.isdeleted, 0) = 1 THEN 1 ELSE 0 END AS bit) AS EffectiveIsDeleted, d.LaboratoryResultsTestName, d.LaboratoryResultsDates, d.LaboratoryResultsTestLOINC, d.LaboratoryResultsTestSNOMED, 
                         d.LaboratoryResultsTestResultsInterpretation, d.LaboratoryResultsTestOther, d.LaboratoryResultsTestOtherText, d.BiorepositoriesClaims, d.PatientReportedOutcomesClaims, d.PatientReportedBehaviorsClaims, 
                         d.PrescriptionOrdersClaims, d.PharmacyDispensingClaims
FROM            dbo.DataMarts AS d INNER JOIN
                         dbo.Organizations AS o ON d.OrganizationId = o.OrganizationId
");
        }
        
        public override void Down()
        {
        }
    }
}
