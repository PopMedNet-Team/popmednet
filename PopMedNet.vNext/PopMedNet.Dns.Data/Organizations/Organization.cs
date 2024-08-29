using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PopMedNet.Utilities;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Organizations")]
    public class Organization : EntityWithID, IEntityWithDeleted, IEntityWithName
    {
        public Organization()
        {
            this.ModifiedOn = DateTime.UtcNow;
            this.Primary = false;
        }

        [MaxLength(255), Required]
        public string Name { get; set; } = "";

        [MaxLength(100), Required(AllowEmptyStrings = true)]
        public string Acronym { get; set; } = "";

        public Guid? ParentOrganizationID { get; set; }
        public virtual Organization? ParentOrganization { get; set; }

        public bool Primary { get; set; } = false;

        [Column("IsDeleted")]
        public bool Deleted { get; set; } = false;

        [Column("IsApprovalRequired")]
        public bool ApprovalRequired { get; set; } = false;

        [MaxLength(510)]
        public string? ContactEmail { get; set; }

        [MaxLength(100)]//to nvarchar
        public string? ContactFirstName { get; set; }

        [MaxLength(100)]//to nvarchar
        public string? ContactLastName { get; set; }

        [MaxLength(15)]//to nvarchar
        public string? ContactPhone { get; set; }

        [MaxLength(1000)]//to nvarchar
        public string? SpecialRequirements { get; set; }

        [MaxLength(1000)]//to nvarchar
        public string? UsageRestrictions { get; set; }

        [MaxLength(1000)]
        public string? HealthPlanDescription { get; set; }

        public bool EnableClaimsAndBilling { get; set; } = false;

        public bool EnableEHRA { get; set; } = false;

        public bool EnableRegistries { get; set; } = false;

        public bool DataModelMSCDM { get; set; } = false;

        public bool DataModelPCORI { get; set; } = false;

        public bool DataModelHMORNVDW { get; set; } = false;

        public bool DataModelESP { get; set; } = false;

        public bool DataModelI2B2 { get; set; } = false;

        public bool DataModelOMOP { get; set; } = false;

        public bool PragmaticClinicalTrials { get; set; } = false;

        //Types of Data Collected
        public bool Biorepositories { get; set; } = false;
        public bool PatientReportedOutcomes { get; set; } = false;
        public bool PatientReportedBehaviors { get; set; } = false;
        public bool PrescriptionOrders { get; set; } = false;

        [MaxLength(512)]
        public string? InpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string? OutpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string? OtherInpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string? OtherOutpatientEHRApplication { get; set; }

        public bool InpatientClaims { get; set; } = false;

        public bool OutpatientClaims { get; set; } = false;
        public bool OutpatientPharmacyClaims { get; set; } = false;
        public bool ObservationalParticipation { get; set; } = false;
        public bool ProspectiveTrials { get; set; } = false;
        public bool EnrollmentClaims { get; set; } = false;
        public bool DemographicsClaims { get; set; } = false;
        public bool LaboratoryResultsClaims { get; set; } = false;
        public bool VitalSignsClaims { get; set; } = false;
        public bool OtherClaims { get; set; } = false;
        [MaxLength(80)]
        public string? OtherClaimsText { get; set; }

        [MaxLength(1000)]
        public string? ObservationClinicalExperience { get; set; }
        public bool DataModelOther { get; set; } = false;
        [MaxLength(80)]
        public string? DataModelOtherText { get; set; }

        [MaxLength(255)]
        public string? X509PublicKey { get; set; }

        [MaxLength(255)]
        public string? X509PrivateKey { get; set; }

        [MaxLength]
        public string? OrganizationDescription { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public virtual ICollection<DataMart> DataMarts { get; set; } = new HashSet<DataMart>();
        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public virtual ICollection<OrganizationRegistry> Registries { get; set; } = new HashSet<OrganizationRegistry>();
        public virtual ICollection<OrganizationEHRS> EHRSes { get; set; } = new HashSet<OrganizationEHRS>();
        public virtual ICollection<Organization> DependantOrganizations { get; set; } = new HashSet<Organization>();
        public virtual ICollection<OrganizationGroup> Groups { get; set; } = new HashSet<OrganizationGroup>();
        public virtual ICollection<ProjectOrganization> Projects { get; set; } = new HashSet<ProjectOrganization>();
        public virtual ICollection<AclProjectOrganization> ProjectOrganizationAcls { get; set; } = new HashSet<AclProjectOrganization>();
        public virtual ICollection<AclOrganization> OrganizationAcls { get; set; } = new HashSet<AclOrganization>();
        public virtual ICollection<AclOrganizationDataMart> OrganizationDataMarts { get; set; } = new HashSet<AclOrganizationDataMart>();
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; } = new HashSet<OrganizationEvent>();
        public virtual ICollection<Audit.OrganizationChangedLog> ChangeLogs { get; set; } = new HashSet<Audit.OrganizationChangedLog>();

    }

    internal class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(t => t.ID);
            //use the build to configure the object
            builder.HasMany(o => o.Users)
                .WithOne(u => u.Organization)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.DataMarts)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrganizationAcls)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Requests)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Registries)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.EHRSes)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DependantOrganizations)
                .WithOne(t => t.ParentOrganization)
                .IsRequired(false)
                .HasForeignKey(t => t.ParentOrganizationID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Groups)
                .WithOne(t => t.Organization).IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrganizationDataMarts)
                .WithOne(t => t.Organization).IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrganizationEvents)
                .WithOne(t => t.Organization).IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectOrganizationAcls)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Projects)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ChangeLogs)
                .WithOne(t => t.Organization)
                .IsRequired(true)
                .HasForeignKey(t => t.OrganizationID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    internal class OrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<Organization>
    {
        public override IQueryable<Organization> SecureList(DataContext db, IQueryable<Organization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[1] {
                    PermissionIdentifiers.Organization.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Organization[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateOrganizations);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Delete);
        }

        public override async Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            var result = await HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit);

            return result;
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.OrganizationID);
        }
    }

    public class OrganizationMappingProfile : AutoMapper.Profile
    {
        public OrganizationMappingProfile()
        {
            CreateMap<Organization, DTO.OrganizationDTO>()
                .ForMember(d => d.ParentOrganization, opt => opt.MapFrom(src => src.ParentOrganization != null ? src.ParentOrganization!.Name : null));

            CreateMap<OrganizationDTO, Organization>()
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.ParentOrganization, opt => opt.Ignore());
        }
    }
}
