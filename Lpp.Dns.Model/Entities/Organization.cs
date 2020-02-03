using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;
using System.Collections.ObjectModel;

namespace Lpp.Dns.Model
{
    [Table("Organizations")]
    public class Organization : ISecurityObject, ISecurityGroupAuthority<OrganizationSecurityGroup>, IHaveId<int>, IHaveDeletedFlag, INamed
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("Organization");

        public Organization()
        {
            this.Users = new HashSet<User>();
            this.Groups = new HashSet<Group>();
            this.DataMarts = new HashSet<DataMart>();
            this.Children = new HashSet<Organization>();
            this.SecurityGroups = new HashSet<OrganizationSecurityGroup>();
            this.Requests = new HashSet<Request>();
            this.Registries = new HashSet<OrganizationRegistry>();
            this.SID = UserDefinedFunctions.NewGuid();
            this.EHRSes = new HashSet<OrganizationEHRS>();
            this.InSearchResults = new HashSet<Request>();
        }

        [Key, Column("OrganizationId")]
        public int Id { get; set; }
        [Column("OrganizationName", TypeName = "varchar"), MaxLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
        [Column("OrganizationAcronym", TypeName = "varchar"), MaxLength(100)]
        public string Acronym { get; set; }

        public Guid SID { get; set; }
        [NotMapped]
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }

        public int? ParentID { get; set; }
        public virtual Organization Parent { get; set; }

        // TODO: Move this to an organizational profile object;  how best to handle domain specific atttributes?
        public bool InpatientClaims { get; set; }
        public bool OutpatientClaims { get; set; }
        public bool OutpatientPharmacyClaims { get; set; }

        public bool EnrollmentClaims { get; set; }
        public bool DemographicsClaims { get; set; }
        public bool LaboratoryResultsClaims { get; set; }
        public bool VitalSignsClaims { get; set; }
        public bool Biorepositories { get; set; }
        public bool PatientReportedOutcomes { get; set; }
        public bool PatientReportedBehaviors { get; set; }
        public bool PrescriptionOrders { get; set; }
        [MaxLength(80)]
        public string OtherClaims { get; set; }

        public bool EnableClaimsAndBilling { get; set; }
        public bool EnableEHRA { get; set; }

        [Column(TypeName = "varchar"), MaxLength(512)]
        public string InpatientEHRApplication { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
        public string OtherInpatientEHRApplication { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
        public string OutpatientEHRApplication { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
        public string OtherOutpatientEHRApplication { get; set; }
        [Column(TypeName = "varchar"), MaxLength(512)]
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
        public string OrganizationDescription { get; set; }
        [Column(TypeName = "varchar"), MaxLength(1000)]
        public string HealthPlanDescription { get; set; }
        [Column(TypeName = "varchar"), MaxLength(1000)]
        public string ObservationClinicalExperience { get; set; }
        public bool ObservationalParticipation { get; set; }
        public bool ProspectiveTrials { get; set; }
        public bool PragmaticClinicalTrials { get; set; }

        public bool DataModelMSCDM { get; set; }
        public bool DataModelHMORNVDW { get; set; }
        public bool DataModelESP { get; set; }
        public bool DataModelI2B2 { get; set; }
        public bool DataModelOMOP { get; set; }
        [MaxLength(80)]
        public string DataModelOther { get; set; }

        public bool Primary { get; set; }
        [MaxLength(255)]
        public string X509PublicKey { get; set; }
        [MaxLength(255)]
        public string X509PrivateKey { get; set; }

        public virtual ICollection<Organization> Children { get; set; }
        public virtual ICollection<OrganizationSecurityGroup> SecurityGroups { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<DataMart> DataMarts { get; set; }
        public virtual ICollection<OrganizationRegistry> Registries { get; set; }
        public virtual ICollection<OrganizationEHRS> EHRSes { get; set; }
        public virtual ICollection<Request> InSearchResults { get; set; }

        public override string ToString() { return Name; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class OrganizationPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var o = builder.Entity<Organization>();

            o.HasMany(t => t.DataMarts).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationId).WillCascadeOnDelete(true);

            o.HasMany(t => t.Groups).WithMany(g => g.Organizations)
                .Map(m => m.ToTable("OrganizationsGroups").MapLeftKey("OrganizationId").MapRightKey("GroupId"));

            o.HasMany(t => t.SecurityGroups).WithRequired(g => g.Parent).Map(m => m.MapKey("OrganizationId"));

            o.HasOptional(t => t.Parent).WithMany(g => g.Children).HasForeignKey(t => t.ParentID).WillCascadeOnDelete(false);

            o.HasMany(t => t.Registries).WithRequired(g => g.Organization).HasForeignKey(g => g.OrganizationID).WillCascadeOnDelete(true);

            o.HasMany(t => t.EHRSes).WithRequired(g => g.Organization).HasForeignKey(g => g.OrganizationID).WillCascadeOnDelete(true);

            o.HasMany(t => t.Users).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationId).WillCascadeOnDelete(true);

            o.HasMany(t => t.Requests).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationId).WillCascadeOnDelete(true);
        }
    }
}
