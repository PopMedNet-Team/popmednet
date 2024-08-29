using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("Comments")]
    public partial class Comment : EntityWithID
    {
        public Comment()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Text { get; set; }
        public Guid ItemID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedByID { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual ICollection<CommentReference> References { get; set; }
    }

    internal class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            this.HasMany(c => c.References).WithRequired(r => r.Comment).HasForeignKey(r => r.CommentID).WillCascadeOnDelete(true);
        }
    }

    internal class CommentSecurityConfiguration : DnsEntitySecurityConfiguration<Comment>
    {

        public override IQueryable<Comment> SecureList(DataContext db, IQueryable<Comment> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments,
                    PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.AddComments
                };

            return query.Where(q =>
                db.Filter(db.WorkflowActivities, identity, permissions)
                    .Any(c => c.ID == db.Actions.Where(t => t.ID == q.ItemID).Select(t => t.WorkflowActivityID).FirstOrDefault())
                //Add other ors here.
            );
        }

        //These permissions should all be extended to take into account the ItemID and what it's parent is.
        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Comment[] objs)
        {
            return TaskEx.FromResult(true);
            //return HasPermissions(db, identity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.AddComments);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return TaskEx.FromResult(true);

            //return HasPermissions(db, identity, keys, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.AddComments);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return TaskEx.FromResult(true);

            //return HasPermissions(db, identity, keys, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.AddComments);
        }
    }

    internal class CommentDtoMappingConfiguration : EntityMappingConfiguration<Comment, CommentDTO>
    {
        public override Expression<Func<Comment, CommentDTO>> MapExpression
        {
            get
            {
                return r => new CommentDTO
                {
                    Comment = r.Text,
                    ItemID = r.ItemID,
                    CreatedOn = r.CreatedOn,
                    CreatedByID = r.CreatedByID,
                    CreatedBy = r.CreatedBy.UserName
                };
            }
        }
    }

    internal class CommentLogConfiguration : EntityLoggingConfiguration<DataContext, Comment>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var comment = obj.Entity as Comment;
            if (comment == null)
                throw new InvalidCastException("The entity being logged is not a Comment.");

            var logs = new List<AuditLog>();


            var request = db.Requests.Find(comment.ItemID);
            if (request != null && obj.State == System.Data.Entity.EntityState.Added)
            {

                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                logs.Add(
                    db.LogsRequestCommentChange.Add(
                        new Audit.RequestCommentChangeLog
                        {
                            CommentID = comment.ID,
                            Description = string.Format("A comment has been {2} to request {0} by {1}", request.Name, (orgUser.Acronym + @"\" + orgUser.UserName), obj.State.ToString()),
                            Reason = obj.State,                            
                            UserID = identity == null ? Guid.Empty : identity.ID,
                        }
                    )
                );
            }

            return logs;
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.RequestCommentChangeLog))
            {
                var log = logItem as Audit.RequestCommentChangeLog;

                db.Entry(log).Reference(l => l.Comment).Load();

                var workflowTask = (from cr in db.CommentReferences
                                    join t in db.Actions.Include(t => t.WorkflowActivity) on cr.ItemID equals t.ID
                                    where cr.CommentID == log.CommentID && cr.Type == CommentItemTypes.Task
                                    select t).FirstOrDefault();

                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                //comment.ItemID is always going to be the Request, task is associated using a comment reference of type Task.
                var details = (from r in db.Requests
                               where r.ID == log.Comment.ItemID
                               select new
                               {
                                   ProjectID = r.ProjectID,
                                   ProjectName = r.Project.Name,
                                   OrganizationID = r.OrganizationID,
                                   RequestTypeName = r.RequestType.Name,
                                   RequestTypeID = r.RequestTypeID,
                                   RequestName = r.Name,
                                   CreatedByID = r.CreatedByID
                               }).Single();

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

                var body = GenerateTimestampText(log) +
                    "<p>Here are your most recent <b>New Comment</b> notifications from " + networkName + ".</p>" +
                    "<p><b>" + actingUser.FullName + "</b> has commented on " + details.RequestTypeName + "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName + "</b>.</p>" +
                    "<p><b>Comment</b><br/><br/>" +
                    log.Comment.Text.Replace(Environment.NewLine, "<br/>") + "</p>";

                var myBody = GenerateTimestampText(log) +
                    "<p>Here are your most recent <b>New Comment</b> on My Request notifications from " + networkName + ".</p>" +
                    "<p><b>" + actingUser.FullName + "</b> has commented on your " + details.RequestTypeName + "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName + "</b>.</p>" +
                    "<p><b>Comment</b><br/><br/>" +
                    log.Comment.Text.Replace(Environment.NewLine, "<br/>") + "</p>";

                Guid requestID = log.Comment.ItemID;

                //user is not a requestUser and has subscribed to the general notification
                var recipients = from s in db.UserEventSubscriptions
                                 where s.EventID == EventIdentifiers.Request.RequestCommentChange.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                                 (
                                    (
                                        db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.UserEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                    )
                                    &&
                                    (
                                        db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.UserEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                    )
                                 )
                                 &&
                                 (
                                     (
                                        from r in db.Requests.Where(r => r.ID == requestID)
                                        let globalAcls = db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                        let projectAcls = db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Project.Requests.Any(rq => rq.ID == requestID) && a.ProjectID == details.ProjectID)
                                        let organizationAcls = db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Organization.Requests.Any(rq => rq.ID == requestID) && a.OrganizationID == details.OrganizationID)
                                        where
                                        (
                                            globalAcls.Any() || projectAcls.Any() || organizationAcls.Any()
                                        )
                                        &&
                                        (
                                            globalAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && organizationAcls.All(a => a.Allowed)
                                        )
                                        select r
                                     ).Any()
                                     ||
                                     db.Requests.Any(r => r.ID == requestID && (r.CreatedByID == s.UserID || r.SubmittedByID == s.UserID))
                                 )
                                 //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                                 && (!db.RequestUsers.Any(ru => ru.RequestID == requestID && ru.UserID == s.UserID)
                                    ||
                                    db.RequestUsers.Any(ru => ru.RequestID == requestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                                 )
                                 && ((!immediate && (Frequencies)s.Frequency != Frequencies.Immediately && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (immediate && (Frequencies)s.Frequency == Frequencies.Immediately))
                                 select s;

                //user is a request user and has subscribed to the "My" notification
                var requestUsers = from s in db.UserEventSubscriptions
                                   where s.EventID == EventIdentifiers.Request.RequestCommentChange.ID && !s.User.Deleted && s.User.Active && s.FrequencyForMy != null &&
                                 (
                                    (
                                        db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                     || db.UserEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                    )
                                    &&
                                    (
                                        db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Organization.Requests.Any(r => r.ID == requestID) && a.OrganizationID == details.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.Project.Requests.Any(r => r.ID == requestID) && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                     && db.UserEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestCommentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID)).All(a => a.Allowed)
                                    )
                                 )
                                 &&
                                 (
                                     (
                                        from r in db.Requests.Where(r => r.ID == requestID)
                                        let globalAcls = db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID))
                                        let projectAcls = db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Project.Requests.Any(rq => rq.ID == requestID) && a.ProjectID == details.ProjectID)
                                        let organizationAcls = db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewRequest.ID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID) && a.Organization.Requests.Any(rq => rq.ID == requestID) && a.OrganizationID == details.OrganizationID)
                                        where
                                        (
                                            globalAcls.Any() || projectAcls.Any() || organizationAcls.Any()
                                        )
                                        &&
                                        (
                                            globalAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && organizationAcls.All(a => a.Allowed)
                                        )
                                        select r
                                     ).Any()
                                     ||
                                     db.Requests.Any(r => r.ID == requestID && (r.CreatedByID == s.UserID || r.SubmittedByID == s.UserID))
                                 )
                                 && db.RequestUsers.Any(ru => ru.RequestID == requestID && ru.UserID == s.UserID)
                                 && ((!immediate && (Frequencies)s.FrequencyForMy != Frequencies.Immediately && (s.NextDueTimeForMy == null || s.NextDueTimeForMy <= DateTime.UtcNow)) || (immediate && (Frequencies)s.FrequencyForMy == Frequencies.Immediately))
                                 select s;

                if (workflowTask != null)
                {
                    //make sure the recipient has permission to view task documents for the specified workflow activity the task is for
                    recipients = from s in recipients
                                 let acl = db.ProjectRequestTypeWorkflowActivities.Where(p => p.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments &&
                                                                                              p.SecurityGroup.Users.Any(u => u.UserID == s.UserID) &&
                                                                                              p.ProjectID == details.ProjectID &&
                                                                                              p.RequestTypeID == details.RequestTypeID &&
                                                                                              p.WorkflowActivityID == workflowTask.WorkflowActivityID
                                                                                         )
                                 where acl.Any() && acl.All(a => a.Allowed)
                                 select s;
                    //make sure the recipient has permission to view task documents for the specified workflow activity the task is for
                    requestUsers = from s in requestUsers
                                 let acl = db.ProjectRequestTypeWorkflowActivities.Where(p => p.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.ViewComments &&
                                                                                              p.SecurityGroup.Users.Any(u => u.UserID == s.UserID) &&
                                                                                              p.ProjectID == details.ProjectID &&
                                                                                              p.RequestTypeID == details.RequestTypeID &&
                                                                                              p.WorkflowActivityID == workflowTask.WorkflowActivityID
                                                                                         )
                                 where acl.Any() && acl.All(a => a.Allowed)
                                 select s;
                }

                var notificationRecipients = recipients.Select(s => new Recipient
                                                {
                                                    Email = s.User.Email,
                                                    Phone = s.User.Phone,
                                                    Name = s.User.FirstName + " " + s.User.LastName,
                                                    UserID = s.UserID
                                                }).ToArray();

                var userObservers = (from u in db.Users
                                     let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == log.Comment.ItemID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestCommentChange.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                     where requestObservers.Any()
                                     select new Recipient
                                     {
                                         Email = u.Email,
                                         Phone = u.Phone,
                                         Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                         UserID = u.ID
                                     }).ToArray();

                var emailObservers = (from o in db.RequestObservers
                                      where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestCommentChange.ID)
                                      && o.UserID.HasValue == false && o.SecurityGroupID.HasValue == false
                                      && o.Email != "" && o.RequestID == log.Comment.ItemID
                                      select new Recipient
                                      {
                                          Email = o.Email,
                                          Phone = "",
                                          Name = o.DisplayName,
                                          UserID = null
                                      }).ToArray();

                notificationRecipients = notificationRecipients.Union(userObservers).Union(emailObservers).ToArray();

                var notification = new Notification
                {
                    Subject = "New Comment on Request Notification",
                    Body = body,
                    Recipients = notificationRecipients
                };
                var myNotification = new Notification
                {
                    Subject = "New Comment on Your Request Notification",
                    Body = myBody,
                    Recipients = requestUsers.Select(s => new Recipient
                    {
                        Email = s.User.Email,
                        Phone = s.User.Phone,
                        Name = s.User.FirstName + " " + s.User.LastName,
                        UserID = s.UserID
                    }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                if (notification.Recipients.Any()) {
                    notifies.Add(notification);
                    
                }
                if (myNotification.Recipients.Any())
                {
                    notifies.Add(myNotification);

                }
                return notifies.AsEnumerable();
            }

            return null;
        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsRequestCommentChange.Include(l => l.Comment) select l, db.UserEventSubscriptions, Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestCommentChange.ID).GroupBy(g => new { g.CommentID, g.UserID }).ToArrayAsync();

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
