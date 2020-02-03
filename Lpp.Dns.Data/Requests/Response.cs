using Lpp.Dns.DTO;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations;
using Lpp.Workflow.Engine.Interfaces;
using Lpp.Utilities;
using System.Linq.Expressions;

namespace Lpp.Dns.Data
{
    [Table("RequestDataMartResponses")]
    public class Response : EntityWithID, IWorkflowEntity
    {
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart {get; set;}   

        public Guid? ResponseGroupID { get; set; }
        public virtual ResponseGroup ResponseGroup { get; set; }

        public Guid? RespondedByID { get; set; }
        public virtual User RespondedBy { get; set; }

        public DateTime? ResponseTime { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public int Count { get; set; }

        public DateTime SubmittedOn { get; set; }
        public Guid SubmittedByID { get; set; }
        public virtual User SubmittedBy { get; set; }

        public string SubmitMessage { get; set; }

        public string ResponseMessage { get; set; }

        [MaxLength]
        public string ResponseData { get; set; }

        public Guid? WorkflowID { get; set; }
        public virtual Workflow Workflow { get; set; }
        public Guid? WorkFlowActivityID { get; set; }
        public virtual WorkflowActivity WorkFlowActivity { get; set; }

        public virtual ICollection<RequestDocument> RequestDocument { get; set; }

        public virtual ICollection<ResponseSearchResult> SearchResults { get; set; }
        public virtual ICollection<Audit.ResponseViewedLog> ViewLogs { get; set; }
    }

    internal class ResponseConfiguration : EntityTypeConfiguration<Response>
    {
        public ResponseConfiguration()
        {
            HasMany(t => t.SearchResults)
                .WithRequired(t => t.Response)
                .HasForeignKey(t => t.ResponseID)
                .WillCascadeOnDelete(false);

            HasMany(t => t.ViewLogs).WithRequired(t => t.Response).HasForeignKey(t => t.ResponseID).WillCascadeOnDelete(true);
        }
    }

    internal class ResponseDtoMappingConfiguration : EntityMappingConfiguration<Response, ResponseDTO>
    {
        public override Expression<Func<Response, ResponseDTO>> MapExpression
        {
            /**NOTE: When adding new properties to Request make sure to update dbo.FilteredRequestList tvf to return the new column.
             * Otherwise you get an invalid column name when trying to map to RequestDTO going through the table value function. **/
            get
            {
                return r => new ResponseDTO
                {
                    Count = r.Count,
                    ID = r.ID,
                    RequestDataMartID = r.RequestDataMartID,
                    RespondedByID = r.RespondedByID,
                    ResponseGroupID = r.ResponseGroupID,
                    ResponseMessage = r.ResponseMessage,
                    ResponseTime = r.ResponseTime,
                    SubmitMessage = r.SubmitMessage,
                    SubmittedByID = r.SubmittedByID,
                    SubmittedOn = r.SubmittedOn,
                    Timestamp = r.Timestamp
                };
                /**NOTE: When adding new properties to Request make sure to update dbo.FilteredRequestList tvf to return the new column.
                * Otherwise you get an invalid column name when trying to map to RequestDTO going through the table value function. **/
            }
        }
    }


    internal class ResponseSecurityConfiguration : DnsEntitySecurityConfiguration<Response>
    {
        public override IQueryable<Response> SecureList(DataContext db, IQueryable<Response> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
            {
                return db.FilteredResponseList(identity.ID);
            }
            else
            {
                var filteredResponses = (// Allowed via requests
                                         from r in db.FilteredRequestList(identity.ID)
                                         from q in query
                                         where r.ID == q.RequestDataMart.RequestID
                                         select new { q, r }).Where(d => d.r != null).Select(d => d.q).Union(
                    // Allowed via project
                                             from p in db.Filter(db.Projects, identity, permissions)
                                             from pdm in p.DataMarts
                                             from q in query
                                             where pdm.ProjectID == q.RequestDataMart.Request.ProjectID && pdm.DataMartID == q.RequestDataMart.DataMartID
                                             select q).Distinct();

                return filteredResponses;
            }

            //return query.Where(r =>
            //    db.Filter(db.Requests, identity, permissions).Where(request => request.ID == r.RequestDataMart.RequestID).Any() 
            //    || db.Filter(db.Projects, identity, permissions).Where(
            //                                p => p.DataMarts.Any(pdm => pdm.ProjectID == r.RequestDataMart.Request.ProjectID && pdm.DataMartID == r.RequestDataMart.DataMartID)
            //          ).Any());

        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Response[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.RequestDataMart.DataMartID).ToArray(), PermissionIdentifiers.DataMartInProject.UploadResults);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Response does not have direct permissions for delete, check it's parent Request");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Response does not have direct permissions for update, check it's parent Request");
        }
    }

    internal class ResponseLogConfiguration : EntityLoggingConfiguration<DataContext, Response>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var response = obj.Entity as Response;
            if (response == null)
                throw new InvalidCastException("The entity passed is not a response");

            if (response.RequestDataMart == null)
            {
                response.RequestDataMart = (from rdm in db.RequestDataMarts.Include(x => x.Request).Include(x => x.DataMart) where rdm.ID == response.RequestDataMartID select rdm).FirstOrDefault();
            }
            else
            {
                if (response.RequestDataMart.Request == null)
                    response.RequestDataMart.Request = (from r in db.Requests where r.ID == response.RequestDataMart.RequestID select r).FirstOrDefault();
                if (response.RequestDataMart.DataMart == null)
                    response.RequestDataMart.DataMart = (from dm in db.DataMarts where dm.ID == response.RequestDataMart.DataMartID select dm).FirstOrDefault();
            }

            if (response.RequestDataMart == null || response.RequestDataMart.DataMart == null)
                return logs;

            if (read && response.RespondedByID.HasValue && !db.LogsResponseViewed.Any(rv => rv.ResponseID == response.ID && rv.UserID == identity.ID))
            {
                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                var log = new Lpp.Dns.Data.Audit.ResponseViewedLog
                {
                    Description = string.Format("Results Viewed: Acting User = {0}, Request = {1}, DataMart = {2}", (orgUser.Acronym + @"\" + orgUser.UserName), response.RequestDataMart.Request.Name, response.RequestDataMart.DataMart.Name),
                    ResponseID = response.ID,
                    TimeStamp = DateTime.UtcNow,
                    UserID = identity.ID
                };
                db.LogsResponseViewed.Add(log);
                logs.Add(log);
            }            

            return logs;
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            if (typeof(T) == typeof(Audit.ResponseViewedLog))
            {
                var log = logItem as Audit.ResponseViewedLog;

                if (log.Response == null)
                    log.Response = db.Responses.Find(log.ResponseID);

                if (log.Response.RequestDataMart == null)
                    log.Response.RequestDataMart = db.RequestDataMarts.Find(log.Response.RequestDataMartID);

                if (log.Response.RequestDataMart.Request == null)
                    log.Response.RequestDataMart.Request = db.Requests.Find(log.Response.RequestDataMart.RequestID);

                if (log.Response.RequestDataMart.DataMart == null)
                    log.Response.RequestDataMart.DataMart = db.DataMarts.Find(log.Response.RequestDataMart.DataMartID);


                var body = GenerateTimestampText(log) + 
                    "<p>The response on '" + log.Response.RequestDataMart.Request.Name + "' (" + log.Response.RequestDataMart.Request.Identifier + ") for DataMart '" 
                    + log.Response.RequestDataMart.DataMart.Name + "' has been viewed.</p>";

                var notification = new Notification
                {
                    Subject = "Results Viewed",
                    Body = body,
                    Recipients = (                        
                        from s in db.UserEventSubscriptions
                        let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                        let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Project.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                        let projectOrgAcls = db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.Project.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                           where s.EventID == EventIdentifiers.Response.ResultsViewed.ID && !s.User.Deleted && s.User.Active 
                                           && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                           && (
                                                (orgAcls.Any() || projectAcls.Any() || projectOrgAcls.Any())
                                                &&
                                                (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectOrgAcls.All(a => a.Allowed))
                                           )
                        from r in db.FilteredRequestListForEvent(s.UserID, log.Response.RequestDataMart.DataMartID) 
                        from rdm in db.RequestDataMarts
                            where rdm.RequestID == r.ID
                        from resp in rdm.Responses
                            where resp.ID == log.ResponseID
                        select new {r, s}).Where(sub => sub.r != null).Select(sub => 
                                       new Recipient
                                       {
                                           Email = sub.s.User.Email,
                                           Phone = sub.s.User.Phone,
                                           Name = sub.s.User.FirstName + " " + sub.s.User.LastName,
                                           UserID = sub.s.UserID
                                       }).Union(
                            from s in db.UserEventSubscriptions
                            join u in db.Users on s.UserID equals u.ID
                            let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Project.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            let projectOrgAcls = db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.Project.Requests.Any(r => r.ID == log.Response.RequestDataMart.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            where s.UserID == log.UserID && s.EventID == EventIdentifiers.Response.ResultsViewed.ID && !s.User.Deleted && s.User.Active &&
                                  ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  && (
                                        (orgAcls.Any() || projectAcls.Any() || projectOrgAcls.Any())
                                        &&
                                        (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectOrgAcls.All(a => a.Allowed))
                                    )
                            select new Recipient
                            {
                                Email = u.Email,
                                Phone = u.Phone,
                                Name = u.UserName,
                                UserID = u.ID
                            }
                        ).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Response Logging Configuration");
        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var notifications = new List<Notification>();
            var logs = await FilterAuditLog(from l in db.LogsResponseViewed.Include(x => x.Response.RequestDataMart.DataMart).Include(x => x.Response.RequestDataMart.Request) select l, db.UserEventSubscriptions, EventIdentifiers.Response.ResultsViewed.ID).GroupBy(g => new { g.ResponseID, g.UserID }).ToArrayAsync();

            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications;
        }
    }
}
