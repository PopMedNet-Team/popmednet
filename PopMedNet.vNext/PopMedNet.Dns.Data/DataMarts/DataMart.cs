using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Dns.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("DataMarts")]
    public partial class DataMart : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted
    {
        public DataMart()
        {
            this.ModifiedOn = DateTime.UtcNow;
            RequiresApproval = false;
            Deleted = false;
            IsGroupDataMart = false;
            IsLocal = false;
        }

        [MaxLength(255), Required]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Url { get; set; }

        public bool RequiresApproval { get; set; }

        [MaxLength]
        public string? DataMartDescription { get; set; }

        public Guid? DataMartTypeID { get; set; }
        public virtual DataMartType? DataMartType { get; set; }       

        [MaxLength(500), Column(TypeName = "varchar")]
        public string? AvailablePeriod { get; set; }

        [MaxLength(510), Column(TypeName = "varchar")]
        public string? ContactEmail { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string? ContactFirstName { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string? ContactLastName { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        public string? ContactPhone { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string? SpecialRequirements { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string? UsageRestrictions { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [MaxLength(1000), Column(TypeName = "varchar")]
        public string? HealthPlanDescription { get; set; }

        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public bool? IsGroupDataMart { get; set; }
        public UnattendedModes? UnattendedMode { get; set; } = UnattendedModes.NoUnattendedOperation;
        [MaxLength]
        public string? Description { get; set; }

        [MaxLength(100), Required(AllowEmptyStrings = true)]
        public string Acronym { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(100), Column(TypeName = "varchar")]
        public string? DataUpdateFrequency { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? InpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? OutpatientEHRApplication { get; set; }

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
        public string? LaboratoryResultsTestOtherText { get; set; }

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
        public string? InpatientOtherText { get; set; }

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
        public string? OutpatientOtherText { get; set; }

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
        public string? EROtherText { get; set; }

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
        public string? DemographicsOtherText { get; set; }

        public bool PatientOutcomesAny { get; set; }
        public bool PatientOutcomesInstruments { get; set; }

        [MaxLength(80)]
        public string? PatientOutcomesInstrumentText { get; set; }
        public bool PatientOutcomesHealthBehavior { get; set; }
        public bool PatientOutcomesHRQoL { get; set; }
        public bool PatientOutcomesReportedOutcome { get; set; }
        public bool PatientOutcomesOther { get; set; }

        [MaxLength(80)]
        public string? PatientOutcomesOtherText { get; set; }

        public bool PatientBehaviorHealthBehavior { get; set; }
        public bool PatientBehaviorInstruments { get; set; }

        [MaxLength(80)]
        public string? PatientBehaviorInstrumentText { get; set; }
        public bool PatientBehaviorOther { get; set; }

        [MaxLength(80)]
        public string? PatientBehaviorOtherText { get; set; }

        public bool VitalSignsAny { get; set; }
        public bool VitalSignsTemperature { get; set; }
        public bool VitalSignsHeight { get; set; }
        public bool VitalSignsWeight { get; set; }
        public bool VitalSignsBMI { get; set; }
        public bool VitalSignsBloodPressure { get; set; }
        public bool VitalSignsOther { get; set; }

        [MaxLength(80)]
        public string? VitalSignsOtherText { get; set; }
        public bool VitalSignsLength { get; set; }

        public bool PrescriptionOrdersAny { get; set; }
        public bool PrescriptionOrderDates { get; set; }
        public bool PrescriptionOrderRxNorm { get; set; }
        public bool PrescriptionOrderNDC { get; set; }
        public bool PrescriptionOrderOther { get; set; }

        [MaxLength(80)]
        public string? PrescriptionOrderOtherText { get; set; }

        public bool PharmacyDispensingAny { get; set; }
        public bool PharmacyDispensingDates { get; set; }
        public bool PharmacyDispensingRxNorm { get; set; }
        public bool PharmacyDispensingDaysSupply { get; set; }
        public bool PharmacyDispensingAmountDispensed { get; set; }
        public bool PharmacyDispensingNDC { get; set; }
        public bool PharmacyDispensingOther { get; set; }

        [MaxLength(80)]
        public string? PharmacyDispensingOtherText { get; set; }

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
        public string? BiorepositoriesOtherText { get; set; }

        public bool LongitudinalCaptureAny { get; set; }
        public bool LongitudinalCapturePatientID { get; set; }
        public bool LongitudinalCaptureStart { get; set; }
        public bool LongitudinalCaptureStop { get; set; }
        public bool LongitudinalCaptureOther { get; set; }

        [MaxLength(80)]
        public string? LongitudinalCaptureOtherValue { get; set; }

        [MaxLength(80), Column(TypeName = "nvarchar")]
        public string? OtherClaims { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? OtherInpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? OtherOutpatientEHRApplication { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? DataModel { get; set; }

        [MaxLength(512), Column(TypeName = "varchar")]
        public string? OtherDataModel { get; set; }

        public bool IsLocal { get; set; } = false;

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public Guid? AdapterID { get; set; }
        public virtual DataModel? Adapter { get; set; }

        public Guid? ProcessorID { get; set; }
        /// <summary>
        /// Gets or sets the data partner identifier, used by distributed regression to specify the folder to download the partner output files to at the analysis center.
        /// </summary>
        [MaxLength(255)]
        public string? DataPartnerIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the data partner code, used by distributed regression to specify the partner code used to identify the partner in the tracking tables.
        /// </summary>
        [MaxLength(255)]
        public string? DataPartnerCode { get; set; }

        public virtual ICollection<RequestDataMart> Requests { get; set; } = new HashSet<RequestDataMart>();
        public virtual ICollection<ProjectDataMart> Projects { get; set; } = new HashSet<ProjectDataMart>();
        public virtual ICollection<DataMartInstalledModel> Models { get; set; } = new HashSet<DataMartInstalledModel>();
        public virtual ICollection<DataMartAvailabilityPeriod_v2> DataMartAvailabilityPeriods_v2 { get; set; } = new HashSet<DataMartAvailabilityPeriod_v2>();
        public virtual ICollection<AclDataMart> DataMartAcls { get; set; } = new HashSet<AclDataMart>();
        public virtual ICollection<AclOrganizationDataMart> OrganizationDataMarts { get; set; } = new HashSet<AclOrganizationDataMart>();
        public virtual ICollection<AclProjectDataMart> ProjectDataMartAcls { get; set; } = new HashSet<AclProjectDataMart>();
        public virtual ICollection<DataMartEvent> DataMartEvents { get; set; } = new HashSet<DataMartEvent>();
        public virtual ICollection<ProjectDataMartEvent> ProjectDataMartEvents { get; set; } = new HashSet<ProjectDataMartEvent>();
        public virtual ICollection<AclDataMartRequestType> DataMartRequestTypeAcls { get; set; } = new HashSet<AclDataMartRequestType>();
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; } = new HashSet<AclProjectDataMartRequestType>();
        public virtual ICollection<Audit.DataMartChangeLog> ChangeLogs { get; set; } = new HashSet<Audit.DataMartChangeLog>();
    }

    internal class DataMartConfiguration : IEntityTypeConfiguration<DataMart>
    {
        public void Configure(EntityTypeBuilder<DataMart> builder)
        {
            builder.HasMany(t => t.Requests)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Projects)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMartAvailabilityPeriods_v2)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Models)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMartAcls)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrganizationDataMarts)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectDataMartAcls)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMartEvents)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectDataMartEvents)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMartRequestTypeAcls)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(t => t.ProjectDataMartRequestTypeAcls)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ChangeLogs)
                .WithOne(t => t.DataMart)
                .IsRequired(true)
                .HasForeignKey(t => t.DataMartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.UnattendedMode).HasConversion<int>();

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
            return a => a.Organization!.DataMarts.Any(dm => objIDs.Contains(a.DataMartID)) && objIDs.Contains(a.DataMartID);
        }

        public override System.Linq.Expressions.Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return a => a.Project!.DataMarts.Any(dm => objIDs.Contains(dm.DataMartID)) && objIDs.Contains(a.DataMartID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization!.DataMarts.Any(dm => objIDs.Contains(dm.ID));
        }
    }

    public class DataMartMappingProfile : AutoMapper.Profile
    {
        public DataMartMappingProfile()
        {
            CreateMap<DataMart, DTO.DataMartDTO>()
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.Organization != null ? src.Organization!.Name : null))
                .ForMember(d => d.Adapter, opt => opt.MapFrom(src => src.AdapterID.HasValue ? src.Adapter!.Name : null))
                .ForMember(d => d.DataMartType, opt => opt.MapFrom(src => src.DataMartType != null ? src.DataMartType!.Name : null))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.StartDate.Value, TimeSpan.Zero) : null))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.EndDate.Value, TimeSpan.Zero) : null));

            CreateMap<DTO.DataMartDTO, DataMart>()
                .ForMember(d => d.Organization, opt => opt.Ignore())
                .ForMember(d => d.Adapter, opt => opt.Ignore())
                .ForMember(d => d.DataMartType, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? (DateTime?)src.StartDate.Value.Date : null))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? (DateTime?)src.EndDate.Value.Date : null));

            CreateMap<DataMart, DTO.DataMartListDTO>()
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.Organization != null ? src.Organization!.Name : null))
                .ForMember(d => d.ParentOrganization, opt => opt.MapFrom(src => (src.Organization != null && src.Organization!.ParentOrganizationID.HasValue) ? src.Organization!.ParentOrganization!.Name : null))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.StartDate.Value, TimeSpan.Zero) : null))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? (DateTimeOffset?)new DateTimeOffset(src.EndDate.Value, TimeSpan.Zero) : null));
        }
    }
}
