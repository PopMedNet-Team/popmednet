using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataDataMartSearchModel
    {
        [DataMember]
        public string Data { get; set; }

        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public MetadataSearchRequestType RequestType { get; set; }
    }

    [DataContract]
    public class MetadataDataMartSearchData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool DataModelMSCDM { get; set; }
        [DataMember]
        public bool DataModelESP { get; set; }
        [DataMember]
        public bool DataModelHMRON { get; set; }
        [DataMember]
        public bool DataModeli2b2 { get; set; }
        [DataMember]
        public bool DataModelOMOP { get; set; }
        [DataMember]
        public bool DataModelPCORI { get; set; }
        [DataMember]
        public bool DataModelOther { get; set; }
        [DataMember]
        public string DataModelOtherValue { get; set; }
        //Data Domains Inpatient
        [DataMember]
        public bool InpatientEncountersAll { get; set; }
        [DataMember]
        public bool InpatientEncountersEncounterID { get; set; }
        [DataMember]
        public bool InpatientEncountersDatesofService { get; set; }
        [DataMember]
        public bool InpatientEncountersProviderIdentifier { get; set; }
        [DataMember]
        public bool InpatientEncountersICD9Procedures { get; set; }
        [DataMember]
        public bool InpatientEncountersICD10Procedures { get; set; }
        [DataMember]
        public bool InpatientEncountersICD9Diagnosis { get; set; }
        [DataMember]
        public bool InpatientEncountersICD10Diagnosis { get; set; }
        [DataMember]
        public bool InpatientEncountersSnowMed { get; set; }
        [DataMember]
        public bool InpatientEncountersHPHCS { get; set; }
        [DataMember]
        public bool InpatientEncountersDisposition { get; set; }
        [DataMember]
        public bool InpatientEncountersDischargeStatus { get; set; }
        [DataMember]
        public bool InpatientEncountersOther { get; set; }
        [DataMember]
        public string InpatientEncountersOtherValue { get; set; }
        //Data Domains Outpatient
        [DataMember]
        public bool OutpatientEncountersAll { get; set; }
        [DataMember]
        public bool OutpatientEncountersEncounterID { get; set; }
        [DataMember]
        public bool OutpatientEncountersDatesofService { get; set; }
        [DataMember]
        public bool OutpatientEncountersProviderIdentifier { get; set; }
        [DataMember]
        public bool OutpatientEncountersICD9Procedures { get; set; }
        [DataMember]
        public bool OutpatientEncountersICD10Procedures { get; set; }
        [DataMember]
        public bool OutpatientEncountersICD9Diagnosis { get; set; }
        [DataMember]
        public bool OutpatientEncountersICD10Diagnosis { get; set; }
        [DataMember]
        public bool OutpatientEncountersSnowMed { get; set; }
        [DataMember]
        public bool OutpatientEncountersHPHCS { get; set; }
        [DataMember]
        public bool OutpatientEncountersOther { get; set; }
        [DataMember]
        public string OutpatientEncountersOtherValue { get; set; }
        //Data Domains Laboratory Tests
        [DataMember]
        public bool LaboratoryTestResultsAll { get; set; }
        [DataMember]
        public bool LaboratoryTestsOrderDates { get; set; }
        [DataMember]
        public bool LaboratoryTestsResultDates { get; set; }
        [DataMember]
        public bool LaboratoryTestsName { get; set; }
        [DataMember]
        public bool LaboratoryTestsLOINC { get; set; }
        [DataMember]
        public bool LaboratoryTestsTestDescription { get; set; }
        [DataMember]
        public bool LaboratoryTestsSNOMED { get; set; }
        [DataMember]
        public bool LaboratoryTestsRESULT { get; set; }
        [DataMember]
        public bool LaboratoryTestsOther { get; set; }
        [DataMember]
        public string LaboratoryTestsOtherValue { get; set; }
        //Data Domain Demographics
        [DataMember]
        public bool DemographicsAll { get; set; }
        [DataMember]
        public bool DemographicsSex { get; set; }
        [DataMember]
        public bool DemographicsDOB { get; set; }
        [DataMember]
        public bool DemographicsDateofDeath { get; set; }
        [DataMember]
        public bool DemographicsAddress { get; set; }//displayed as Zip Code
        [DataMember]
        public bool DemographicsRace { get; set; }
        [DataMember]
        public bool DemographicsEthnicity { get; set; }
        [DataMember]
        public bool DemographicsOther { get; set; }
        [DataMember]
        public string DemographicsOtherValue { get; set; }
        //Data Domain Patient Reported Outcomes
        [DataMember]
        public bool PatientReportedOutcomesAll { get; set; }
        [DataMember]
        public bool PatientReportedOutcomesHealthBehavior { get; set; }
        [DataMember]
        public bool PatientReportedOutcomesHRQOL { get; set; }
        [DataMember]
        public bool PatientReportedOutcomesPRO { get; set; }
        [DataMember]
        public bool PatientReportedOutcomesOther { get; set; }
        [DataMember]
        public string PatientReportedOutcomesOtherValue { get; set; }
        //Data Domain Vital Signs
        [DataMember]
        public bool VitalSignsAll { get; set; }
        [DataMember]
        public bool VitalSignsTemp { get; set; }
        [DataMember]
        public bool VitalSignsHeight { get; set; }
        [DataMember]
        public bool VitalSignsWeight { get; set; }
        [DataMember]
        public bool VitalSignsLength { get; set; }
        [DataMember]
        public bool VitalSignsBMI { get; set; }
        [DataMember]
        public bool VitalSignsBloodPressure { get; set; }
        [DataMember]
        public bool VitalSignsOther { get; set; }
        [DataMember]
        public string VitalSignsOtherValue { get; set; }
        //Data Domain Prescription Orders
        [DataMember]
        public bool PrescriptionOrdersAll { get; set; }
        [DataMember]
        public bool PrescriptionOrdersDates { get; set; }
        [DataMember]
        public bool PrescriptionOrdersRxNorm { get; set; }
        [DataMember]
        public bool PrescriptionOrdersNDC { get; set; }
        [DataMember]
        public bool PrescriptionOrdersOther { get; set; }
        [DataMember]
        public string PrescriptionOrdersOtherValue { get; set; }
        //Data Domains PharmacyDispensing
        [DataMember]
        public bool PharmacyDispensingAll { get; set; }
        [DataMember]
        public bool PharmacyDispensingDates { get; set; }
        [DataMember]
        public bool PharmacyDispensingRxNorm { get; set; }
        [DataMember]
        public bool PharmacyDispensingSupply { get; set; }
        [DataMember]
        public bool PharmacyDispensingAmount { get; set; }
        [DataMember]
        public bool PharmacyDispensingNDC { get; set; }
        [DataMember]
        public bool PharmacyDispensingOther { get; set; }
        [DataMember]
        public string PharmacyDispensingOtherValue { get; set; }
        //Data Domain Biorepositories
        [DataMember]
        public bool BiorepositoriesAny { get; set; }
        //[DataMember]
        //public bool Biorepositories { get; set; }
        //[DataMember]
        //public bool BiorepositoriesName { get; set; }
        //[DataMember]
        //public bool BiorepositoriesDescription { get; set; }
        //[DataMember]
        //public bool BiorepositoriesDisease { get; set; }
        //[DataMember]
        //public bool BiorepositoriesSpecimenSource { get; set; }
        //[DataMember]
        //public bool BiorepositoriesSpecimanType { get; set; }
        //[DataMember]
        //public bool BiorepositoriesProcessing { get; set; }
        //[DataMember]
        //public bool BiorepositoriesSNOMED { get; set; }
        //[DataMember]
        //public bool BiorepositoriesStorage { get; set; }
        //[DataMember]
        //public bool BiorepositoriesOther { get; set; }
        //[DataMember]
        //public string BiorepositoriesOtherValue { get; set; }

        //Installed Models
        [DataMember]
        public bool InstallModelSql { get; set; }
        [DataMember]
        public bool InstallModelDataChecker { get; set; }
        [DataMember]
        public bool InstallModelESP { get; set; }
        [DataMember]
        public bool InstallModelFile { get; set; }
        [DataMember]
        public bool InstallModelSAS { get; set; }
        [DataMember]
        public bool InstallModelModular { get; set; }
        [DataMember]
        public bool InstallModelSPAN { get; set; }
        [DataMember]
        public bool InstallModelSummaryPrev { get; set; }
        [DataMember]
        public bool InstallModelSummaryInci { get; set; }
        [DataMember]
        public bool InstallModelSummaryMFU { get; set; }
        [DataMember]
        public bool InstallModelMetaData { get; set; }
        [DataMember]
        public bool InstallModelQueryComposer { get; set; }
        [DataMember]
        public bool InstallModelSqlSample { get; set; }

        //Date Period Range
        [DataMember]
        public int? StartDate { get; set; }
        [DataMember]
        public int? EndDate { get; set; }
        [DataMember]
        public bool UpdateNone { get; set; }
        [DataMember]
        public bool UpdateDaily { get; set; }
        [DataMember]
        public bool UpdateWeekly { get; set; }
        [DataMember]
        public bool UpdateMonthly { get; set; }
        [DataMember]
        public bool UpdateQuarterly { get; set; }
        [DataMember]
        public bool UpdateSemiAnnually { get; set; }
        [DataMember]
        public bool UpdateAnnually { get; set; }
        [DataMember]
        public bool UpdateOther { get; set; }
        [DataMember]
        public string UpdateOtherValue { get; set; }

        //Longitudinal Capture
        [DataMember]
        public bool LongitudinalCaptureAll { get; set; }
        [DataMember]
        public bool LongitudinalCapturePatientID { get; set; }
        [DataMember]
        public bool LongitudinalCaptureStart { get; set; }
        [DataMember]
        public bool LongitudinalCaptureStop { get; set; }
        [DataMember]
        public bool LongitudinalCaptureOther { get; set; }
        [DataMember]
        public string LongitudinalCaptureOtherValue { get; set; }
    }
}
