namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataDomainAnyFieldsToDataMartsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "LaboratoryResultsAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "InpatientEncountersAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "OutpatientEncountersAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "DemographicsAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PatientOutcomesAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "VitalSignsAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PrescriptionOrdersAny", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PharmacyDispensingAny", c => c.Boolean(nullable: false));
            //AddColumn("dbo.DataMarts", "BiorepositoriesAny", c => c.Boolean(nullable: false)); // BiorepositoryAny field was added manually in 201403311541485_AddBiorepositoriesAnyToDataMartsTable.cs
            AddColumn("dbo.DataMarts", "LongitudinalCaptureAny", c => c.Boolean(nullable: false));

            Sql(@"ALTER VIEW [dbo].[DNS3_DataMarts]
AS
SELECT        d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, d.ContactPhone, d.SpecialRequirements, 
                         d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.Description, d.Acronym, d.DataMartDescription, d.StartDate, d.EndDate, 
                         d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, d.Registries, d.DataModel, d.OtherDataModel, d.SID, 
                         d.IsLocal, d.LaboratoryResultsClaims, d.OtherClaims, CAST(CASE WHEN ISNULL(d .isdeleted, 0) = 1 OR
                         ISNULL(o.isdeleted, 0) = 1 THEN 1 ELSE 0 END AS bit) AS EffectiveIsDeleted, d.LaboratoryResultsTestName, d.LaboratoryResultsDates, d.LaboratoryResultsTestLOINC, d.LaboratoryResultsTestSNOMED, 
                         d.LaboratoryResultsTestResultsInterpretation, d.LaboratoryResultsTestOther, d.LaboratoryResultsTestOtherText, d.LaboratoryResultsSpecimenSource, d.LaboratoryResultsTestDescriptions, 
                         d.LaboratoryResultsOrderDates, d.InpatientDatesOfService, d.InpatientICD9Procedures, d.InpatientICD10Procedures, d.InpatientICD9Diagnosis, d.InpatientICD10Diagnosis, d.InpatientSNOMED, d.InpatientHPHCS, 
                         d.InpatientDisposition, d.InpatientDischargeStatus, d.InpatientOther, d.InpatientOtherText, d.OutpatientClinicalSetting, d.OutpatientDatesOfService, d.OutpatientICD9Procedures, d.OutpatientICD10Procedures, 
                         d.OutpatientICD9Diagnosis, d.OutpatientICD10Diagnosis, d.OutpatientSNOMED, d.OutpatientHPHCS, d.OutpatientOther, d.OutpatientOtherText, d.ERPatientID, d.EREncounterID, d.EREnrollmentDates, 
                         d.EREncounterDates, d.ERClinicalSetting, d.ERICD9Diagnosis, d.ERICD10Diagnosis, d.ERHPHCS, d.ERNDC, d.ERSNOMED, d.ERProviderIdentifier, d.ERProviderFacility, d.EREncounterType, d.ERDRG, 
                         d.ERDRGType, d.EROther, d.EROtherText, d.DemographicsPatientID, d.DemographicsSex, d.DemographicsDateOfBirth, d.DemographicsDateOfDeath, d.DemographicsAddressInfo, d.DemographicsRace, 
                         d.DemographicsEthnicity, d.DemographicsOtherText, d.PatientOutcomesInstruments, d.PatientOutcomesInstrumentText, d.PatientOutcomesHealthBehavior, d.PatientOutcomesHRQoL, 
                         d.PatientOutcomesReportedOutcome, d.PatientOutcomesOther, d.PatientOutcomesOtherText, d.PatientBehaviorHealthBehavior, d.PatientBehaviorInstruments, d.PatientBehaviorInstrumentText, 
                         d.PatientBehaviorOther, d.PatientBehaviorOtherText, d.VitalSignsTemperature, d.VitalSignsHeight, d.VitalSignsWeight, d.VitalSignsBMI, d.VitalSignsBloodPressure, d.VitalSignsOther, d.VitalSignsOtherText, 
                         d.PrescriptionOrderDates, d.PrescriptionOrderRxNorm, d.PrescriptionOrderNDC, d.PrescriptionOrderOther, d.PrescriptionOrderOtherText, d.PharmacyDispensingDates, d.PharmacyDispensingRxNorm, 
                         d.PharmacyDispensingDaysSupply, d.PharmacyDispensingAmountDispensed, d.PharmacyDispensingNDC, d.PharmacyDispensingOther, d.PharmacyDispensingOtherText, d.BiorepositoriesName, 
                         d.BiorepositoriesDescription, d.BiorepositoriesDiseaseName, d.BiorepositoriesSpecimenSource, d.BiorepositoriesSpecimenType, d.BiorepositoriesProcessingMethod, d.BiorepositoriesSNOMED, 
                         d.BiorepositoriesStorageMethod, d.BiorepositoriesOther, d.BiorepositoriesOtherText, d.DemographicsOther, d.InpatientEncountersEncounterID, d.InpatientEncountersProviderIdentifier,
                         d.OutpatientEncountersEncounterID, d.OutpatientEncountersProviderIdentifier, d.LongitudinalCapturePatientID, d.LongitudinalCaptureStart, d.LongitudinalCaptureStop, d.LongitudinalCaptureOther, d.LongitudinalCaptureOtherValue,d.VitalSignsLength, d.BiorepositoriesAny,
                         d.LongitudinalCaptureAny,
                         d.PharmacyDispensingAny,
                         d.PrescriptionOrdersAny,
                         d.VitalSignsAny,
                         d.PatientOutcomesAny,
                         d.DemographicsAny,
                         d.OutpatientEncountersAny,
                         d.InpatientEncountersAny,
                         d.LaboratoryResultsAny
FROM            dbo.DataMarts AS d INNER JOIN
                         dbo.Organizations AS o ON d.OrganizationId = o.OrganizationId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "LongitudinalCaptureAny");
            //DropColumn("dbo.DataMarts", "BiorepositoriesAny"); // BiorepositoryAny field was added manually in 201403311541485_AddBiorepositoriesAnyToDataMartsTable.cs
            DropColumn("dbo.DataMarts", "PharmacyDispensingAny");
            DropColumn("dbo.DataMarts", "PrescriptionOrdersAny");
            DropColumn("dbo.DataMarts", "VitalSignsAny");
            DropColumn("dbo.DataMarts", "PatientOutcomesAny");
            DropColumn("dbo.DataMarts", "DemographicsAny");
            DropColumn("dbo.DataMarts", "OutpatientEncountersAny");
            DropColumn("dbo.DataMarts", "InpatientEncountersAny");
            DropColumn("dbo.DataMarts", "LaboratoryResultsAny");

            Sql(@"ALTER VIEW [dbo].[DNS3_DataMarts]
AS
SELECT        d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, d.ContactPhone, d.SpecialRequirements, 
                         d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.Description, d.Acronym, d.DataMartDescription, d.StartDate, d.EndDate, 
                         d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, d.Registries, d.DataModel, d.OtherDataModel, d.SID, 
                         d.IsLocal, d.LaboratoryResultsClaims, d.OtherClaims, CAST(CASE WHEN ISNULL(d .isdeleted, 0) = 1 OR
                         ISNULL(o.isdeleted, 0) = 1 THEN 1 ELSE 0 END AS bit) AS EffectiveIsDeleted, d.LaboratoryResultsTestName, d.LaboratoryResultsDates, d.LaboratoryResultsTestLOINC, d.LaboratoryResultsTestSNOMED, 
                         d.LaboratoryResultsTestResultsInterpretation, d.LaboratoryResultsTestOther, d.LaboratoryResultsTestOtherText, d.LaboratoryResultsSpecimenSource, d.LaboratoryResultsTestDescriptions, 
                         d.LaboratoryResultsOrderDates, d.InpatientDatesOfService, d.InpatientICD9Procedures, d.InpatientICD10Procedures, d.InpatientICD9Diagnosis, d.InpatientICD10Diagnosis, d.InpatientSNOMED, d.InpatientHPHCS, 
                         d.InpatientDisposition, d.InpatientDischargeStatus, d.InpatientOther, d.InpatientOtherText, d.OutpatientClinicalSetting, d.OutpatientDatesOfService, d.OutpatientICD9Procedures, d.OutpatientICD10Procedures, 
                         d.OutpatientICD9Diagnosis, d.OutpatientICD10Diagnosis, d.OutpatientSNOMED, d.OutpatientHPHCS, d.OutpatientOther, d.OutpatientOtherText, d.ERPatientID, d.EREncounterID, d.EREnrollmentDates, 
                         d.EREncounterDates, d.ERClinicalSetting, d.ERICD9Diagnosis, d.ERICD10Diagnosis, d.ERHPHCS, d.ERNDC, d.ERSNOMED, d.ERProviderIdentifier, d.ERProviderFacility, d.EREncounterType, d.ERDRG, 
                         d.ERDRGType, d.EROther, d.EROtherText, d.DemographicsPatientID, d.DemographicsSex, d.DemographicsDateOfBirth, d.DemographicsDateOfDeath, d.DemographicsAddressInfo, d.DemographicsRace, 
                         d.DemographicsEthnicity, d.DemographicsOtherText, d.PatientOutcomesInstruments, d.PatientOutcomesInstrumentText, d.PatientOutcomesHealthBehavior, d.PatientOutcomesHRQoL, 
                         d.PatientOutcomesReportedOutcome, d.PatientOutcomesOther, d.PatientOutcomesOtherText, d.PatientBehaviorHealthBehavior, d.PatientBehaviorInstruments, d.PatientBehaviorInstrumentText, 
                         d.PatientBehaviorOther, d.PatientBehaviorOtherText, d.VitalSignsTemperature, d.VitalSignsHeight, d.VitalSignsWeight, d.VitalSignsBMI, d.VitalSignsBloodPressure, d.VitalSignsOther, d.VitalSignsOtherText, 
                         d.PrescriptionOrderDates, d.PrescriptionOrderRxNorm, d.PrescriptionOrderNDC, d.PrescriptionOrderOther, d.PrescriptionOrderOtherText, d.PharmacyDispensingDates, d.PharmacyDispensingRxNorm, 
                         d.PharmacyDispensingDaysSupply, d.PharmacyDispensingAmountDispensed, d.PharmacyDispensingNDC, d.PharmacyDispensingOther, d.PharmacyDispensingOtherText, d.BiorepositoriesName, 
                         d.BiorepositoriesDescription, d.BiorepositoriesDiseaseName, d.BiorepositoriesSpecimenSource, d.BiorepositoriesSpecimenType, d.BiorepositoriesProcessingMethod, d.BiorepositoriesSNOMED, 
                         d.BiorepositoriesStorageMethod, d.BiorepositoriesOther, d.BiorepositoriesOtherText, d.DemographicsOther, d.InpatientEncountersEncounterID, d.InpatientEncountersProviderIdentifier,
                         d.OutpatientEncountersEncounterID, d.OutpatientEncountersProviderIdentifier, d.LongitudinalCapturePatientID, d.LongitudinalCaptureStart, d.LongitudinalCaptureStop, d.LongitudinalCaptureOther, d.LongitudinalCaptureOtherValue,d.VitalSignsLength, d.BiorepositoriesAny
FROM            dbo.DataMarts AS d INNER JOIN
                         dbo.Organizations AS o ON d.OrganizationId = o.OrganizationId");
        }
    }
}
