namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMissingDataMartField1View : DbMigration
    {
        public override void Up()
        {
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
                         d.BiorepositoriesStorageMethod, d.BiorepositoriesOther, d.BiorepositoriesOtherText, d.DemographicsOther
FROM            dbo.DataMarts AS d INNER JOIN
                         dbo.Organizations AS o ON d.OrganizationId = o.OrganizationId
");
        }
        
        public override void Down()
        {
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
                         d.BiorepositoriesStorageMethod, d.BiorepositoriesOther, d.BiorepositoriesOtherText
FROM            dbo.DataMarts AS d INNER JOIN
                         dbo.Organizations AS o ON d.OrganizationId = o.OrganizationId
");
        }
    }
}
