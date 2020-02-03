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
using Lpp.Utilities;
using Lpp.Dns.DTO.Security;
using LinqKit;
using Lpp.Dns.DTO.Enums;
using System.Linq.Expressions;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;

namespace Lpp.Dns.Data
{
    [Table("Projects")]
    public class Project : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted, Lpp.Security.ISecurityObject
    {
        [MaxLength(255), Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Acronym { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate {get; set;}

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column("isActive")]
        public bool Active { get; set; }

        public string Description { get; set; }

        public Guid? GroupID { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<ProjectDataMart> DataMarts { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<ProjectOrganization> Organizations { get; set; }
        public virtual ICollection<AclProject> ProjectAcls { get; set; }
        public virtual ICollection<AclProjectDataMart> ProjectDataMartAcls { get; set; }
        public virtual ICollection<ProjectEvent> ProjectEventAcls { get; set;}
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; }
        public virtual ICollection<AclProjectRequestType> ProjectRequestTypeAcls { get; set; }
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; }
        public virtual ICollection<Audit.ProjectChangeLog> ChangeLogs { get; set; }
        public virtual ICollection<ProjectRequestType> RequestTypes { get; set; }


        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("Project");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }
    }

    internal class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasMany(t => t.Requests).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);

            HasMany(t => t.DataMarts).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);

            HasMany(t => t.Activities).WithOptional(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);

            HasMany(t => t.ProjectAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectDataMartAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectEventAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);

            HasMany(t => t.ProjectDataMartRequestTypeAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.Organizations).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectRequestTypeAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.ChangeLogs).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestTypes).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls).WithRequired(t => t.Project).HasForeignKey(t => t.ProjectID).WillCascadeOnDelete(true);
        }
    }

    internal class ProjectSecurityConfiguration : DnsEntitySecurityConfiguration<Project>
    {
        public override IQueryable<Project> SecureList(DataContext db, IQueryable<Project> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View                    
                };

            return db.Filter(query, identity, permissions);
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Project[] objs)
        {
            if (objs.Any(p => p.GroupID == null))
                return false;

            var groupIDs = objs.Select(p => p.GroupID.Value).ToArray();

            var aclGroups = db.GroupAcls.FilterAcl(identity, PermissionIdentifiers.Group.CreateProjects).Where(g => groupIDs.Contains(g.GroupID));
            var aclGlobal = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Group.CreateProjects);

            return (aclGlobal.Any() && aclGlobal.All(a => a.Allowed)) ||
                (aclGroups.Any() && aclGroups.All(a => a.Allowed));
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Edit);
        }

        public override Expression<Func<AclProject, bool>> ProjectFilter(params Guid[] objIDs)
        {
            return p => objIDs.Contains(p.ProjectID);
        }

        public override Expression<Func<AclDataMart, bool>> DataMartFilter(params Guid[] objIDs)
        {
            return dm => dm.DataMart.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }

        public override Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return g => g.Group.Projects.Any(p => objIDs.Contains(p.ID));
        }

        public override Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return pdm => objIDs.Contains(pdm.ProjectID);
        }

        public override Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return o => o.Organization.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }

        public override Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMartFilter(params Guid[] objIDs)
        {
            return odm => odm.DataMart.Projects.Any(p => objIDs.Contains(p.ProjectID)) || odm.Organization.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }
    }

    internal class ProjectDtoMappingConfiguration : EntityMappingConfiguration<Project, ProjectDTO>
    {
        public override Expression<Func<Project, ProjectDTO>> MapExpression
        {
            get
            {
                return (p) => new ProjectDTO {
                    Acronym = p.Acronym,
                    Active = p.Active,
                    Deleted = p.Deleted,
                    Description = p.Description,
                    EndDate = p.EndDate,
                    ID = p.ID,
                    Name = p.Name,
                    StartDate = p.StartDate,
                    Timestamp = p.Timestamp,
                    GroupID = p.GroupID,
                    Group = p.Group.Name
                };
            }
        }
    }

    internal class ProjectLogConfiguration : EntityLoggingConfiguration<DataContext, Project>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var project = obj.Entity as Project;
            if (project == null)
                throw new InvalidCastException("The entity passed is not a project");

            if (obj.State != EntityState.Unchanged)
            {
                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                var userID = identity == null ? Guid.Empty : identity.ID;
                var logItem = db.LogsProjectChange.Find(userID, DateTimeOffset.UtcNow, project.ID);

                if (logItem == null)
                {
                    logItem = new Audit.ProjectChangeLog
                    {
                        Description = string.Format("The {0} project has been {1} by {2}.", project.Name, obj.State, (orgUser.Acronym + @"\" + orgUser.UserName)),
                        Reason = obj.State,
                        UserID = userID,
                        ProjectID = project.ID,
                        Project = project
                    };

                    db.LogsProjectChange.Add(logItem);
                    logs.Add(logItem);
                }
            }

            return logs.AsEnumerable();
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.ProjectChangeLog))
            {
                var log = logItem as Audit.ProjectChangeLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var body = GenerateTimestampText(log) + 
                    "<p>Here are your most recent <b>Project Change</b> notifications from <b>" + networkName + "</b>.</p>" +
                    "<p>A change has been made to the <b>" + (log.Project == null ? "Unknown Project" : log.Project.Name) + "</b> project by <b>" + actingUser.FullName + "</b>.</p>";

                var notification = new Notification
                {
                    Subject = "Project Change Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.Project.Change.ID && !s.User.Deleted && s.User.Active &&
                                        (
                                            (db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Project.Change.ID 
                                                                 && a.ProjectID == log.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)) 
                                          || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Project.Change.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).Any()
                                            )
                                        &&
                                            (db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Project.Change.ID 
                                                                 && a.ProjectID == log.ProjectID 
                                                                 && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                          && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Project.Change.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
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

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Group Logging Configuration");

        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsProjectChange.Include(x => x.Project) select l, db.UserEventSubscriptions, EventIdentifiers.Project.Change.ID).GroupBy(g => new { g.ProjectID, g.UserID }).ToArrayAsync();

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
