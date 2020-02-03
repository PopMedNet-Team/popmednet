using System;
using System.Linq;
using System.Collections.Generic;
using Lpp.Security;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("DNS3_DataMarts")]
    public class DataMart : ISecurityObject, IHaveId<int>, INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("DataMart");

        public DataMart()
        {
            this.Routings = new HashSet<RequestRouting>();
            this.Groups = new HashSet<Group>();
            this.Projects = new HashSet<Project>();
            this.InstalledModels = new HashSet<DataMartInstalledModel>();
            this.UnattendedMode = UnattendedModeKind.NoUnattendedOperation;
            this.SID = UserDefinedFunctions.NewGuid();
            this.DataMartTypeId = 1;
        }

        [Key]
        public int Id { get; set; }
        public Guid SID { get; set; }
        [Column(TypeName = "varchar"), MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Url { get; set; }
        public bool RequiresApproval { get; set; }
        public int DataMartTypeId { get; set; }
        [MaxLength(500)]
        public string AvailablePeriod { get; set; }
        [Column(TypeName = "varchar"), MaxLength(510)]
        public string ContactEmail { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string ContactFirstName { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string ContactLastName { get; set; }
        [Column(TypeName = "varchar"), MaxLength(15)]
        public string ContactPhone { get; set; }
        [Column(TypeName = "varchar"), MaxLength(1000)]
        public string SpecialRequirements { get; set; }
        [Column(TypeName = "varchar"), MaxLength(1000)]
        public string UsageRestrictions { get; set; }
        [MaxLength(1000)]
        public string HealthPlanDescription { get; set; }
        public string DataMartDescription { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool EffectiveIsDeleted { get; set; }

        public UnattendedModeKind UnattendedMode { get; set; }

        /// <summary>
        /// This flag designates one of the datamarts as "local" - i.e. an interface to the data stored in our own PMN/DNS database
        /// </summary>
        public bool IsLocal { get; set; }


        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }

        public virtual Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        [Column(TypeName = "varchar"), MaxLength(510)]
        public string Description { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string Acronym { get; set; }

        public bool LaboratoryResultsAny { get; set; }
        public bool LaboratoryResultsClaims { get; set; }
        public bool LaboratoryResultsTestName { get; set; }
        public bool LaboratoryResultsDates { get; set; }
        public bool LaboratoryResultsTestLOINC { get; set; }
        public bool LaboratoryResultsTestSNOMED { get; set; }
        public bool LaboratoryResultsTestResultsInterpretation { get; set; }
        public bool LaboratoryResultsSpecimenSource { get; set; }
        public bool LaboratoryResultsOrderDates { get; set; }
        public bool LaboratoryResultsTestDescriptions { get; set; }

        public bool LaboratoryResultsTestOther { get; set; }

        [MaxLength(80)]
        public string LaboratoryResultsTestOtherText { get; set; }

        public bool InpatientEncountersAny { get; set; }
        public bool InpatientEncountersEncounterID { get; set; }
        public bool InpatientEncountersProviderIdentifier { get; set; }
        public bool InpatientDatesOfService { get; set; }
        public bool InpatientICD9Procedures { get; set; }
        public bool InpatientICD10Procedures { get; set; }
        public bool InpatientICD9Diagnosis { get; set; }
        public bool InpatientICD10Diagnosis { get; set; }
        public bool InpatientSNOMED { get; set; }
        public bool InpatientHPHCS { get; set; }
        public bool InpatientDisposition { get; set; }
        public bool InpatientDischargeStatus { get; set; }
        public bool InpatientOther { get; set; }
        [MaxLength(80)]
        public string InpatientOtherText { get; set; }

        public bool OutpatientEncountersAny { get; set; }
        public bool OutpatientEncountersEncounterID { get; set; }
        public bool OutpatientEncountersProviderIdentifier { get; set; }
        public bool OutpatientClinicalSetting { get; set; }
        public bool OutpatientDatesOfService { get; set; }
        public bool OutpatientICD9Procedures { get; set; }
        public bool OutpatientICD10Procedures { get; set; }
        public bool OutpatientICD9Diagnosis { get; set; }
        public bool OutpatientICD10Diagnosis { get; set; }
        public bool OutpatientSNOMED { get; set; }
        public bool OutpatientHPHCS { get; set; }
        public bool OutpatientOther { get; set; }
        [MaxLength(80)]
        public string OutpatientOtherText { get; set; }

        public string OtherClaims { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "varchar"), MaxLength(100)]
        public string DataUpdateFrequency { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
        public string DataModel { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
        public string OtherDataModel { get; set; }

        public bool ERPatientID { get; set; }
        public bool EREncounterID { get; set; }
        public bool EREnrollmentDates { get; set; }
        public bool EREncounterDates { get; set; }
        public bool ERClinicalSetting { get; set; }
        public bool ERICD9Diagnosis { get; set; }
        public bool ERICD10Diagnosis { get; set; }
        public bool ERHPHCS { get; set; }
        public bool ERNDC { get; set; }
        public bool ERSNOMED { get; set; }
        public bool ERProviderIdentifier { get; set; }
        public bool ERProviderFacility { get; set; }
        public bool EREncounterType { get; set; }
        public bool ERDRG { get; set; }
        public bool ERDRGType { get; set; }
        public bool EROther { get; set; }
        [MaxLength(80)]
        public string EROtherText { get; set; }

        public bool DemographicsAny { get; set; }
        public bool DemographicsPatientID { get; set; }
        public bool DemographicsSex { get; set; }
        public bool DemographicsDateOfBirth { get; set; }
        public bool DemographicsDateOfDeath { get; set; }
        public bool DemographicsAddressInfo { get; set; }
        public bool DemographicsRace { get; set; }
        public bool DemographicsEthnicity { get; set; }
        public bool DemographicsOther { get; set; }
        [MaxLength(80)]
        public string DemographicsOtherText { get; set; }

        public bool PatientOutcomesAny { get; set; }
        public bool PatientOutcomesInstruments { get; set; }
        [MaxLength(80)]
        public string PatientOutcomesInstrumentText { get; set; }
        public bool PatientOutcomesHealthBehavior { get; set; }
        public bool PatientOutcomesHRQoL { get; set; }
        public bool PatientOutcomesReportedOutcome { get; set; }
        public bool PatientOutcomesOther { get; set; }
        [MaxLength(80)]
        public string PatientOutcomesOtherText { get; set; }

        public bool PatientBehaviorHealthBehavior { get; set; }
        public bool PatientBehaviorInstruments { get; set; }
        [MaxLength(80)]
        public string PatientBehaviorInstrumentText { get; set; }
        public bool PatientBehaviorOther { get; set; }
        [MaxLength(80)]
        public string PatientBehaviorOtherText { get; set; }

        public bool VitalSignsAny { get; set; }
        public bool VitalSignsTemperature { get; set; }
        public bool VitalSignsHeight { get; set; }
        public bool VitalSignsWeight { get; set; }
        public bool VitalSignsLength { get; set; }
        public bool VitalSignsBMI { get; set; }
        public bool VitalSignsBloodPressure { get; set; }
        public bool VitalSignsOther { get; set; }
        [MaxLength(80)]
        public string VitalSignsOtherText { get; set; }

        public bool PrescriptionOrdersAny { get; set; }
        public bool PrescriptionOrderDates { get; set; }
        public bool PrescriptionOrderRxNorm { get; set; }
        public bool PrescriptionOrderNDC { get; set; }
        public bool PrescriptionOrderOther { get; set; }
        [MaxLength(80)]
        public string PrescriptionOrderOtherText { get; set; }

        public bool PharmacyDispensingAny { get; set; }
        public bool PharmacyDispensingDates { get; set; }
        public bool PharmacyDispensingRxNorm { get; set; }
        public bool PharmacyDispensingDaysSupply { get; set; }
        public bool PharmacyDispensingAmountDispensed { get; set; }
        public bool PharmacyDispensingNDC { get; set; }
        public bool PharmacyDispensingOther { get; set; }
        [MaxLength(80)]
        public string PharmacyDispensingOtherText { get; set; }

        public bool BiorepositoriesAny { get; set; }
        public bool BiorepositoriesName { get; set; }
        public bool BiorepositoriesDescription { get; set; }
        public bool BiorepositoriesDiseaseName { get; set; }
        public bool BiorepositoriesSpecimenSource { get; set; }
        public bool BiorepositoriesSpecimenType { get; set; }
        public bool BiorepositoriesProcessingMethod { get; set; }
        public bool BiorepositoriesSNOMED { get; set; }
        public bool BiorepositoriesStorageMethod { get; set; }
        public bool BiorepositoriesOther { get; set; }
        [MaxLength(80)]
        public string BiorepositoriesOtherText { get; set; }

        public bool LongitudinalCaptureAny { get; set; }
        public bool LongitudinalCapturePatientID { get; set; }
        public bool LongitudinalCaptureStart { get; set; }
        public bool LongitudinalCaptureStop { get; set; }
        public bool LongitudinalCaptureOther { get; set; }
        [MaxLength(80)]
        public string LongitudinalCaptureOtherValue { get; set; }

        public virtual ICollection<DataMartInstalledModel> InstalledModels { get; set; }
        public virtual ICollection<RequestRouting> Routings { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Request> InSearchResults { get; set; }

        public override string ToString() { return Name; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class DataMartPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var dm = builder.Entity<DataMart>();

            dm.HasMany(d => d.InstalledModels).WithRequired(m => m.DataMart).HasForeignKey(m => m.DataMartId);
            dm.HasMany(t => t.Routings).WithRequired(t => t.DataMart).HasForeignKey(t => t.DataMartId).WillCascadeOnDelete(true);

            //To-Do implement link between groups and Data marts
        }
    }

}
