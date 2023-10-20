using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using System.IO;
using System.Web;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("DataMarts")]
    public partial class DataMart : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted, Lpp.Security.ISecurityObject
    {
        public DataMart() {
            this.ModifiedOn = DateTime.UtcNow;
            RequiresApproval = false;
            Deleted = false;
            IsGroupDataMart = false;
            IsLocal = false;
            Models = new HashSet<DataMartInstalledModel>();
        }

        [MaxLength(255), Required]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        public bool RequiresApproval { get; set; }

        [MaxLength]
        public string DataMartDescription { get; set; }

        public virtual DataMartType DataMartType { get; set; }

        public Guid DataMartTypeID { get; set;}

        [MaxLength(500), Column(TypeName = "varchar")]
        public string AvailablePeriod { get; set; }

        [MaxLength(510), Column(TypeName = "varchar")]
        public string ContactEmail { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string ContactFirstName { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string ContactLastName { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        public string ContactPhone { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string SpecialRequirements { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string UsageRestrictions { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string HealthPlanDescription { get; set; }

        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public bool? IsGroupDataMart { get; set; }


        public UnattendedModes? UnattendedMode { get; set; }

        [MaxLength]
        public string Description { get; set; }

        [MaxLength(100), Required(AllowEmptyStrings=true)]
        public string Acronym { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string DataUpdateFrequency { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string InpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string OutpatientEHRApplication { get; set; }

        public bool LaboratoryResultsAny { get; set; }
        public bool LaboratoryResultsClaims { get; set; }
        public bool LaboratoryResultsTestName { get; set; }
        public bool LaboratoryResultsDates { get; set; }
        public bool LaboratoryResultsTestLOINC { get; set; }
        public bool LaboratoryResultsTestSNOMED { get; set; }
        public bool LaboratoryResultsSpecimenSource { get; set; }
        public bool LaboratoryResultsTestDescriptions { get; set; }
        public bool LaboratoryResultsOrderDates { get; set; }
        public bool LaboratoryResultsTestResultsInterpretation { get; set; }
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
        public bool VitalSignsBMI { get; set; }
        public bool VitalSignsBloodPressure { get; set; }
        public bool VitalSignsOther { get; set; }

        [MaxLength(80)]
        public string VitalSignsOtherText { get; set; }
        public bool VitalSignsLength { get; set; }

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

        [MaxLength(80), Column(TypeName = "nvarchar")]
        public string OtherClaims { get; set; }

        //TODO: registies is not applicable and should be removed in the future
        [MaxLength(1000), Column(TypeName = "varchar")]
        public string Registeries { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string OtherInpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string OtherOutpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string DataModel { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string OtherDataModel { get; set; }

        public bool IsLocal { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid? AdapterID { get; set; }
        public virtual DataModel Adapter { get; set; }

        public Guid? ProcessorID { get; set; }
        /// <summary>
        /// Gets or sets the data partner identifier, used by distributed regression to specify the folder to download the partner output files to at the analysis center.
        /// </summary>
        [MaxLength(255)]
        public string DataPartnerIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the data partner code, used by distributed regression to specify the partner code used to identify the partner in the tracking tables.
        /// </summary>
        [MaxLength(255)]
        public string DataPartnerCode { get; set; }

        public virtual ICollection<RequestDataMart> Requests { get; set; }
        public virtual ICollection<ProjectDataMart> Projects { get; set; }
        public virtual ICollection<DataMartInstalledModel> Models { get; set; }
        public virtual ICollection<DataMartAvailabilityPeriod_v2> DataMartAvailabilityPeriods_v2 { get; set; }

        public virtual ICollection<AclDataMart> DataMartAcls { get; set; }
        public virtual ICollection<AclOrganizationDataMart> OrganizationDataMarts { get; set; }
        public virtual ICollection<AclProjectDataMart> ProjectDataMartAcls { get; set; }
        public virtual ICollection<DataMartEvent> DataMartEvents { get; set; }
        public virtual ICollection<ProjectDataMartEvent> ProjectDataMartEvents { get; set; }
        public virtual ICollection<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; }
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; }
        public virtual ICollection<Audit.DataMartChangeLog> ChangeLogs { get; set; }
        

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("DataMart");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }
    }

    internal class DataMartConfiguration : EntityTypeConfiguration<DataMart>
    {
        public DataMartConfiguration() 
        {
            HasMany(t => t.Requests).WithRequired(t => t.DataMart).HasForeignKey(t => t.DataMartID).WillCascadeOnDelete(true);

            HasMany(t => t.Projects).WithRequired(t => t.DataMart).HasForeignKey(t => t.DataMartID).WillCascadeOnDelete(true);

            HasMany(t => t.DataMartAvailabilityPeriods_v2).WithRequired(t => t.DataMart).HasForeignKey(t => t.DataMartID).WillCascadeOnDelete(false);

            HasMany(t => t.Models)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.DataMartAcls)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.OrganizationDataMarts)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.ProjectDataMartAcls)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.DataMartEvents)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.ProjectDataMartEvents)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.DataMartRequestTypeAcls)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);
            HasMany(t => t.ProjectDataMartRequestTypeAcls)
                .WithRequired(t => t.DataMart)
                .HasForeignKey(t => t.DataMartID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.ChangeLogs).WithRequired(t => t.DataMart).HasForeignKey(t => t.DataMartID).WillCascadeOnDelete(true);
        }
    }

    internal class DataMartSecurityConfiguration : DnsEntitySecurityConfiguration<DataMart>
    {

        public override IQueryable<DataMart> SecureList(DataContext db, IQueryable<DataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[1] {
                    PermissionIdentifiers.DataMart.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params DataMart[] objs)
        {
            var perms = await db.HasGrantedPermissions<Organization>(identity, objs.Select(dm => dm.OrganizationID).ToArray(), PermissionIdentifiers.Organization.CreateDataMarts);

            return perms.Any();
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclDataMart, bool>> DataMartFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.DataMartID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMartFilter(Guid[] objIDs)
        {
            return a => a.Organization.DataMarts.Any(dm => objIDs.Contains(a.DataMartID)) && objIDs.Contains(a.DataMartID);
        }

        public override System.Linq.Expressions.Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return a => a.Project.DataMarts.Any(dm => objIDs.Contains(dm.DataMartID)) && objIDs.Contains(a.DataMartID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.DataMarts.Any(dm => objIDs.Contains(dm.ID));
        }
    }

    internal class DataMartListDTOMapping : EntityMappingConfiguration<DataMart, DataMartListDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataMart, DataMartListDTO>> MapExpression
        {
            get
            {
                return d => new DataMartListDTO
                {
                    Acronym = d.Acronym,
                    Description = d.Description,
                    EndDate = d.EndDate,
                    ID = d.ID,
                    Name = d.Name,
                    StartDate = d.StartDate,
                    Timestamp = d.Timestamp,
                    OrganizationID = d.OrganizationID,
                    Organization = d.Organization.Name,
                    ParentOrganziationID = d.Organization.ParentOrganizationID,
                    ParentOrganization = d.Organization.ParentOrganization.Name
                };
            }
        }
    }

    internal class DataMartDTOMapping : EntityMappingConfiguration<DataMart, DataMartDTO>
    {
        public override System.Linq.Expressions.Expression<Func<DataMart, DataMartDTO>> MapExpression
        {
            get
            {
                return (dm) => new DataMartDTO
                {
                    Acronym = dm.Acronym,
                    AvailablePeriod = dm.AvailablePeriod,
                    ContactEmail = dm.ContactEmail,
                    ContactFirstName = dm.ContactFirstName,
                    ContactLastName = dm.ContactLastName,
                    ContactPhone = dm.ContactPhone,
                    DataMartTypeID = dm.DataMartTypeID,
                    DataModel = dm.DataModel,
                    DataUpdateFrequency = dm.DataUpdateFrequency,
                    Deleted = dm.Deleted,
                    DataMartType = dm.DataMartType.Name,
                    Description = dm.Description,
                    EndDate = dm.EndDate,
                    HealthPlanDescription = dm.HealthPlanDescription,
                    ID = dm.ID,
                    InpatientEHRApplication = dm.InpatientEHRApplication,
                    IsGroupDataMart = dm.IsGroupDataMart,
                    IsLocal = dm.IsLocal,
                    Url = dm.Url,
                    Name = dm.Name,
                    OrganizationID = dm.OrganizationID,
                    Organization = dm.Organization.Name,
                    OtherClaims = dm.OtherClaims,
                    OtherDataModel = dm.OtherDataModel,
                    OtherInpatientEHRApplication = dm.OtherInpatientEHRApplication,
                    OtherOutpatientEHRApplication = dm.OtherOutpatientEHRApplication,
                    OutpatientEHRApplication = dm.OutpatientEHRApplication,
                    RequiresApproval = dm.RequiresApproval,
                    SpecialRequirements = dm.SpecialRequirements,
                    StartDate = dm.StartDate,
                    Timestamp = dm.Timestamp,
                    UnattendedMode = dm.UnattendedMode,
                    UsageRestrictions = dm.UsageRestrictions,

                    LaboratoryResultsAny = dm.LaboratoryResultsAny,
                    LaboratoryResultsClaims = dm.LaboratoryResultsClaims,
                    LaboratoryResultsTestName = dm.LaboratoryResultsTestName,
                    LaboratoryResultsDates = dm.LaboratoryResultsDates,
                    LaboratoryResultsTestLOINC = dm.LaboratoryResultsTestLOINC,
                    LaboratoryResultsTestSNOMED = dm.LaboratoryResultsTestSNOMED,
                    LaboratoryResultsSpecimenSource = dm.LaboratoryResultsSpecimenSource,
                    LaboratoryResultsTestDescriptions = dm.LaboratoryResultsTestDescriptions,
                    LaboratoryResultsOrderDates = dm.LaboratoryResultsOrderDates,
                    LaboratoryResultsTestResultsInterpretation = dm.LaboratoryResultsTestResultsInterpretation,
                    LaboratoryResultsTestOther = dm.LaboratoryResultsTestOther,
                    LaboratoryResultsTestOtherText = dm.LaboratoryResultsTestOtherText,
                    InpatientEncountersAny = dm.InpatientEncountersAny,
                    InpatientEncountersEncounterID = dm.InpatientEncountersEncounterID,
                    InpatientEncountersProviderIdentifier = dm.InpatientEncountersProviderIdentifier,
                    InpatientDatesOfService = dm.InpatientDatesOfService,
                    InpatientICD9Procedures = dm.InpatientICD9Procedures,
                    InpatientICD10Procedures = dm.InpatientICD10Procedures,
                    InpatientICD9Diagnosis = dm.InpatientICD9Diagnosis,
                    InpatientICD10Diagnosis = dm.InpatientICD10Diagnosis,
                    InpatientSNOMED = dm.InpatientSNOMED,
                    InpatientHPHCS = dm.InpatientHPHCS,
                    InpatientDisposition = dm.InpatientDisposition,
                    InpatientDischargeStatus = dm.InpatientDischargeStatus,
                    InpatientOther = dm.InpatientOther,
                    InpatientOtherText = dm.InpatientOtherText,
        
                    OutpatientEncountersAny = dm.OutpatientEncountersAny,
                    OutpatientEncountersEncounterID = dm.OutpatientEncountersEncounterID,
                    OutpatientEncountersProviderIdentifier = dm.OutpatientEncountersProviderIdentifier,
                    OutpatientClinicalSetting = dm.OutpatientClinicalSetting,
                    OutpatientDatesOfService = dm.OutpatientDatesOfService,
                    OutpatientICD9Procedures = dm.OutpatientICD9Procedures,
                    OutpatientICD10Procedures = dm.OutpatientICD10Procedures,
                    OutpatientICD9Diagnosis = dm.OutpatientICD9Diagnosis,
                    OutpatientICD10Diagnosis = dm.OutpatientICD10Diagnosis,
                    OutpatientSNOMED = dm.OutpatientSNOMED,
                    OutpatientHPHCS = dm.OutpatientHPHCS,
                    OutpatientOther = dm.OutpatientOther,
                    OutpatientOtherText = dm.OutpatientOtherText,
                    ERPatientID = dm.ERPatientID,
                    EREncounterID = dm.EREncounterID,
                    EREnrollmentDates = dm.EREnrollmentDates,
                    EREncounterDates = dm.EREncounterDates,
                    ERClinicalSetting = dm.ERClinicalSetting,
                    ERICD9Diagnosis = dm.ERICD9Diagnosis,
                    ERICD10Diagnosis = dm.ERICD10Diagnosis,
                    ERHPHCS = dm.ERHPHCS,
                    ERNDC = dm.ERNDC,
                    ERSNOMED = dm.ERSNOMED,
                    ERProviderIdentifier = dm.ERProviderIdentifier,
                    ERProviderFacility = dm.ERProviderFacility,
                    EREncounterType = dm.EREncounterType,
                    ERDRG = dm.ERDRG,
                    ERDRGType = dm.ERDRGType,
                    EROther = dm.EROther,
                    EROtherText = dm.EROtherText,
                    DemographicsAny = dm.DemographicsAny,
                    DemographicsPatientID = dm.DemographicsPatientID,
                    DemographicsSex = dm.DemographicsSex,
                    DemographicsDateOfBirth = dm.DemographicsDateOfBirth,
                    DemographicsDateOfDeath = dm.DemographicsDateOfDeath,
                    DemographicsAddressInfo = dm.DemographicsAddressInfo,
                    DemographicsRace = dm.DemographicsRace,
                    DemographicsEthnicity = dm.DemographicsEthnicity,
                    DemographicsOther = dm.DemographicsOther,
                    DemographicsOtherText = dm.DemographicsOtherText,
                    PatientOutcomesAny = dm.PatientOutcomesAny,
                    PatientOutcomesInstruments = dm.PatientOutcomesInstruments,
                    PatientOutcomesInstrumentText = dm.PatientOutcomesInstrumentText,
                    PatientOutcomesHealthBehavior = dm.PatientOutcomesHealthBehavior,
                    PatientOutcomesHRQoL = dm.PatientOutcomesHRQoL,
                    PatientOutcomesReportedOutcome = dm.PatientOutcomesReportedOutcome,
                    PatientOutcomesOther = dm.PatientOutcomesOther,
                    PatientOutcomesOtherText = dm.PatientOutcomesOtherText,
                    PatientBehaviorHealthBehavior = dm.PatientBehaviorHealthBehavior,
                    PatientBehaviorInstruments = dm.PatientBehaviorInstruments,
                    PatientBehaviorInstrumentText = dm.PatientBehaviorInstrumentText,
                    PatientBehaviorOther = dm.PatientBehaviorOther,
                    PatientBehaviorOtherText = dm.PatientBehaviorOtherText,
                    VitalSignsAny = dm.VitalSignsAny,
                    VitalSignsTemperature = dm.VitalSignsTemperature,
                    VitalSignsHeight = dm.VitalSignsHeight,
                    VitalSignsWeight = dm.VitalSignsWeight,
                    VitalSignsBMI = dm.VitalSignsBMI,
                    VitalSignsBloodPressure = dm.VitalSignsBloodPressure,
                    VitalSignsOther = dm.VitalSignsOther,
                    VitalSignsOtherText = dm.VitalSignsOtherText,
                    VitalSignsLength = dm.VitalSignsLength,
                    PrescriptionOrdersAny = dm.PrescriptionOrdersAny,
                    PrescriptionOrderDates = dm.PrescriptionOrderDates,
                    PrescriptionOrderRxNorm = dm.PrescriptionOrderRxNorm,
                    PrescriptionOrderNDC = dm.PrescriptionOrderNDC,
                    PrescriptionOrderOther = dm.PrescriptionOrderOther,
                    PrescriptionOrderOtherText = dm.PrescriptionOrderOtherText,
                    PharmacyDispensingAny = dm.PharmacyDispensingAny,
                    PharmacyDispensingDates = dm.PharmacyDispensingDates,
                    PharmacyDispensingRxNorm = dm.PharmacyDispensingRxNorm,
                    PharmacyDispensingDaysSupply = dm.PharmacyDispensingDaysSupply,
                    PharmacyDispensingAmountDispensed = dm.PharmacyDispensingAmountDispensed,
                    PharmacyDispensingNDC = dm.PharmacyDispensingNDC,
                    PharmacyDispensingOther = dm.PharmacyDispensingOther,
                    PharmacyDispensingOtherText = dm.PharmacyDispensingOtherText,
                    BiorepositoriesAny = dm.BiorepositoriesAny,
                    BiorepositoriesName = dm.BiorepositoriesName,
                    BiorepositoriesDescription = dm.BiorepositoriesDescription,
                    BiorepositoriesDiseaseName = dm.BiorepositoriesDiseaseName,
                    BiorepositoriesSpecimenSource = dm.BiorepositoriesSpecimenSource,
                    BiorepositoriesSpecimenType = dm.BiorepositoriesSpecimenType,
                    BiorepositoriesProcessingMethod = dm.BiorepositoriesProcessingMethod,
                    BiorepositoriesSNOMED = dm.BiorepositoriesSNOMED,
                    BiorepositoriesStorageMethod = dm.BiorepositoriesStorageMethod,
                    BiorepositoriesOther = dm.BiorepositoriesOther,
                    BiorepositoriesOtherText = dm.BiorepositoriesOtherText,
                    LongitudinalCaptureAny = dm.LongitudinalCaptureAny,
                    LongitudinalCapturePatientID = dm.LongitudinalCapturePatientID,
                    LongitudinalCaptureStart = dm.LongitudinalCaptureStart,
                    LongitudinalCaptureStop = dm.LongitudinalCaptureStop,
                    LongitudinalCaptureOther = dm.LongitudinalCaptureOther,
                    LongitudinalCaptureOtherValue = dm.LongitudinalCaptureOtherValue,
                    AdapterID = dm.AdapterID,
                    Adapter = dm.Adapter.Name,
                    ProcessorID = dm.ProcessorID,
                    DataPartnerIdentifier = dm.DataPartnerIdentifier,
                    DataPartnerCode = dm.DataPartnerCode
                };
            }
        }
    }

    internal class DataMartLogConfiguration : EntityLoggingConfiguration<DataContext, DataMart>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var dm = obj.Entity as DataMart;
            if (dm == null)
                throw new InvalidCastException("The entity passed is not a DataMart");

            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault() ?? new { UserName = "<unknown>", Acronym = "<unknown>"};

            var logItem = new Audit.DataMartChangeLog
            {
                Description = string.Format("DataMart '{0}' has been {1} by {2}", dm.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                Reason = obj.State,
                UserID = identity == null ? Guid.Empty : identity.ID,
                DataMartID = dm.ID,
                DataMart = dm
            };

            db.LogsDataMartChange.Add(logItem);
            logs.Add(logItem);

            return logs.AsEnumerable();

        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.DataMartChangeLog))
            {
                var log = logItem as Audit.DataMartChangeLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault() ?? new User { FirstName = string.Empty, LastName = "<unknown>" };

                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>DataMart Change</b> notifications from <b>" + networkName + "</b>.</p>" + 
                           "<p>A change has been made to the <b>" + log.DataMart.Name + "</b> DataMart by <b>" + actingUser.FullName + "</b>.</p>";


                var notification = new Notification
                {
                    Subject = "DataMart Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.DataMart.Change.ID && !s.User.Deleted && s.User.Active &&
                                       (
                                          (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.DataMart.Change.ID 
                                                                         && a.OrganizationID == log.DataMart.OrganizationID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                        || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.DataMart.Change.ID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                          )
                                       &&
                                          (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.DataMart.Change.ID 
                                                                         && a.OrganizationID == log.DataMart.OrganizationID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                        && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.DataMart.Change.ID 
                                                                         && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                          )
                                       )
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  select new Recipient
                                       {
                                           Email = s.User.Email,
                                           Phone = s.User.Phone,
                                           Name = s.User.FirstName + " " + s.User.LastName,
                                           UserID = s.UserID
                                       }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.NewDataMartClientLog))
            {
                var log = logItem as Audit.NewDataMartClientLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>New DataMart Client Available</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p>A new version of the DataMart Client is now available. </p>" +
                           "<p>To download the most recent version, please visit the Resources page on the portal. </p>" +
                           "<p>For instructions on installing and configuring the DataMart Client, please see the <a href='https://popmednet.atlassian.net/wiki/'>PopMedNet Wiki</a></p>";


                var notification = new Notification
                {
                    Subject = "New DataMart Client Available Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                        where s.EventID == EventIdentifiers.DataMart.NewDataMartAvailable.ID 
                                        && !db.LogsNewDataMartClient.Any(l => l.UserID == s.UserID && l.LastModified >= log.LastModified)
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  select new Recipient
                                        {
                                            Email = s.User.Email,
                                            Phone = s.User.Phone,
                                            Name = s.User.FirstName + " " + s.User.LastName,
                                            UserID = s.UserID
                                        }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the DataMart Logging Configuration");

        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsDataMartChange.Include(x => x.DataMart) select l, db.UserEventSubscriptions, EventIdentifiers.DataMart.Change.ID).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log, db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            const string DmcFilesFolder = "~/DataMartClient";
            var lastModified = FilterAuditLog(from l in db.LogsNewDataMartClient select l, db.UserEventSubscriptions, EventIdentifiers.DataMart.NewDataMartAvailable.ID).Select(l => (DateTime?) l.LastModified).Max() ?? DateTime.MinValue;

            try
            {
                var currentModified = new DirectoryInfo(HttpContext.Current.Server.MapPath(DmcFilesFolder)).GetFiles().Where(f => f.LastWriteTimeUtc > lastModified.AddMinutes(5)).Select(f => (DateTime?)f.LastWriteTimeUtc).Max() ?? DateTime.MinValue;

                var newDMCLogs = await (from oldLog in FilterAuditLog(from l in db.LogsNewDataMartClient select l, db.UserEventSubscriptions, EventIdentifiers.DataMart.NewDataMartAvailable.ID) join s in db.UserEventSubscriptions on oldLog.UserID equals s.UserID where s.EventID == EventIdentifiers.DataMart.NewDataMartAvailable.ID && oldLog.LastModified < currentModified select s).ToArrayAsync();

                var firstRun = true;
                foreach (var newlog in newDMCLogs)
                {
                    var log = new Audit.NewDataMartClientLog
                    {
                        Description = "A new DataMart Client is available for download: " + currentModified.ToString(),
                        LastModified = currentModified,
                        TimeStamp = DateTime.UtcNow,
                        UserID = newlog.UserID
                    };

                    if (firstRun)
                    {
                        firstRun = false;
                        var notification = CreateNotifications(log, db, false);
                        if (notification != null && notification.Any())
                            notifications.AddRange(notification);
                        db.LogsNewDataMartClient.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                //Log that we couldn't find the data mart client here.
            }

            return notifications.AsEnumerable();

        }
    }
}
