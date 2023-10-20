using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("Groups")]
    public class Group : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted, Lpp.Security.ISecurityObject
    {
        public Group() {
            this.Deleted = false;
            this.ApprovalRequired = true;
            this.Name = string.Empty;
            this.Projects = new HashSet<Project>();
        }

        [Required, MaxLength(255)]
        public string Name { get; set;}

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column("isApprovalRequired")]
        public bool ApprovalRequired { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<OrganizationGroup> Organizations { get; set; }
        public virtual ICollection<AclGroup> GroupAcls { get; set; }
        public virtual ICollection<GroupEvent> GroupEvents { get; set; }


        public virtual ICollection<Audit.GroupChangeLog> ChangeLogs { get; set; }

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("Group");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }
    }

    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            HasMany(t => t.Projects).WithOptional(t => t.Group).HasForeignKey(t => t.GroupID).WillCascadeOnDelete(false);

            HasMany(t => t.Organizations)
                .WithRequired(t => t.Group)
                .HasForeignKey(t => t.GroupID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.GroupAcls).WithRequired(t => t.Group)
                .HasForeignKey(t => t.GroupID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.GroupEvents)
                .WithRequired(t => t.Group)
                .HasForeignKey(t => t.GroupID)
                .WillCascadeOnDelete(true);

            HasMany(t => t.ChangeLogs).WithRequired(t => t.Group).HasForeignKey(t => t.GroupID).WillCascadeOnDelete(true);
        }
    }

    internal class GroupSecurityConfiguration : DnsEntitySecurityConfiguration<Group>
    {
        public override IQueryable<Group> SecureList(DataContext db, IQueryable<Group> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Group[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.CreateGroups);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.GroupID);
        }
    }

    internal class GroupDTOMappingConfiguration : EntityMappingConfiguration<Group, GroupDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Group, GroupDTO>> MapExpression
        {
            get
            {
                return (q) => new GroupDTO {
                    ApprovalRequired = q.ApprovalRequired,
                    Deleted = q.Deleted,
                    ID = q.ID,
                    Name = q.Name,
                    Timestamp = q.Timestamp
                };
            }
        }
    }

    internal class GroupLogConfiguration : EntityLoggingConfiguration<DataContext, Group>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var group = obj.Entity as Group;
            if (group == null)
                throw new InvalidCastException("The entity passed is not a group");
                        
            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault() ?? new { UserName = "<unknown>", Acronym = "<unknown>" };

            var logItem = new Audit.GroupChangeLog
            {
                Description = string.Format("Group '{0}' has been {1} by {2}", group.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                Reason = obj.State,
                UserID = identity == null ? Guid.Empty : identity.ID,
                GroupID = group.ID,
                Group = group
            };

            db.LogsGroupChange.Add(logItem);
            logs.Add(logItem);

            return logs.AsEnumerable();
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.GroupChangeLog))
            {
                var log = logItem as Audit.GroupChangeLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault() ?? new User { FirstName = string.Empty, LastName = "<unknown>" };

                if (log.Group == null)
                    log.Group = db.Groups.Find(log.GroupID);
                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>Group Change</b> notifications from " + networkName + ".</p>" +
                           "<p>A change has been made to the " + log.Group.Name + " group by " + actingUser.FullName + ".</p>"; 


                var notification = new Notification
                {
                    Subject = "Group Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.Group.Change.ID && !s.User.Deleted && s.User.Active &&
                                       (
                                           (db.GroupEvents.Any(a => a.EventID == EventIdentifiers.Group.Change.ID && a.GroupID == log.GroupID 
                                               && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                         || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Group.Change.ID 
                                               && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                           )
                                       &&
                                           (db.GroupEvents.Where(a => a.EventID == EventIdentifiers.Group.Change.ID && a.GroupID == log.GroupID
                                               && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                         && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Group.Change.ID
                                             && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
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

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Group Logging Configuration");
        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsGroupChange.Include(x => x.Group) select l, db.UserEventSubscriptions, EventIdentifiers.Group.Change.ID).GroupBy(g => new { g.GroupID, g.UserID }).ToArrayAsync();

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
