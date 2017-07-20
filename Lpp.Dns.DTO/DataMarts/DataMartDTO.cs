using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DataMart List
    /// </summary>
    [DataContract]
    public class DataMartDTO : DataMartListDTO
    {
        /// <summary>
        /// DataMArt DTO
        /// </summary>
        public DataMartDTO() { }
        /// <summary>
        /// Determines the approval for DataMarts list
        /// </summary>
        [DataMember]
        public bool RequiresApproval { get; set; }
        /// <summary>
        /// The ID of DataMart Type
        /// </summary>
        [DataMember]
        public Guid DataMartTypeID { get; set; }
        /// <summary>
        /// DataMart Type
        /// </summary>
        [DataMember]
        public string DataMartType { get; set; }
        /// <summary>
        /// Available Period
        /// </summary>
        [DataMember]
        [MaxLength(500)]
        public string AvailablePeriod { get; set; }
        /// <summary>
        /// Contact Email
        /// </summary>
        [DataMember]
        [MaxLength(510)]
        public string ContactEmail { get; set; }
        /// <summary>
        /// Contact First Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string ContactFirstName { get; set; }
        /// <summary>
        /// Contact Last Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string ContactLastName { get; set; }
        /// <summary>
        /// Contact Phone
        /// </summary>
        [DataMember]
        [MaxLength(15)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// Special Requirements
        /// </summary>
        [DataMember]
        [MaxLength(1000)]
        public string SpecialRequirements { get; set; }
        /// <summary>
        /// Usage Restrictions
        /// </summary>
        [DataMember]
        [MaxLength(1000)]
        public string UsageRestrictions { get; set; }
        /// <summary>
        /// Determines whether it's deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Health Plan Description
        /// </summary>
        [DataMember]
        [MaxLength(1000)]
        public string HealthPlanDescription { get; set; }
        /// <summary>
        /// Determines whether DataMart is Grouped or not
        /// </summary>
        [DataMember]
        public bool? IsGroupDataMart { get; set; }
        /// <summary>
        /// Unattended Mode
        /// </summary>
        [DataMember]
        public UnattendedModes? UnattendedMode { get; set; }
        /// <summary>
        /// DataUpdate Frequency
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string DataUpdateFrequency { get; set; }
        /// <summary>
        /// Inpatient EHR Application
        /// </summary>
        [DataMember]
        [MaxLength(512)]
        public string InpatientEHRApplication { get; set; }
        /// <summary>
        /// Outpatient EHR Application
        /// </summary>
        [DataMember]
        [MaxLength(512)]
        public string OutpatientEHRApplication { get; set; }
        //[DataMember]
        //public bool LaboratoryResultsClaims { get; set; }
        /// <summary>
        /// Other Claims
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string OtherClaims { get; set; }
        /// <summary>
        /// Other Inpatient EHR Application
        /// </summary>
        [DataMember]
        [MaxLength(512)]
        public string OtherInpatientEHRApplication { get; set; }
        /// <summary>
        /// Other Outpatient EHR Application
        /// </summary>
        [DataMember]
        [MaxLength(512)]
        public string OtherOutpatientEHRApplication { get; set; }
        /// <summary>
        /// Determines whether Laboratory Results Any has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsAny { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Any has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsClaims { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Test Name has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestName { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Dates has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsDates { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Test LOINC has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestLOINC { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Test SNOMED has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestSNOMED { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Specimen Source has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsSpecimenSource { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Test Description has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestDescriptions { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Order Dates has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsOrderDates { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Test Results Interpretation has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestResultsInterpretation { get; set; }
        /// <summary>
        ///  Determines whether Laboratory Results Other has been selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsTestOther { get; set; }
        /// <summary>
        ///  Laboratory Results Test Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string LaboratoryResultsTestOtherText { get; set; }
        /// <summary>
        ///  Determines whether Inpatient Encounters Any has been selected
        /// </summary>

        [DataMember]
        public bool InpatientEncountersAny { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Any has been selected
        /// </summary>
        [DataMember]
        public bool InpatientEncountersEncounterID { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Provider Identifier has been selected
        /// </summary>
        [DataMember]
        public bool InpatientEncountersProviderIdentifier { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Dates of Service Any has been selected
        /// </summary>
        [DataMember]
        public bool InpatientDatesOfService { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters ICD9 Procedures has been selected
        /// </summary>
        [DataMember]
        public bool InpatientICD9Procedures { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters ICD10 Procedures has been selected
        /// </summary>
        [DataMember]
        public bool InpatientICD10Procedures { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters ICD9 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool InpatientICD9Diagnosis { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters ICD10 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool InpatientICD10Diagnosis { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters SNOMED has been selected
        /// </summary>
        [DataMember]
        public bool InpatientSNOMED { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters HCPCS has been selected
        /// </summary>
        [DataMember]
        public bool InpatientHPHCS { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Disposition has been selected
        /// </summary>
        [DataMember]
        public bool InpatientDisposition { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Discharge Status has been selected
        /// </summary>
        [DataMember]
        public bool InpatientDischargeStatus { get; set; }
        /// <summary>
        /// Determines whether Inpatient Encounters Other has been selected
        /// </summary>
        [DataMember]
        public bool InpatientOther { get; set; }
        /// <summary>
        /// Inpatient Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string InpatientOtherText { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Any has been selected
        /// </summary>

        [DataMember]
        public bool OutpatientEncountersAny { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Encounter ID has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientEncountersEncounterID { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Provider Identifier has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientEncountersProviderIdentifier { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Clinical setting has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientClinicalSetting { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Dates of Service has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientDatesOfService { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters ICD9 Procedures has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientICD9Procedures { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters ICD10 Procedures has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientICD10Procedures { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters ICD9 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientICD9Diagnosis { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters ICD10 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientICD10Diagnosis { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters SNOMED has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientSNOMED { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters HCPCS has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientHPHCS { get; set; }
        /// <summary>
        /// Determines whether Outpatient Encounters Other has been selected
        /// </summary>
        [DataMember]
        public bool OutpatientOther { get; set; }
        /// <summary>
        /// Outpatient Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string OutpatientOtherText { get; set; }
        /// <summary>
        /// Determines whether ER Patient ID has been selected
        /// </summary>


        [DataMember]
        public bool ERPatientID { get; set; }
        /// <summary>
        /// Determines whether ER Encounter ID has been selected
        /// </summary>
        [DataMember]
        public bool EREncounterID { get; set; }
        /// <summary>
        /// Determines whether ER Enrollment Dates has been selected
        /// </summary>
        [DataMember]
        public bool EREnrollmentDates { get; set; }
        /// <summary>
        /// Determines whether ER Encounter Dates has been selected
        /// </summary>
        [DataMember]
        public bool EREncounterDates { get; set; }
        /// <summary>
        /// Determines whether ER Clinical setting has been selected
        /// </summary>
        [DataMember]
        public bool ERClinicalSetting { get; set; }
        /// <summary>
        /// Determines whether ER ICD9 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool ERICD9Diagnosis { get; set; }
        /// <summary>
        /// Determines whether ER ICD10 Diagnosis has been selected
        /// </summary>
        [DataMember]
        public bool ERICD10Diagnosis { get; set; }
        /// <summary>
        /// Determines whether ER HCPCS has been selected
        /// </summary>
        [DataMember]
        public bool ERHPHCS { get; set; }
        /// <summary>
        /// Determines whether ER NDC has been selected
        /// </summary>
        [DataMember]
        public bool ERNDC { get; set; }
        /// <summary>
        /// Determines whether ER SNOMED has been selected
        /// </summary>
        [DataMember]
        public bool ERSNOMED { get; set; }
        /// <summary>
        /// Determines whether ER Provider Identifier has been selected
        /// </summary>
        [DataMember]
        public bool ERProviderIdentifier { get; set; }
        /// <summary>
        /// Determines whether ER Provider Facility has been selected
        /// </summary>
        [DataMember]
        public bool ERProviderFacility { get; set; }
        /// <summary>
        /// Determines whether ER Encounter Type has been selected
        /// </summary>
        [DataMember]
        public bool EREncounterType { get; set; }
        /// <summary>
        /// Determines whether ER DRUG has been selected
        /// </summary>
        [DataMember]
        public bool ERDRG { get; set; }
        /// <summary>
        /// Determines whether ER Drug Type has been selected
        /// </summary>
        [DataMember]
        public bool ERDRGType { get; set; }
        /// <summary>
        /// Determines whether ER Other has been selected
        /// </summary>
        [DataMember]
        public bool EROther { get; set; }
        /// <summary>
        /// ER Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string EROtherText { get; set; }
        /// <summary>
        /// Determines whether Demographics Any has been selected
        /// </summary>

        [DataMember]
        public bool DemographicsAny { get; set; }
        /// <summary>
        ///  Determines whether Demographics Patient ID has been selected
        /// </summary>
        [DataMember]
        public bool DemographicsPatientID { get; set; }
        /// <summary>
        ///  Determines whether Demographics Sex has been selected
        /// </summary>
        [DataMember]
        public bool DemographicsSex { get; set; }
        /// <summary>
        ///  Determines whether Demographics Date Of Birth has been selected
        /// </summary>
        [DataMember]
        public bool DemographicsDateOfBirth { get; set; }
        /// <summary>
        ///  Determines whether Demographics Date of Death has been selected
         /// </summary>
        [DataMember]
        public bool DemographicsDateOfDeath { get; set; }
        /// <summary>
        ///  Determines whether Demographics Address info has been selected
         /// </summary>
        [DataMember]
        public bool DemographicsAddressInfo { get; set; }
        /// <summary>
        ///  Determines whether Demographics Race has been selected
         /// </summary>
        [DataMember]
        public bool DemographicsRace { get; set; }
        /// <summary>
        ///  Determines whether Demographics Ethnicity has been selected
         /// </summary>
        [DataMember]
        public bool DemographicsEthnicity { get; set; }
        /// <summary>
        ///  Determines whether Demographics Other has been selected
         /// </summary>
        [DataMember]
        public bool DemographicsOther { get; set; }
        /// <summary>
        ///  Demographics Other Text
         /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string DemographicsOtherText { get; set; }
        /// <summary>
        ///  Determines whether Patient Outcomes Any has been selected
         /// </summary>

        [DataMember]
        public bool PatientOutcomesAny { get; set; }
        /// <summary>
        /// Determines whether Patient Outcomes Instruments has been selected
        /// </summary>
        [DataMember]
        public bool PatientOutcomesInstruments { get; set; }
        /// <summary>
        /// Patient Outcomes Instrument Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PatientOutcomesInstrumentText { get; set; }
        /// <summary>
        /// Determines whether Patient Outcomes Health Behavior has been selected
        /// </summary>
        [DataMember]
        public bool PatientOutcomesHealthBehavior { get; set; }
        /// <summary>
        /// Determines whether Patient Outcomes HRQoL has been selected
        /// </summary>
        [DataMember]
        public bool PatientOutcomesHRQoL { get; set; }
        /// <summary>
        /// Determines whether Patient Outcomes Reported Outcome has been selected
        /// </summary>
        [DataMember]
        public bool PatientOutcomesReportedOutcome { get; set; }
        /// <summary>
        /// Determines whether Patient Outcomes Other has been selected
        /// </summary>
        [DataMember]
        public bool PatientOutcomesOther { get; set; }
        /// <summary>
        /// Patient Outcomes Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PatientOutcomesOtherText { get; set; }
        /// <summary>
        /// Determines whether PatientBehavior Health Behavior has been selected
        /// </summary>

        [DataMember]
        public bool PatientBehaviorHealthBehavior { get; set; }
        /// <summary>
        ///  Determines whether PatientBehavior Instruments has been selected
        /// </summary>
        [DataMember]
        public bool PatientBehaviorInstruments { get; set; }
        /// <summary>
        ///  PatientBehavior Instrument Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PatientBehaviorInstrumentText { get; set; }
        /// <summary>
        ///  Determines whether PatientBehavior Other has been selected
        /// </summary>
        [DataMember]
        public bool PatientBehaviorOther { get; set; }
        /// <summary>
        ///  PatientBehavior Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PatientBehaviorOtherText { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Any has been selected
        /// </summary>

        [DataMember]
        public bool VitalSignsAny { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Temperature has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsTemperature { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Height has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsHeight { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Weight has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsWeight { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs BMI has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsBMI { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Blood Pressure has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsBloodPressure { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Other has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsOther { get; set; }
        /// <summary>
        ///  Vital Signs Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string VitalSignsOtherText { get; set; }
        /// <summary>
        ///  Determines whether Vital Signs Length has been selected
        /// </summary>
        [DataMember]
        public bool VitalSignsLength { get; set; }
        /// <summary>
        ///  Determines whether Prescription Orders Any has been selected
        /// </summary>

        [DataMember]
        public bool PrescriptionOrdersAny { get; set; }
        /// <summary>
        /// Determines whether Prescription Orders Dates has been selected
        /// </summary>
        [DataMember]
        public bool PrescriptionOrderDates { get; set; }
        /// <summary>
        /// Determines whether Prescription Orders RxNorm has been selected
        /// </summary>
        [DataMember]
        public bool PrescriptionOrderRxNorm { get; set; }
        /// <summary>
        /// Determines whether Prescription Orders NDC has been selected
        /// </summary>
        [DataMember]
        public bool PrescriptionOrderNDC { get; set; }
        /// <summary>
        /// Determines whether Prescription Orders Other has been selected
        /// </summary>
        [DataMember]
        public bool PrescriptionOrderOther { get; set; }
        /// <summary>
        /// Prescription Order Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PrescriptionOrderOtherText { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing Any has been selected
        /// </summary>

        [DataMember]
        public bool PharmacyDispensingAny { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing Dates has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingDates { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing RxNorm has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingRxNorm { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing Days supply has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingDaysSupply { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing Amount Dispensed has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingAmountDispensed { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing NDC has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingNDC { get; set; }
        /// <summary>
        /// Determines whether Pharmacy Dispensing Other has been selected
        /// </summary>
        [DataMember]
        public bool PharmacyDispensingOther { get; set; }
        /// <summary>
        /// Pharmacy Dispensing Other Text
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string PharmacyDispensingOtherText { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Any has been selected
        /// </summary>

        [DataMember]
        public bool BiorepositoriesAny { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Name has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesName { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Description has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesDescription { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Disease Name has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesDiseaseName { get; set; }
        /// <summary>
        /// Determines whether Biorepositories specimen source has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesSpecimenSource { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Specimen Type has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesSpecimenType { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Processing Method has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesProcessingMethod { get; set; }
        /// <summary>
        /// Determines whether Biorepositories SNOMED has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesSNOMED { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Storage Method has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesStorageMethod { get; set; }
        /// <summary>
        /// Determines whether Biorepositories Other has been selected
        /// </summary>
        [DataMember]
        public bool BiorepositoriesOther { get; set; }
        /// <summary>
        /// Biorepositories Other Text 
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string BiorepositoriesOtherText { get; set; }
        /// <summary>
        /// Determines whether Longitudinal Capture Any has been selected
        /// </summary>

        [DataMember]
        public bool LongitudinalCaptureAny { get; set; }
        /// <summary>
        /// Determines whether Longitudinal Capture Patient ID has been selected
        /// </summary>
        [DataMember]
        public bool LongitudinalCapturePatientID { get; set; }
        /// <summary>
        /// Determines whether Longitudinal Capture Start has been selected
        /// </summary>
        [DataMember]
        public bool LongitudinalCaptureStart { get; set; }
        /// <summary>
        /// Determines whether Longitudinal Capture Stop has been selected
        /// </summary>
        [DataMember]
        public bool LongitudinalCaptureStop { get; set; }
        /// <summary>
        /// Determines whether Longitudinal Capture Other has been selected
        /// </summary>
        [DataMember]
        public bool LongitudinalCaptureOther { get; set; }
        /// <summary>
        ///Longitudinal Capture Other Value
        /// </summary>
        [DataMember]
        [MaxLength(80)]
        public string LongitudinalCaptureOtherValue { get; set; }
        /// <summary>
        /// Data Model
        /// </summary>

        [DataMember]
        [MaxLength(512)]
        public string DataModel { get; set; }
        /// <summary>
        /// Other Data Model
        /// </summary>
        [DataMember]
        [MaxLength(512)]
        public string OtherDataModel { get; set; }
        /// <summary>
        /// Determines whether DataMart is Local or not
        /// </summary>
        [DataMember]
        public bool IsLocal { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        [DataMember, MaxLength(255)]
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the ID of the data adapter this datamart supports.
        /// </summary>
        [DataMember]
        public Guid? AdapterID { get; set; }
        /// <summary>
        /// Gets or sets the name of the data adapter the datamart supports.
        /// </summary>
        [DataMember]
        public string Adapter { get; set; }

        /// <summary>
        /// Gets or sets the adapter ID on the datamart.
        /// </summary>
        [DataMember]
        public Guid? ProcessorID { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier for if the Adapter ID is Distributed Regression
        /// </summary>
        [DataMember]
        public string DataPartnerIdentifier { get; set; }

        ///// <summary>
        ///// Gets or Sets the Start Date
        ///// </summary>
        //[DataMember]
        //public DateTime? StartDate { get; set; }

        ///// <summary>
        ///// Gets or Sets the End Date
        ///// </summary>
        //[DataMember]
        //public DateTime? EndDate { get; set; }

    }
}
