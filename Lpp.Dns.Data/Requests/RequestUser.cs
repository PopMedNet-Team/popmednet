using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Objects;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lpp.Dns.Data
{
    [Table("RequestUsers")]
    public class RequestUser : Entity
    {
        [Key, Column(Order = 1)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }
        [Key, Column(Order = 2)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }
        [Key, Column(Order = 3)]
        public Guid WorkflowRoleID { get; set; }
        public virtual WorkflowRole WorkflowRole {get; set;}
    }

    internal class RequestUserMappingConfiguration : EntityMappingConfiguration<RequestUser, RequestUserDTO>{

        public override System.Linq.Expressions.Expression<Func<RequestUser, RequestUserDTO>> MapExpression
        {
            get
            {
                return (r) => new RequestUserDTO {
                    Email = r.User.Email,
                    FullName = ((r.User.FirstName + " " + r.User.MiddleName).Trim() + " " + r.User.LastName).Trim(),
                    IsRequestCreatorRole = r.WorkflowRole.IsRequestCreator,
                    RequestID = r.RequestID,
                    UserID = r.UserID,
                    Username = r.User.UserName,
                    WorkflowRole = r.WorkflowRole.Name,
                    WorkflowRoleID = r.WorkflowRoleID
                };
            }
        }
    }

    public class RequestUserLogConfiguration : EntityLoggingConfiguration<DataContext, RequestUser>
    {
        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, Utilities.Security.ApiIdentity identity, bool read)
        {
            var requestUser = obj.Entity as RequestUser;
            if (requestUser == null)
                throw new InvalidCastException("Invalid entity to log, expecting RequestUser.");

            var logs = new List<AuditLog>();

            logs.Add(CreateLogItem(requestUser, obj.State, identity, db, true));

            return logs;
        }

        public AuditLog CreateLogItem(RequestUser requestUser, EntityState entityState, Utilities.Security.ApiIdentity identity, DataContext db, bool addToDbContext = true)
        {
            var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();
            var assignee = db.Users.Where(u => u.ID == requestUser.UserID).Select(u => new { u.UserName, Organization = u.Organization.Acronym }).FirstOrDefault();
            
            var role = db.WorkflowRoles.Where(r => r.ID == requestUser.WorkflowRoleID).Select(r => r.Name).FirstOrDefault();

            string description = string.Format(@"{0} assigned as {1}\{2} by {3}\{4}", role, assignee.Organization, assignee.UserName, orgUser.Acronym, orgUser.UserName);
            if (entityState == EntityState.Deleted)
            {
                description = string.Format(@"{2}, {0}\{1}, has been un-assigned from a request by {3}\{4}", assignee.Organization, assignee.UserName, role, orgUser.Acronym, orgUser.UserName);
            }

            var log = new Audit.RequestAssignmentChangeLog{
                                                            Description = description,
                                                            Reason = entityState,
                                                            UserID = identity.ID,
                                                            RequestID = requestUser.RequestID,
                                                            RequestUserUserID = requestUser.UserID,
                                                            WorkflowRoleID = requestUser.WorkflowRoleID
                                                        };

            if (addToDbContext)
            {
                db.LogsRequestAssignmentChange.Add(log);
            }

            return log;
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) != typeof(Audit.RequestAssignmentChangeLog))
                return null;

            
            var log = logItem as Audit.RequestAssignmentChangeLog;            
            var actingUser = db.Users.Find(log.UserID);
            db.Entry(log).Reference(l => l.WorkflowRole).Load();
            db.Entry(log).Reference(l => l.RequestUserUser).Load();

            var requestDetails = db.Requests.Where(r => r.ID == log.RequestID).Select(r => new { NetworkName = db.Networks.Select(n => n.Name).FirstOrDefault(), RequestTypeName = r.RequestType.Name, RequestName = r.Name, ProjectName = r.Project.Name }).Single();

            //Do not send notification if the assignment change is Request Creator
            if (log.WorkflowRole.Name == "Request Creator")
                return null;

            var body = GenerateTimestampText(log) + 
                "<p>Here are your most recent <b>Request Assigned</b> notifications from <b>" + requestDetails.NetworkName + "</b>.</p>" +
                "<p><b>" + log.RequestUserUser.FullName + (log.Reason == EntityState.Deleted ? "</b> has been un-assigned from <b>" : "</b> has been assigned as <b>" + log.WorkflowRole.Name + "</b> for <b>" ) + requestDetails.RequestTypeName +
                "</b> request <b>" + requestDetails.RequestName + "</b> by <b>" + actingUser.FullName + "</b> in project <b>" + requestDetails.ProjectName + "</b>.</p>";
            var myBody = GenerateTimestampText(log) +
                "<p>Here are your most recent <b>Request Assigned to me</b> notifications from <b>" + requestDetails.NetworkName + "</b>.</p>" +
                "<p><b>" + (log.Reason == EntityState.Deleted ? "</b>You have been un-assigned from <b>" : "</b>You have been assigned as <b>" + log.WorkflowRole.Name + "</b> for <b>") + requestDetails.RequestTypeName +
                "</b> request <b>" + requestDetails.RequestName + "</b> by <b>" + actingUser.FullName + "</b> in project <b>" + requestDetails.ProjectName + "</b>.</p>";

            //User is not being assigned to the request and is subscribed to the general notification
            var recipients = (from s in db.UserEventSubscriptions
                              where s.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                              (
                                 (db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Project.Requests.Any(r => r.ID == log.RequestID)
                                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                               || db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID)
                                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                               || db.UserEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                 )
                                &&
                                (
                                    db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Project.Requests.Any(r => r.ID == log.RequestID)
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID)
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                && db.UserEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.UserID == s.UserID
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && a.User.Active)).All(a => a.Allowed)
                                 )
                              )
                              //user is not being assigned the request OR user is being assigned to the request but has not subscribed to the "My" notification
                              && ((!(log.RequestUserUserID == s.UserID))
                                  ||
                                 (log.RequestUserUserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                                )
                              && ((!immediate && s.NextDueTime <= DateTime.UtcNow) || s.Frequency == Frequencies.Immediately)
                              select new Recipient
                              {
                                  Email = s.User.Email,
                                  Phone = s.User.Phone,
                                  Name = s.User.FirstName + " " + s.User.LastName,
                                  UserID = s.UserID
                              }).ToArray();

            var userObservers = (from u in db.Users
                                 let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == log.RequestID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                 where requestObservers.Any()
                                 select new Recipient
                                 {
                                     Email = u.Email,
                                     Phone = u.Phone,
                                     Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                     UserID = u.ID
                                 }).ToArray();

            var emailObservers = (from o in db.RequestObservers
                                  where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID)
                                  && o.UserID.HasValue == false && o.SecurityGroupID.HasValue == false
                                  && o.Email != "" && o.RequestID == log.RequestID
                                  select new Recipient
                                  {
                                      Email = o.Email,
                                      Phone = "",
                                      Name = o.DisplayName,
                                      UserID = null
                                  }).ToArray();

            recipients = recipients.Union(userObservers).Union(emailObservers).ToArray();

            //user is being assigned to the request and is subscribed to the "My" notification
            var requestUsers = (from s in db.UserEventSubscriptions
                              where s.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && !s.User.Deleted && s.User.Active && s.FrequencyForMy != null &&
                              (
                                 (db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Project.Requests.Any(r => r.ID == log.RequestID)
                                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                               || db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID)
                                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                               || db.UserEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.UserID == s.UserID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                 )
                                &&
                                (
                                    db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Project.Requests.Any(r => r.ID == log.RequestID)
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID)
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                && db.UserEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestAssignmentChange.ID && a.UserID == s.UserID
                                        && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && a.User.Active)).All(a => a.Allowed)
                                 )
                              )
                              //Request Assigned to you notification should only be sent to assignee
                              && log.RequestUserUserID == s.UserID
                              && ((!immediate && s.NextDueTimeForMy <= DateTime.UtcNow) || s.FrequencyForMy == Frequencies.Immediately)

                              select new Recipient
                              {
                                  Email = s.User.Email,
                                  Phone = s.User.Phone,
                                  Name = s.User.FirstName + " " + s.User.LastName,
                                  UserID = s.UserID
                              }).ToArray();

            var notification = new Notification
            {
                Subject = "Request Assigned Notification",
                Body = body,
                Recipients = recipients
            };
            var myNotification = new Notification
            {
                Subject = "Request Assigned to You Notification",
                Body = myBody,
                Recipients = requestUsers
            };
            IList<Notification> notifies = new List<Notification>();
            if (notification.Recipients.Any())
            {
                notifies.Add(notification);

            }
            if (myNotification.Recipients.Any())
            {
                notifies.Add(myNotification);

            }
            return notifies.AsEnumerable();
        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsRequestAssignmentChange.Include(x => x.Request) select l, db.UserEventSubscriptions, Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestAssignmentChange.ID).GroupBy(g => new { g.RequestID, g.UserID }).ToArrayAsync();

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

