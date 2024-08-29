namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrganizationView : DbMigration
    {
        public override void Up()
        {
            Sql(@"  ALTER VIEW DNS3_Organizations AS SELECT
                    OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired,
                    OrganizationAcronym AS Acronym, ParentId, [SID], ContactEmail, ContactFirstName, 
                    ContactLastName, ContactPhone, SpecialRequirements, UsageRestrictions,
                    HealthPlanDescription,ObservationClinicalExperience, InpatientEHRApplication,
                    OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
                    InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registries,
                    ObservationalParticipation, ProspectiveTrials, EnrollmentClaims, DemographicsClaims,
                    LaboratoryResultsClaims, VitalSignsClaims, OtherClaims, EnableClaimsAndBilling, EnableEHRA,
                    DataModelMSCDM, DataModelHMORNVDW, DataModelESP, DataModelI2B2, DataModelOther,
                    [Primary], X509PublicKey, X509PrivateKey
                    FROM dbo.Organizations");
        }
        
        public override void Down()
        {
            Sql(@"  ALTER VIEW DNS3_Organizations AS SELECT
                    OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired,
                    OrganizationAcronym AS Acronym, ParentId, [SID], ContactEmail, ContactFirstName, 
                    ContactLastName, ContactPhone, SpecialRequirements, UsageRestrictions,
                    HealthPlanDescription,ObservationClinicalExperience, InpatientEHRApplication,
                    OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
                    InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registries,
                    ObservationalParticipation, ProspectiveTrials, EnrollmentClaims, DemographicsClaims,
                    LaboratoryResultsClaims, VitalSignsClaims, OtherClaims, EnableClaimsAndBilling, EnableEHRA,
                    DataModelMSCDM, DataModelHMORNVDW, DataModelESP, DataModelI2B2, DataModelOther
                    FROM dbo.Organizations");
        }
    }
}
