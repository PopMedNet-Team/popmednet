using Lpp.Dns.DTO;
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
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using System.Data.Entity.Infrastructure;
using Lpp.Dns.DTO.Events;
using System.Web.Configuration;
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    [Table("Registries")]
    public class Registry : EntityWithID, IEntityWithDeleted, IEntityWithName, Lpp.Security.ISecurityObject
    {
        public Registry() { }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column(TypeName="tinyint")]
        public RegistryTypes Type { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength]
        public string Description { get; set; }

        [MaxLength(500)]
        public string RoPRUrl { get; set; }

        public virtual ICollection<OrganizationRegistry> Organizations { get; set; }

        public virtual ICollection<RegistryItemDefinition> Items { get; set; }

        public virtual ICollection<AclRegistry> RegistryAcls { get; set; }
        public virtual ICollection<RegistryEvent> RegistryEvents { get; set; }

        public virtual ICollection<Audit.RegistryChangeLog> RegistryChangeLogs { get; set; }

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("Registry");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }

    }

    internal class RegistryConfiguration : EntityTypeConfiguration<Registry>
    {
        public RegistryConfiguration()
        {
            HasMany(t => t.Organizations).WithRequired(t => t.Registry).HasForeignKey(t => t.RegistryID).WillCascadeOnDelete(true);

            HasMany(t => t.Items).WithMany(i => i.Registries).Map(m => m.ToTable("RegistryDefinitions").MapLeftKey("RegistryID").MapRightKey("RegistryItemDefinitionID"));

            HasMany(t => t.RegistryAcls)
                .WithRequired(t => t.Registry)
                .HasForeignKey(t => t.RegistryID)
                .WillCascadeOnDelete(true);
            HasMany(t => t.RegistryEvents)
                .WithRequired(t => t.Registry)
                .HasForeignKey(t => t.RegistryID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.RegistryChangeLogs).WithRequired(t => t.Registry).HasForeignKey(t => t.RegistryID).WillCascadeOnDelete(true);
        }
    }

    internal class RegistrySecurityConfiguration : DnsEntitySecurityConfiguration<Registry>
    {
        public override IQueryable<Registry> SecureList(DataContext db, IQueryable<Registry> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Registry.View                    
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Registry[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateRegistries);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclRegistry, bool>> RegistryFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.RegistryID);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Registries.Any(r => objIDs.Contains(r.RegistryID));
        }
    }

    internal class RegistryLoggingConfiguration : EntityLoggingConfiguration<DataContext, Registry>
    {

        public override IEnumerable<AuditLog> ProcessEvents(DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();
            if (obj.State == EntityState.Deleted)
                return logs;
            var registry = obj.Entity as Registry;
            if (registry == null)
                throw new InvalidCastException("The entity passed is not a registry");

            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

            var logItem = new Audit.RegistryChangeLog
            {
                Description = string.Format("Registry '{0}' has been {1} by {2}", registry.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                Reason = obj.State,
                RegistryID = registry.ID,
                Registry = registry,
                UserID = identity == null ? Guid.Empty : identity.ID
            };

            //Create the log item for change.
            db.LogsRegistryChange.Add(logItem);

            logs.Add(logItem);

            return logs.AsEnumerable();
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.RegistryChangeLog))
            {
                var log = logItem as Audit.RegistryChangeLog;

                var registry = db.Registries.Find(log.RegistryID);

                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var body = GenerateTimestampText(log) +
                    "<p>Here are your most recent <b>Registry Change</b> notifications.</p>" +
                    "<p>A change has been made to the <b>" + registry.Name + "</b> registry by <b>" + actingUser.FullName + "</b>.</p>";


                var notification = new Notification
                {
                    Subject = "Registry Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.Registry.Change.ID && !s.User.Deleted && s.User.Active &&
                                  (
                                      (
                                            db.RegistryEvents.Where(a => a.RegistryID == log.RegistryID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).Any() && 
                                            db.RegistryEvents.Where(a => a.RegistryID == log.RegistryID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)                                            
                                       )
                                       ||
                                       (
                                            db.GlobalEvents.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).Any() &&
                                            db.RegistryEvents.Where(a => a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                        )
                                   )
                                   && ((!immediate && s.NextDueTime <= DateTime.UtcNow) || s.Frequency == Frequencies.Immediately)
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

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Registry Logging Configuration");

        }


        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsRegistryChange.Include(x => x.Registry) select l, db.UserEventSubscriptions, EventIdentifiers.Registry.Change.ID).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log, db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications.AsEnumerable();
        }
    }

    internal class RegistryDTOMappingConfiguration : EntityMappingConfiguration<Registry, RegistryDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Registry, RegistryDTO>> MapExpression
        {
            get
            {
                return (q) => new RegistryDTO
                {
                    Deleted = q.Deleted,
                    Type = q.Type,
                    Name = q.Name,
                    Description = q.Description,
                    RoPRUrl = q.RoPRUrl,
                    ID = q.ID,
                    Timestamp = q.Timestamp
                };
            }
        }
    }
}
