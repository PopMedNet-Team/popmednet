using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("Organizations")]
    public class Organization : EntityWithID, IEntityWithDeleted, IEntityWithName, Lpp.Security.ISecurityObject
    {
        public Organization() {
            this.ModifiedOn = DateTime.UtcNow;
            this.Primary = false;
            this.DataMarts = new HashSet<DataMart>();
            this.Registries = new HashSet<OrganizationRegistry>();
            this.EHRSes = new HashSet<OrganizationEHRS>();
        }

        [MaxLength(255), Required]
        public string Name { get; set; }

        [MaxLength(100), Required(AllowEmptyStrings=true)]
        public string Acronym { get; set; }

        public Guid? ParentOrganizationID { get; set; }
        public virtual Organization ParentOrganization { get; set; }

        public bool Primary { get; set; }

        [Column("IsDeleted")]
        public bool Deleted { get; set; }

        [Column("IsApprovalRequired")]
        public bool ApprovalRequired { get; set; }

        [MaxLength(510)]
        public string ContactEmail { get; set; }

        [MaxLength(100)]//to nvarchar
        public string ContactFirstName { get; set; }

        [MaxLength(100)]//to nvarchar
        public string ContactLastName { get; set; }

        [MaxLength(15)]//to nvarchar
        public string ContactPhone { get; set; }

        [MaxLength(1000)]//to nvarchar
        public string SpecialRequirements { get; set; }

        [MaxLength(1000)]//to nvarchar
        public string UsageRestrictions { get; set; }

        [MaxLength(1000)]
        public string HealthPlanDescription { get; set; }

        public bool EnableClaimsAndBilling { get; set; }

        public bool EnableEHRA {get; set;}

        public bool EnableRegistries { get; set; }

        public bool DataModelMSCDM { get; set; }

        public bool DataModelPCORI { get; set; }

        public bool DataModelHMORNVDW { get; set; }

        public bool DataModelESP { get; set; }

        public bool DataModelI2B2 { get; set; }

        public bool DataModelOMOP { get; set; }

        public bool PragmaticClinicalTrials { get; set; }

        //Types of Data Collected
        public bool Biorepositories { get; set; }
        public bool PatientReportedOutcomes { get; set; }
        public bool PatientReportedBehaviors { get; set; }
        public bool PrescriptionOrders { get; set; }        

        [MaxLength(512)]
        public string InpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string OutpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string OtherInpatientEHRApplication { get; set; }

        [MaxLength(512)]
        public string OtherOutpatientEHRApplication { get; set; }

        public bool InpatientClaims { get; set; }

        public bool OutpatientClaims { get; set; }
        public bool OutpatientPharmacyClaims { get; set; }
        public bool ObservationalParticipation { get; set; }
        public bool ProspectiveTrials { get; set; }
        public bool EnrollmentClaims { get; set; }
        public bool DemographicsClaims { get; set; }
        public bool LaboratoryResultsClaims { get; set; }
        public bool VitalSignsClaims { get; set; }
        public bool OtherClaims { get; set; }
        [MaxLength(80)]
        public string OtherClaimsText { get; set; }
        
        [MaxLength(1000)]
        public string ObservationClinicalExperience { get; set; }
        public bool DataModelOther { get; set; }
        [MaxLength(80)]
        public string DataModelOtherText { get; set; }

        [MaxLength(255)]
        public string X509PublicKey { get; set; }

        [MaxLength(255)]
        public string X509PrivateKey { get; set; }

        [MaxLength]
        public string OrganizationDescription { get; set; }

        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<DataMart> DataMarts { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<OrganizationRegistry> Registries { get; set; }
        public virtual ICollection<OrganizationEHRS> EHRSes { get; set; }
        public virtual ICollection<Organization> DependantOrganizations { get; set; }
        public virtual ICollection<OrganizationGroup> Groups { get; set; }
        public virtual ICollection<ProjectOrganization> Projects { get; set; }

        public virtual ICollection<AclProjectOrganization> ProjectOrganizationAcls { get; set; }

        public virtual ICollection<AclOrganization> OrganizationAcls { get; set; }
        public virtual ICollection<AclOrganizationDataMart> OrganizationDataMarts { get; set; }
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual ICollection<Audit.OrganizationChangedLog> ChangeLogs { get; set; }

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("Organization");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get {
                return ObjectKind;
            }
        }
    }

    internal class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
        public OrganizationConfiguration()
        {
            HasMany(t => t.DataMarts).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);

            HasMany(t => t.Requests).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);

            HasMany(t => t.Users).WithOptional(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);

            HasMany(t => t.Registries).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);

            HasMany(t => t.EHRSes).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);

            HasMany(t => t.DependantOrganizations)
                .WithOptional(t => t.ParentOrganization)
                .HasForeignKey(t => t.ParentOrganizationID)
                .WillCascadeOnDelete(false);

            HasMany(t => t.Groups)
                .WithRequired(t => t.Organization)
                .HasForeignKey(t => t.OrganizationID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.OrganizationAcls)
                .WithRequired(t => t.Organization)
                .HasForeignKey(t => t.OrganizationID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.OrganizationDataMarts)
                .WithRequired(t => t.Organization)
                .HasForeignKey(t => t.OrganizationID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.OrganizationEvents)
                .WithRequired(t => t.Organization)
                .HasForeignKey(t => t.OrganizationID)
                .WillCascadeOnDelete(true);
            HasMany(t => t.ProjectOrganizationAcls)
                .WithRequired(t => t.Organization)
                .HasForeignKey(t => t.OrganizationID)
                .WillCascadeOnDelete(true);
            HasMany(t => t.Projects).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);
            HasMany(t => t.ChangeLogs).WithRequired(t => t.Organization).HasForeignKey(t => t.OrganizationID).WillCascadeOnDelete(true);
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

    internal class OrganizationDtoMappingConfiguration : EntityMappingConfiguration<Organization, OrganizationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Organization, OrganizationDTO>> MapExpression
        {
            get
            {
                return (o) => new OrganizationDTO
                {
                    Acronym = o.Acronym,
                    DataModelESP = o.DataModelESP,
                    DataModelHMORNVDW = o.DataModelHMORNVDW,
                    DataModelI2B2 = o.DataModelI2B2,
                    DataModelMSCDM = o.DataModelMSCDM,
                    DataModelOMOP = o.DataModelOMOP,
                    DataModelPCORI = o.DataModelPCORI,
                    DataModelOther = o.DataModelOther,
                    DataModelOtherText = o.DataModelOtherText,
                    EnableClaimsAndBilling = o.EnableClaimsAndBilling,
                    EnableEHRA = o.EnableEHRA,
                    EnableRegistries = o.EnableRegistries,
                    ID = o.ID,
                    Name = o.Name,
                    ParentOrganization = o.ParentOrganization.Name,
                    ParentOrganizationID = o.ParentOrganizationID,
                    Primary = o.Primary,
                    X509PublicKey = o.X509PublicKey,
                    Deleted = o.Deleted,
                    ContactEmail = o.ContactEmail,
                    ContactFirstName = o.ContactFirstName,
                    ContactLastName = o.ContactLastName,
                    ContactPhone = o.ContactPhone,
                    SpecialRequirements = o.SpecialRequirements,
                    UsageRestrictions = o.UsageRestrictions,
                    OrganizationDescription = o.OrganizationDescription,
                    PragmaticClinicalTrials = o.PragmaticClinicalTrials,
                    ObservationalParticipation = o.ObservationalParticipation,
                    ProspectiveTrials = o.ProspectiveTrials,
                    InpatientClaims = o.InpatientClaims,
                    OutpatientClaims = o.OutpatientClaims,
                    OutpatientPharmacyClaims = o.OutpatientPharmacyClaims,
                    EnrollmentClaims = o.EnrollmentClaims,
                    DemographicsClaims =  o.DemographicsClaims,
                    LaboratoryResultsClaims = o.LaboratoryResultsClaims,
                    VitalSignsClaims = o.VitalSignsClaims,
                    OtherClaims = o.OtherClaims,
                    OtherClaimsText = o.OtherClaimsText,
                    Biorepositories = o.Biorepositories,
                    PatientReportedBehaviors = o.PatientReportedBehaviors,
                    PatientReportedOutcomes = o.PatientReportedOutcomes,
                    PrescriptionOrders = o.PrescriptionOrders
                };
            }
        }
    }

    internal class OrganizationLogConfiguration : EntityLoggingConfiguration<DataContext, Organization>
    {
        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var organization = obj.Entity as Organization;
            if (organization == null)
                throw new InvalidCastException("The entity passed is not an organization");

            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

            var logItem = new Audit.OrganizationChangedLog
            {
                Description = string.Format("Organization '{0}' has been {1} by {2}", organization.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                Reason = obj.State,
                UserID = identity == null ? Guid.Empty : identity.ID,
                OrganizationID = organization.ID,
                Organization = organization
            };

            db.LogsOrganizationChange.Add(logItem);
            logs.Add(logItem);

            return logs.AsEnumerable();

        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.OrganizationChangedLog))
            {
                var log = logItem as Audit.OrganizationChangedLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                if (log.Organization == null)
                    log.Organization = db.Organizations.Find(log.OrganizationID);
                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>Organization Change</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p>A change has been made to <b>" + log.Organization.Name + "</b> organization by <b>" + actingUser.FullName + "</b>.</p>";

                var notification = new Notification
                {
                    Subject = "Organization Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                       where s.EventID == EventIdentifiers.Organization.Change.ID && !s.User.Deleted && s.User.Active &&
                                       (
                                          (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Organization.Change.ID && a.OrganizationID == log.OrganizationID 
                                                                              && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                        || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Organization.Change.ID 
                                                                              && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()                                           
                                          )
                                       &&
                                          (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Organization.Change.ID && a.OrganizationID == log.OrganizationID 
                                                                              && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed) 
                                        && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Organization.Change.ID 
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

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Organization Logging Configuration");

        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsOrganizationChange.Include(x => x.Organization) select l, db.UserEventSubscriptions, EventIdentifiers.Organization.Change.ID).GroupBy(g => new { g.OrganizationID, g.UserID }).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications.AsEnumerable();

        }
    }
}
