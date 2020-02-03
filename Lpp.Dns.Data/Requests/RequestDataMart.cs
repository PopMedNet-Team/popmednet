using System.Data.Entity.ModelConfiguration;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Objects;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Events;
using System.Text.RegularExpressions;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Infrastructure;

namespace Lpp.Dns.Data
{
    [Table("RequestDataMarts")]
    public class RequestDataMart : EntityWithID
    {
        public RequestDataMart()
        {
            UpdatedOn = DateTime.UtcNow;
        }

        public Guid RequestID { get; set; }       
        public virtual Request Request { get; set; }

        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }

        [Column("QueryStatusTypeID")]
        public RoutingStatus Status { get; set; }

        [Column(TypeName = "tinyint")]
        public Priorities Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? RequestTime { get; set; }

        public DateTime? ResponseTime { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDetail { get; set; }

        public string RejectReason { get; set; }

        [Column("isResultsGrouped")]
        public bool? ResultsGrouped { get; set; }

        public DateTime UpdatedOn { get; set; }

        [Column("PropertiesXml")]
        public string Properties { get; set; }

        public RoutingType? RoutingType { get; set; }

        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<Audit.UploadedResultNeedsApprovalLog> UploadedResultNeedsApprovalLogs { get; set; }
        public virtual ICollection<Audit.RoutingStatusChangeLog> RoutingStatusChangeLogs { get; set; }
        public virtual ICollection<Audit.NewRequestSubmittedLog> NewRequestSubmittedLogs { get; set; }
        public virtual ICollection<Audit.RequestDataMartMetadataChangeLog> RequestDataMartMetadataChangeLogs { get; set; }

        public Response AddResponse(Guid submittedByID)
        {
            Response response = new Response { RequestDataMart = this, SubmittedByID = submittedByID, SubmittedOn = DateTime.UtcNow };

            if (this.Responses == null)
                this.Responses = new HashSet<Response>();

            if (this.Responses.Count == 0)
            {
                response.Count = 1;
            }
            else
            {
                response.Count = this.Responses.Max(r => r.Count) + 1;
            }           

            this.Responses.Add(response);

            return response;
        }   
     
        public static RequestDataMart Create(Guid requestID, Guid dataMartID, Guid submittedByID){
            RequestDataMart routing = new RequestDataMart { DataMartID = dataMartID, RequestID = requestID, Status = RoutingStatus.Draft };
            routing.AddResponse(submittedByID);
            return routing;
        }
    }

    internal class RequestDataMartConfiguration : EntityTypeConfiguration<RequestDataMart>
    {
        public RequestDataMartConfiguration()
        {
            HasMany(t => t.Responses)
                .WithRequired(t => t.RequestDataMart)
                .HasForeignKey(t => t.RequestDataMartID)
                .WillCascadeOnDelete(false);
            HasMany(t => t.UploadedResultNeedsApprovalLogs).WithRequired(t => t.RequestDataMart).HasForeignKey(t => t.RequestDataMartID).WillCascadeOnDelete(true);
            HasMany(t => t.RoutingStatusChangeLogs).WithRequired(t => t.RequestDataMart).HasForeignKey(t => t.RequestDataMartID).WillCascadeOnDelete(true);
            HasMany(t => t.NewRequestSubmittedLogs).WithRequired(t => t.RequestDataMart).HasForeignKey(t => t.RequestDataMartID).WillCascadeOnDelete(false);
            HasMany(t => t.RequestDataMartMetadataChangeLogs).WithRequired(t => t.RequestDataMart).HasForeignKey(t => t.RequestDataMartID).WillCascadeOnDelete(true);
        }
    }

    internal class RequestDataMartDtoMappingConfiguration : EntityMappingConfiguration<RequestDataMart, RequestDataMartDTO>
    {
        public override System.Linq.Expressions.Expression<Func<RequestDataMart, RequestDataMartDTO>> MapExpression
        {
            get
            {
                return (dm) => new RequestDataMartDTO
                {
                    DataMart = dm.DataMart.Name,
                    DataMartID = dm.DataMartID,
                    ErrorDetail = dm.ErrorDetail,
                    ErrorMessage = dm.ErrorMessage,
                    Properties  = dm.Properties,
                    Status = dm.Status,
                    Priority = dm.Priority,
                    DueDate = dm.DueDate,
                    RejectReason = dm.RejectReason,
                    RequestID = dm.RequestID,
                    RequestTime = dm.RequestTime,
                    ResponseTime = dm.ResponseTime,
                    ResultsGrouped = dm.ResultsGrouped,
                    ID = dm.ID,
                    Timestamp = dm.Timestamp,
                    RoutingType = dm.RoutingType,
                    ResponseID = dm.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).Select(r => r.ID).FirstOrDefault(),
                    ResponseGroupID = dm.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).Select(r => r.ResponseGroupID).FirstOrDefault(),
                    ResponseGroup = dm.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).Select(r => r.ResponseGroup.Name).FirstOrDefault(),
                    ResponseMessage = dm.Responses.Where(r => r.Count == r.RequestDataMart.Responses.Max(x => x.Count)).Select(r => r.ResponseMessage).FirstOrDefault()
                };
            }
        }
    }

    public class HomepageRouteDetailDTOMappingConfiguration : EntityMappingConfiguration<RequestDataMart, HomepageRouteDetailDTO> 
    {
        public override System.Linq.Expressions.Expression<Func<RequestDataMart, HomepageRouteDetailDTO>> MapExpression
        {
            get
            {
                return (rdm) => new HomepageRouteDetailDTO
                {
                    RequestDataMartID = rdm.ID,
                    DataMartID = rdm.DataMartID,
                    RequestID = rdm.Request.ID,
                    DueDate = rdm.DueDate,
                    Identifier = rdm.Request.Identifier,
                    MSRequestID = rdm.Request.MSRequestID,
                    Name = rdm.Request.Name,
                    Priority = rdm.Priority,
                    Project = rdm.Request.Project.Name,
                    RequestType = rdm.Request.RequestType.Name,
                    SubmittedByName = rdm.Request.SubmittedBy.UserName,
                    SubmittedOn = rdm.Request.SubmittedOn,
                    IsWorkflowRequest = rdm.Request.WorkFlowActivityID.HasValue,
                    RequestStatus = rdm.Request.Status,
                    StatusText = rdm.Request.WorkFlowActivityID.HasValue ? rdm.Request.WorkflowActivity.Name :
                            rdm.Request.Status == RequestStatuses.AwaitingRequestApproval ? "Awaiting Request Approval" :
                            rdm.Request.Status == RequestStatuses.AwaitingResponseApproval ? "Awaiting Response Approval" :
                            rdm.Request.Status == RequestStatuses.Cancelled ? "Cancelled" :
                            rdm.Request.Status == RequestStatuses.Complete ? "Complete" :
                            rdm.Request.Status == RequestStatuses.Draft ? (rdm.Request.Scheduled ? "Scheduled" : "Draft") :
                            rdm.Request.Status == RequestStatuses.DraftReview ? "Draft Review" :
                            rdm.Request.Status == RequestStatuses.ExaminedByInvestigator ? "Examined By Investigator" :
                            rdm.Request.Status == RequestStatuses.Failed ? "Failed" :
                            rdm.Request.Status == RequestStatuses.Hold ? "Hold" :
                            rdm.Request.Status == RequestStatuses.PartiallyComplete ? rdm.Request.DataMarts.Where(d => d.Status == RoutingStatus.Completed || d.Status == RoutingStatus.ResultsModified || d.Status == RoutingStatus.ResponseRejectedAfterUpload || d.Status == RoutingStatus.ResponseRejectedBeforeUpload || d.Status == RoutingStatus.RequestRejected).Count().ToString() + " / " + rdm.Request.DataMarts.Where(d => d.Status != RoutingStatus.Canceled).Count().ToString() + " Responses Received" :
                            rdm.Request.Status == RequestStatuses.PendingUpload ? "Pending Upload" :
                            rdm.Request.Status == RequestStatuses.RequestRejected ? "Request Rejected" :
                            rdm.Request.Status == RequestStatuses.ResponseRejectedAfterUpload ? "Response Rejected After Upload" :
                            rdm.Request.Status == RequestStatuses.ResponseRejectedBeforeUpload ? "Response Rejected Before Upload" :
                            rdm.Request.Status == RequestStatuses.Resubmitted ? "Resubmitted" : rdm.Request.Status == RequestStatuses.Submitted ? "Submitted" :
                            rdm.Request.Status == RequestStatuses.ThirdPartySubmittedDraft ? "3rd Party Draft" : "Unknown",
                    RoutingStatus = rdm.Status,
                    RoutingStatusText = rdm.Status == RoutingStatus.Draft ? "Draft" :
                            rdm.Status == RoutingStatus.Submitted ? "Submitted" :
                            rdm.Status == RoutingStatus.Completed ? "Completed" :
                            rdm.Status == RoutingStatus.AwaitingRequestApproval ? "Awaiting Request Approval" :
                            rdm.Status == RoutingStatus.RequestRejected ? "Request Rejected" :
                            rdm.Status == RoutingStatus.Canceled ? "Canceled" :
                            rdm.Status == RoutingStatus.Resubmitted ? "Re-submitted" :
                            rdm.Status == RoutingStatus.PendingUpload ? "Pending Upload" :
                            rdm.Status == RoutingStatus.AwaitingResponseApproval ? "Awaiting Response Approval" :
                            rdm.Status == RoutingStatus.Hold ? "Hold" :
                            rdm.Status == RoutingStatus.ResponseRejectedBeforeUpload ? "Response Rejected Before Upload" :
                            rdm.Status == RoutingStatus.ResponseRejectedAfterUpload ? "Response Rejected After Upload" :
                            rdm.Status == RoutingStatus.ExaminedByInvestigator ? "Examined By Investigator" :
                            rdm.Status == RoutingStatus.ResultsModified ? "Results Modified" :
                            rdm.Status == RoutingStatus.Failed ? "Failed" : "Unknown"
                };
            }
        }
    }

    internal class RequestDataMartLogConfiguration : EntityLoggingConfiguration<DataContext, RequestDataMart>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, Utilities.Security.ApiIdentity identity, bool read)
        {
            if (obj.State == EntityState.Deleted)
            {
                return new AuditLog[] { };
            }

            var rdm = obj.Entity as RequestDataMart;
            if (rdm == null)
                throw new InvalidCastException("The entity passed is not a Request Data Mart");

            var logs = new List<AuditLog>();

            if (obj.State == EntityState.Added)
            {
                return CreateAddedLogItems(rdm, db, identity);
            }

            db.Entry(rdm).Reference(r => r.Request).Load();
            var details = db.RequestDataMarts.Where(dm => dm.ID == rdm.ID).Select(r => new { DataMartName = r.DataMart.Name, RequestName = r.Request.Name, RequestTypeName = r.Request.RequestType.Name }).Single();
            var currentTaskID = PmnTask.GetActiveTaskIDForRequestActivity(rdm.RequestID, rdm.Request.WorkFlowActivityID, db);

            var request = db.Requests.Find(rdm.RequestID);

            if (obj.State == EntityState.Modified)
            {
                LogRequestDataMartMetadataChanges(obj, db, identity, logs, request, currentTaskID, rdm);
            }

            if (((RoutingStatus)obj.CurrentValues["Status"] == RoutingStatus.Canceled) && ((int)request.Status >= (int)RequestStatuses.Submitted))
            {
                var logItem = new Audit.RequestDataMartAddedRemovedLog
                {
                    Description = string.Format("The DataMart {0} has been removed from Request {1}. ", details.DataMartName, details.RequestName),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID,
                    Reason = EntityState.Deleted
                };

                db.LogsRequestDataMartAddedRemoved.Add(logItem);
            }
            if (obj.State == EntityState.Modified && ((RoutingStatus)obj.OriginalValues["Status"] == RoutingStatus.Canceled) && ((RoutingStatus)obj.CurrentValues["Status"] == RoutingStatus.Submitted))
            {
                //cancelled routing re-added
                var logItem = new Audit.RequestDataMartAddedRemovedLog
                {
                    Description = string.Format("The DataMart {0} has been added to Request {1}. ", details.DataMartName, details.RequestName),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID,
                    Reason = EntityState.Added
                };

                db.LogsRequestDataMartAddedRemoved.Add(logItem);
            }

            if ((RoutingStatus)obj.OriginalValues["Status"] != (RoutingStatus)obj.CurrentValues["Status"] && RoutingStatus.AwaitingResponseApproval == (RoutingStatus)obj.CurrentValues["Status"])
            {
                var logItem = new Audit.UploadedResultNeedsApprovalLog
                {
                    Description = string.Format("Results uploaded by {0} for request {1} are awaiting approval. ", details.DataMartName, details.RequestName),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID
                };

                db.LogsUploadedResultNeedsApproval.Add(logItem);
                logs.Add(logItem);
            }


            RoutingStatus originalStatus = (RoutingStatus)obj.OriginalValues["Status"];
            RoutingStatus currentStatus = (RoutingStatus)obj.CurrentValues["Status"];

            if (((originalStatus != currentStatus && originalStatus != RoutingStatus.Draft) || 
                (originalStatus == RoutingStatus.ResultsModified && currentStatus == RoutingStatus.ResultsModified))
                && originalStatus != RoutingStatus.AwaitingRequestApproval)
            {
                //Routing Status Changed
                var orgUser = identity == null ? new { UserName = "", Acronym = "" } : db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();
                Guid? responseID = db.Responses.Where(rsp => rsp.RequestDataMartID == rdm.ID).OrderByDescending(rsp => rsp.Count).Select(rsp => (Guid?)rsp.ID).FirstOrDefault();

                var logRoutingStatusChanged = new Audit.RoutingStatusChangeLog
                {
                    Description = "Routing status of " + details.DataMartName + " for request " + details.RequestName + " has been changed from " + originalStatus.ToString(true) + " to " + currentStatus.ToString(true) + " by " + orgUser.Acronym + @"\" + orgUser.UserName + ".",
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID,
                    OldStatus = originalStatus,
                    NewStatus = currentStatus,
                    ResponseID = responseID
                };

                db.LogsRoutingStatusChange.Add(logRoutingStatusChanged);
                logs.Add(logRoutingStatusChanged);
            }

            if(originalStatus != currentStatus && currentStatus == RoutingStatus.Resubmitted)
            {

                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                var dmNumber = 0;
                foreach (var dm in rdm.Request.DataMarts.OrderBy(d => d.ID))
                {
                    if (dm.ID == rdm.ID)
                        break;
                    dmNumber++;
                }

                //this is where the new request submitted log item should get created on a resubmit, not in the request
                var newRequestResubmittedLogItem = new Audit.NewRequestSubmittedLog {
                    Description = string.Format("New request of type '{0}' has been submitted by {1}", details.RequestTypeName, (orgUser.Acronym + @"\" + orgUser.UserName)),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestID = request.ID,
                    RequestDataMartID = rdm.ID,
                    TaskID = currentTaskID
                };

                newRequestResubmittedLogItem.TimeStamp = DateTimeOffset.Now.AddMilliseconds(dmNumber);

                db.LogsNewRequestSubmitted.Add(newRequestResubmittedLogItem);
                logs.Add(newRequestResubmittedLogItem);
            }

            return logs.AsEnumerable();

        }

        void LogRequestDataMartMetadataChanges(DbEntityEntry obj, DataContext db, ApiIdentity identity, List<AuditLog> logs, Request request, Guid? taskID, RequestDataMart rdm)
        {
            if (!request.Private)
            {
                var changedProperties = GetMetadataFieldsWithChanges(obj).ToList();
                if (changedProperties.Count == 0)
                    return;

                if (!db.Entry(request).Reference(r => r.Project).IsLoaded)
                    db.Entry(request).Reference(r => r.Project).Load();

                if (!db.Entry(request).Reference(r => r.RequestType).IsLoaded)
                    db.Entry(request).Reference(r => r.RequestType).Load();

                PropertyChangeDetailDTO changeDetail = changedProperties.FirstOrDefault(f => f.Property == "DueDate");
                if (changeDetail != null)
                {
                    if (changeDetail.NewValue != null)
                    {
                        changeDetail.NewValueDisplay = ((DateTime)changeDetail.NewValue).ToString("d");
                    }
                    if (changeDetail.OriginalValue != null)
                    {
                        changeDetail.OriginalValueDisplay = ((DateTime)changeDetail.OriginalValue).ToString("d");
                    }
                }

                StringBuilder description = new StringBuilder();
                description.AppendFormat("<p>The following metadata fields of your <b>{0}</b> request <b>{1}</b> ", request.RequestType.Name, request.Name);
                if (!string.IsNullOrWhiteSpace(request.MSRequestID))
                {
                    description.AppendFormat("(ID <b>{0}</b>) ", request.MSRequestID);
                }
                description.AppendFormat("in the <b>{0}</b> project were modified.</p>", request.Project.Name);

                description.Append("<table class=\"table table-condensed notification-table\"><thead><tr class=\"notification-tableheader\"><th><b>Request Field</b></th><th><b>Old Value</b></th><th><b>New Value</b></th></tr></thead><tbody>");
                foreach (var propChange in changedProperties)
                {
                    description.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", propChange.PropertyDisplayName, propChange.OriginalValueDisplay, propChange.NewValueDisplay);
                }
                description.Append("</tbody></table>");


                var log = new Audit.RequestDataMartMetadataChangeLog
                {
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    Request = request,
                    RequestID = request.ID,
                    TaskID = taskID,
                    Description = description.ToString(),
                    RequestDataMartID = rdm.ID
                };

                var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.None,
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });

                log.ChangeDetail = Newtonsoft.Json.JsonConvert.SerializeObject(changedProperties, serializerSettings);

                db.LogsRequestDataMartMetadataChange.Add(log);
                logs.Add(log);
            }
        }

        /// <summary>
        /// Property names that are monitored for request dataMart metadata changes.
        /// </summary>
        //readonly string[] MetadataPropertyNames = new[] { "Priority", "DueDate" };
        readonly Dictionary<string, string> MetadataPropertyNames = new Dictionary<string, string> {
            {"Priority", "Priority"},
            {"DueDate", "Due Date"}
        };

        /// <summary>
        /// Gets a collection of request dataMart metadata property names that have changed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<PropertyChangeDetailDTO> GetMetadataFieldsWithChanges(DbEntityEntry obj)
        {
            return MetadataPropertyNames.Select(f => new PropertyChangeDetailDTO { Property = f.Key, PropertyDisplayName = f.Value, OriginalValue = obj.OriginalValues[f.Key], OriginalValueDisplay = obj.OriginalValues[f.Key].ToStringEx(), NewValue = obj.CurrentValues[f.Key], NewValueDisplay = obj.CurrentValues[f.Key].ToStringEx() }).Where(f => !DbPropertyValuesEqual(f.Property, obj.OriginalValues, obj.CurrentValues));
        }

        static bool DbPropertyValuesEqual(string key, DbPropertyValues originalValues, DbPropertyValues currentValues)
        {
            if (originalValues[key] == null && currentValues[key] == null)
                return true;

            if ((originalValues[key] == null && currentValues[key] != null) || (originalValues[key] != null && currentValues[key] == null))
            {
                return false;
            }

            return originalValues[key].Equals(currentValues[key]);
        }

        IEnumerable<AuditLog> CreateAddedLogItems(RequestDataMart rdm, DataContext db, Utilities.Security.ApiIdentity identity)
        {
            var requestChangeTracker = db.ChangeTracker.Entries<Request>().FirstOrDefault(r => r.Entity.ID == rdm.RequestID);
            var request = db.Requests.Find(rdm.RequestID);
            var currentTaskID = PmnTask.GetActiveTaskIDForRequestActivity(rdm.RequestID, request.WorkFlowActivityID, db);

            if ((int)request.Status >= (int)RequestStatuses.Submitted)
            {

                db.Entry(rdm).Reference(r => r.Request).Load();
                var details = db.DataMarts.Where(dm => dm.ID == rdm.DataMartID).FirstOrDefault();

                var logItem = new Audit.RequestDataMartAddedRemovedLog
                {
                    Description = string.Format("The DataMart {0} has been added to Request {1}. ", details.Name, rdm.Request.Name),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID,
                    Reason = EntityState.Added
                };

                db.LogsRequestDataMartAddedRemoved.Add(logItem);

            }

            if ((int)request.Status >= (int)RequestStatuses.Submitted && (requestChangeTracker == null || (int)(requestChangeTracker.OriginalValues["Status"] ?? 0) >= (int)RequestStatuses.Submitted))
            {
                //if the request status is at least submitted, and is either not part of this transaction or had already had a status of submitted 
                //or greater log a submitted event for the request datamart

                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                if (request.RequestType == null)
                    db.Entry(request).Reference(r => r.RequestType).Load();

                var dmNumber = 0;
                foreach (var dm in rdm.Request.DataMarts.OrderBy(d => d.ID))
                {
                    if (dm.ID == rdm.ID)
                        break;
                    dmNumber++;
                }

                //New Request Submitted
                var newRequestSubmittedLogItem = new Audit.NewRequestSubmittedLog
                {
                    Description = string.Format("New request of type '{0}' has been submitted by {1}.", request.RequestType.Name, (orgUser.Acronym + @"\" + orgUser.UserName)),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestID = request.ID,
                    Request = request,
                    RequestDataMartID = rdm.ID,
                    RequestDataMart = rdm,
                    TaskID = currentTaskID
                };

                newRequestSubmittedLogItem.TimeStamp = DateTimeOffset.Now.AddMilliseconds(dmNumber);

                db.LogsNewRequestSubmitted.Add(newRequestSubmittedLogItem);

                return new[] { newRequestSubmittedLogItem };
            }

            return Enumerable.Empty<AuditLog>();
        }

        string TranslatePurposeOfUse(string value)
        {
            switch (value)
            {
                case "CLINTRCH":
                    return "Clinical Trial Research";
                case "HMARKT":
                    return "Healthcare Marketing";
                case "HOPERAT":
                    return "Healthcare Operations";
                case "HPAYMT":
                    return "Healthcare Payment";
                case "HRESCH":
                    return "Healthcare Research";
                case "OBSRCH":
                    return "Observational Research";
                case "PATRQT":
                    return "Patient Requested";
                case "PTR":
                    return "Prep-to-Research";
                case "PUBHLTH":
                    return "Public Health";
                case "QA":
                    return "Quality Assurance";
                case "TREAT":
                    return "Treatment";
            }
            return value;
        }

        public override IEnumerable<Notification> CreateNotifications<T>(T logItem, DataContext db, bool immediate)
        {
            var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();

            if (typeof(T) == typeof(Audit.UploadedResultNeedsApprovalLog))
            {
                var log = logItem as Audit.UploadedResultNeedsApprovalLog;
                log.RequestDataMart = db.RequestDataMarts.Where(r => r.ID == log.RequestDataMartID).Include("Request.RequestType").Include("Request.Project").FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                if (log.RequestDataMart.Request == null)
                    log.RequestDataMart.Request = db.Requests.Find(log.RequestDataMart.RequestID);

                if (log.RequestDataMart.DataMart == null)
                    log.RequestDataMart.DataMart = db.DataMarts.Find(log.RequestDataMart.DataMartID);

                if (log.RequestDataMart.Request.Project == null)
                    log.RequestDataMart.Request.Project = db.Projects.Find(log.RequestDataMart.Request.ProjectID);

                var body = GenerateTimestampText(log) +
                           "<p>Here are your most recent <b>Uploaded Results Need Approval</b> notifications from " + networkName + ".</p>" +
                           "<p>The " + log.RequestDataMart.DataMart.Name + " routing status for " + log.RequestDataMart.Request.RequestType.Name +
                           " request " + log.RequestDataMart.Request.Name + " in project " + log.RequestDataMart.Request.Project.Name +
                           " are awaiting approval.</p>";
                body += "<h3>Message</h3>" + log.Description;

                if (log.RequestDataMart.Responses == null || !log.RequestDataMart.Responses.Any())
                    log.RequestDataMart.Responses = db.Responses.Where(r => r.RequestDataMartID == log.RequestDataMartID).ToList();

                var response = log.RequestDataMart.Responses.OrderByDescending(r => r.Count).First();

                var notification = new Notification
                {
                    Subject = "Uploaded Results Needs Approval Notification",
                    Body = body,
                    Recipients = (from s in db.ReturnUserEventSubscriptionsByResponse(response.ID, EventIdentifiers.Response.UploadedResultNeedsApproval.ID, immediate)
                                  join us in db.Users on s.UserID equals us.ID
                                  where
                                       (
                                           (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID && a.OrganizationID == log.RequestDataMart.DataMart.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                         || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID && a.ProjectID == log.RequestDataMart.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                         || db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID
                                             && a.ProjectID == log.RequestDataMart.Request.ProjectID && a.DataMartID == log.RequestDataMart.DataMartID
                                             && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                           )
                                       &&
                                           (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID && a.OrganizationID == log.RequestDataMart.DataMart.OrganizationID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                         && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID && a.ProjectID == log.RequestDataMart.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                         && db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Response.UploadedResultNeedsApproval.ID
                                             && a.ProjectID == log.RequestDataMart.Request.ProjectID && a.DataMartID == log.RequestDataMart.DataMartID
                                             && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                           )
                                       )
                                       && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                                  select new Recipient
                                  {
                                      Email = us.Email,
                                      Phone = us.Phone,
                                      Name = ((us.FirstName + " " + us.MiddleName).Trim() + " " + us.LastName).Trim(),
                                      UserID = s.UserID
                                  }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.RoutingStatusChangeLog))
            {
                var log = logItem as Audit.RoutingStatusChangeLog;

                string oldStatus = log.OldStatus.HasValue ? log.OldStatus.Value.ToString(true) : string.Empty;
                string newStatus = log.NewStatus.HasValue ? log.NewStatus.Value.ToString(true) : string.Empty;

                if (string.IsNullOrEmpty(oldStatus) || string.IsNullOrEmpty(newStatus))
                {
                    string pattern = @"from ([\w| ]+) to ([\w| ]+) by";
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(log.Description);

                    if (string.IsNullOrEmpty(oldStatus))
                    {
                        oldStatus = match.Groups[1].ToString();
                    }
                    if (string.IsNullOrEmpty(newStatus))
                    {
                        newStatus = match.Groups[2].ToString();
                    }
                }

                var details = (from rdm in db.RequestDataMarts
                               let lastResponse = rdm.Responses.OrderByDescending(rsp => rsp.Count).FirstOrDefault()
                              where rdm.ID == log.RequestDataMartID
                              select new {
                                  RequestID = rdm.RequestID,
                                  DataMartID = rdm.DataMartID,
                                  DataMartName = rdm.DataMart.Name,
                                  DataMartOrganizationID = rdm.DataMart.OrganizationID,
                                  RequestTypeName = rdm.Request.RequestType.Name,
                                  RequestName = rdm.Request.Name,
                                  ProjectID = rdm.Request.ProjectID,
                                  ProjectName = rdm.Request.Project.Name,
                                  RoutingStatus = rdm.Status,
                                  LastResponseMessage = lastResponse.ResponseMessage,
                                  SubmitMessage = lastResponse.SubmitMessage
                              }).FirstOrDefault();

                var actingUser = db.Users.Find(log.UserID);

                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>Request DataMart Routing Status Changed</b> notifications from <b>" + networkName + "</b>.</p>" +
                           "<p>The <b>" + details.DataMartName + "</b> routing status for <b>" + details.RequestTypeName +
                           "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName +
                           "</b> was changed from <b>" + oldStatus + "</b> to <b>" + newStatus + "</b> by <b>" + actingUser.FullName + "</b>.</p>";
                body += "<h3>Message</h3><pre>" + ((details.RoutingStatus == RoutingStatus.Resubmitted || details.RoutingStatus == RoutingStatus.Submitted) ? details.SubmitMessage : details.LastResponseMessage) + "</pre>";

                var myBody = GenerateTimestampText(log) +
                          "<p>Here are your most recent <b>My Request DataMart Routing Status Changed</b> notifications from <b>" + networkName + "</b>.</p>" +
                          "<p>The <b>" + details.DataMartName + "</b> routing status for <b>" + details.RequestTypeName +
                          "</b> request <b>" + details.RequestName + "</b> in project <b>" + details.ProjectName +
                          "</b> was changed from <b>" + oldStatus + "</b> to <b>" + newStatus + "</b> by <b>" + actingUser.FullName + "</b>.</p>";
                myBody += "<h3>Message</h3><pre>" + ((details.RoutingStatus == RoutingStatus.Resubmitted || details.RoutingStatus == RoutingStatus.Submitted) ? details.SubmitMessage : details.LastResponseMessage) + "</pre>";

              //user is not a requestUser and has subscribed to the general notification
              var recipients = (from s in db.UserEventSubscriptions
                                  join u in db.ResponseRelatedNotificationRecipients(EventIdentifiers.Request.RoutingStatusChanged.ID, log.RequestDataMartID)
                                  on s.UserID equals u.ID
                                  where (s.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                                  (
                                        (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID && a.OrganizationID == details.DataMartOrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                        || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                        || db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID
                                            && a.ProjectID == details.ProjectID && a.DataMartID == details.DataMartID
                                            && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).Any()
                                        )
                                    &&
                                        (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID && a.OrganizationID == details.DataMartOrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                        && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID && a.ProjectID == details.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                        && db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID
                                            && a.ProjectID == details.ProjectID && a.DataMartID == details.DataMartID
                                            && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                        )
                                    )
                                  && (!immediate || s.Frequency == Frequencies.Immediately)
                                  //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                                  && (!db.RequestUsers.Any(ru => ru.RequestID == details.RequestID && ru.UserID == s.UserID)
                                      ||
                                     (db.RequestUsers.Any(ru => ru.RequestID == details.RequestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null))
                                  ))
                                  select new Recipient
                                  {
                                      Email = u.Email,
                                      Phone = u.Phone,
                                      Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                      UserID = s.UserID
                                  }).ToArray();

                //user is a request user and has subscribed to the "My" notification
                var requestUsers = from s in db.UserEventSubscriptions
                                   where (s.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID) && !s.User.Deleted && s.User.Active && s.FrequencyForMy != null
                                         && (db.Requests.Any(r => r.ID == details.RequestID))
                                         && db.RequestUsers.Any(ru => ru.RequestID == details.RequestID && ru.UserID == s.UserID)
                                         && (!immediate || s.FrequencyForMy == Frequencies.Immediately)
                                   select s;

                var userObservers = (from u in db.Users
                                     let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == details.RequestID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                     where requestObservers.Any()
                                     select new Recipient
                                     {
                                         Email = u.Email,
                                         Phone = u.Phone,
                                         Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                         UserID = u.ID
                                     }).ToArray();

                var emailObservers = (from o in db.RequestObservers
                                      where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RoutingStatusChanged.ID)
                                      && o.UserID.HasValue == false && o.SecurityGroupID.HasValue == false
                                      && o.Email != "" && o.RequestID == details.RequestID
                                      select new Recipient
                                      {
                                          Email = o.Email,
                                          Phone = "",
                                          Name = o.DisplayName,
                                          UserID = null
                                      }).ToArray();

                recipients = recipients.Union(userObservers).Union(emailObservers).ToArray();

                var notification = new Notification
                {
                    Subject = "Request DataMart Routing Status Changed Notification",
                    Body = body,
                    Recipients = recipients
                };

                var myNotification = new Notification
                {
                    Subject = "My Request DataMart Routing Status Changed Notification",
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
            // Requesters need to be informed separately of DM routing change specifically pertaining to their request only.
            // Handled through Request Status Changed as in v4.
            else if (typeof(T) == typeof(Audit.RequestStatusChangedLog))
            {
                var log = logItem as Audit.RequestStatusChangedLog;

                //user is not a requestUser and has subscribed to the general notification
                var recipients = // Subscriber with project level permission to RequestStatusChanged for the Request due to routing changed.
                          (from s in db.UserEventSubscriptions
                          join u in db.Users on s.UserID equals u.ID
                          where (s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                           (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                        || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                        || db.ProjectOrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                           )
                      &&
                           (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                        && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                        && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID
                                     && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID
                                     && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                           )
                         && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                        //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                        && (!db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                            ||
                            db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                            )
                        )
                           select new Recipient
                          {
                              Email = s.User.Email,
                              Phone = s.User.Phone,
                              Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                              UserID = s.UserID
                          }).ToArray();

                //Requester should always get RequestStatusChanged notification due to routing change regardless of permissions, but will only get general notification if not subscribed to "My" notification
                var requester = (from s in db.UserEventSubscriptions
                                join u in db.Users on s.UserID equals u.ID
                                where u.ID == log.Request.SubmittedByID && !u.Deleted && s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && s.FrequencyForMy == null
                                   && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                select new Recipient
                                {
                                    Email = u.Email,
                                    Phone = u.Phone,
                                    Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                    UserID = u.ID
                                }
                      ).ToArray();

                recipients = recipients.Union(requester).ToArray();

                var requestUsers = (from s in db.UserEventSubscriptions
                                    join u in db.Users on s.UserID equals u.ID
                                    where (s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                                     (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                  || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                  || db.ProjectOrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                     )
                                &&
                                     (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                  && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                  && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID
                                               && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID
                                               && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                     )
                                   && ((!immediate && (s.NextDueTimeForMy == null || s.NextDueTimeForMy <= DateTime.UtcNow)) || (Frequencies)s.FrequencyForMy == Frequencies.Immediately)
                                  //user is a request user 
                                  && db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID))
                                    select new Recipient
                                    {
                                        Email = s.User.Email,
                                        Phone = s.User.Phone,
                                        Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                        UserID = s.UserID
                                    }).ToArray();


                IList<Notification> notifies = new List<Notification>();

                if (recipients.Any() || requestUsers.Any())
                {
                    var requestLogConfiguration = new RequestLogConfiguration();
                    string[] emailText = AsyncHelpers.RunSync<string[]>(() => requestLogConfiguration.GenerateRequestStatusChangedEmailContent(db, log.RequestID, log.UserID, log.OldStatus, log.NewStatus));
                    string body = emailText[0];
                    string myBody = emailText[1];

                    if (recipients.Any())
                    {
                        var notification = new Notification
                        {
                            Subject = "Request Status Changed Notification",
                            Body = body,
                            Recipients = recipients
                        };

                        notifies.Add(notification);
                    }

                    if (requestUsers.Any())
                    {
                        var myNotification = new Notification
                        {
                            Subject = "Your Request Status Changed Notification",
                            Body = myBody,
                            Recipients = requestUsers
                        };

                        notifies.Add(myNotification);
                    }

                }               

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.NewRequestSubmittedLog))
            {
                var log = logItem as Audit.NewRequestSubmittedLog;
                if (log.RequestDataMartID.HasValue)
                {

                    var requestDataMart = db.RequestDataMarts.Find(log.RequestDataMartID.Value);
                    var datamart = db.DataMarts.Find(requestDataMart.DataMartID);

                    var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();
                    log.Request = db.Requests.Where(r => r.ID == log.RequestID).Include("RequestType").Include("Activity.ParentActivity.ParentActivity")
                        .Include("RequesterCenter").Include("WorkplanType").Include("Project").Include("SubmittedBy").Include("ReportAggregationLevel").FirstOrDefault();

                    var body = GenerateTimestampText(log) +
                               "<p>Here are your most recent <b>New Request Submitted</b> notifications from " + networkName + ".</p>" +
                               "<p>" + actingUser.FullName + " has submitted a new <b>" + log.Request.RequestType.Name + "</b> request " + log.Request.Name +
                               " in project <b>" + log.Request.Project.Name + "</b> to DataMart <b>" + requestDataMart.DataMart.Name +"</b>.";

                    //get source activities text for body of email
                    var activities = db.Activities.Where(a => a.ProjectID == log.Request.ProjectID && a.Deleted == false).ToArray();
                    var sourceActivity = activities.FirstOrDefault(a => a.ID == log.Request.SourceActivityID);
                    var sourceActivityProject = activities.FirstOrDefault(a => a.ID == log.Request.SourceActivityProjectID);
                    var sourceTaskOrder = activities.FirstOrDefault(a => a.ID == log.Request.SourceTaskOrderID);

                    body += "<h3>Description</h3><p>" + log.Request.Description + "</p>";
                    body += "<h3>Request Details</h3>" +
                            "Request type: <b>" + log.Request.RequestType.Name + "</b><br/>" +
                            "Request Name: <b>" + log.Request.Name + "</b><br/>" +
                            "Request ID: <b>" + (log.Request.MSRequestID.IsNullOrEmpty() ? "" : log.Request.MSRequestID) + "</b><br/>" +
                            "Project: <b>" + log.Request.Project.Name + "</b><br/>" +
                            "Budget Item: <b>" + (log.Request.ActivityID != null && log.Request.Activity.ParentActivityID != null && log.Request.Activity.ParentActivity.ParentActivity != null ? log.Request.Activity.ParentActivity.ParentActivity.Name : "") +
                                            (log.Request.ActivityID != null && log.Request.Activity.ParentActivityID != null ? "; " + log.Request.Activity.ParentActivity.Name : "") +
                                            (log.Request.ActivityID != null ? "; " + log.Request.Activity.Name : "") + "</b><br/>" +
                            "Source Item: <b>" + (sourceTaskOrder != null ? sourceTaskOrder.Name : "") +
                                            (sourceActivity != null ? "; " + sourceActivity.Name : "") +
                                            (sourceActivityProject != null ? "; " + sourceActivityProject.Name : "") + "</b><br/>" +
                            "Request Due Date: <b>" + (log.Request.DueDate != null ? log.Request.DueDate.Value.ToShortDateString() : "") + "</b><br/>" +
                            "Contact Person: <b>" + log.Request.SubmittedBy.FullName + " (" + log.Request.SubmittedBy.Email + ")" + "</b><br/>" +
                            "Priority: <b>" + log.Request.Priority + "</b><br/>" +
                            "Purpose of Use: <b>" + (TranslatePurposeOfUse(log.Request.PurposeOfUse) ?? "") + "</b><br/>" +
                            "Level of PHI Disclosure: <b>" + (log.Request.PhiDisclosureLevel ?? "") + "</b><br/>" +
                            "Level of Report Aggregation: <b>" + (log.Request.ReportAggregationLevelID != null && log.Request.ReportAggregationLevelID != Guid.Empty ? log.Request.ReportAggregationLevel.Name : "") + "</b><br/>" +
                            "Requester Center: <b>" + (log.Request.RequesterCenterID != null && log.Request.RequesterCenterID != Guid.Empty ? log.Request.RequesterCenter.Name : "") + "</b><br/>" +
                            "Workplan Type: <b>" + (log.Request.WorkplanTypeID != null && log.Request.WorkplanTypeID != Guid.Empty ? log.Request.WorkplanType.Name : "") + "</b><br/>" +
                            "<h3>Additional Instructions</h3><br/><pre>" + log.Request.AdditionalInstructions + "</pre><br/>";

                    body += "<h3>DataMarts</h3><p>" + string.Join(",<br/>", db.RequestDataMarts.Where(dm => dm.RequestID == log.RequestID).Select(dm => dm.DataMart.Name).ToArray()) + "</p>";


                    /**
                     * Limit the recipients to the normal list of request submitted users, 
                     * and then limit that by the users that have SeeRequest permissions for the specific DataMart
                     * */                    

                    var recipients = from s in db.UserEventSubscriptions
                                     join u in db.RequestRelatedNotificationRecipients(EventIdentifiers.Request.NewRequestSubmitted.ID, log.RequestID) on s.UserID equals u.ID
                                     where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                     && (
                                        from uu in db.Users
                                        let dmAcls = db.DataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.SecurityGroup.Users.Any(su => su.UserID == uu.ID) && a.DataMartID == requestDataMart.DataMartID)
                                        let dmProjects = db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.SecurityGroup.Users.Any(su => su.UserID == uu.ID) && a.Project.Requests.Any(r => r.ID == requestDataMart.RequestID))
                                        let dmOrganizations = db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.SecurityGroup.Users.Any(su => su.UserID == uu.ID) && a.Organization.DataMarts.Any(dm => dm.ID == requestDataMart.DataMartID))
                                        let dmProjectDataMarts = db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.SecurityGroup.Users.Any(su => su.UserID == uu.ID) && a.Project.Requests.Any(r => r.ID == requestDataMart.RequestID) && a.DataMartID == requestDataMart.DataMartID)
                                        let dmGlobal = db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.SeeRequests.ID && a.SecurityGroup.Users.Any(su => su.UserID == uu.ID))
                                        where uu.ID == s.UserID
                                        && (
                                            (dmAcls.Any(a => a.Allowed) || dmProjects.Any(a => a.Allowed) || dmOrganizations.Any(a => a.Allowed) || dmProjectDataMarts.Any(a => a.Allowed) || dmGlobal.Any(a => a.Allowed))
                                            &&
                                            (dmAcls.All(a => a.Allowed) && dmProjects.All(a => a.Allowed) && dmOrganizations.All(a => a.Allowed) && dmProjectDataMarts.All(a => a.Allowed) && dmGlobal.All(a => a.Allowed))
                                           )
                                        select uu
                                     ).Any()
                                     select u;


                    var notification = new Notification
                    {
                        Subject = "New Request Submitted Notification",
                        Body = body,
                        Recipients = (from u in recipients
                                      select new Recipient
                                      {
                                          Email = u.Email,
                                          Phone = u.Phone,
                                          Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                          UserID = u.ID
                                      }).ToArray()
                    };
                    IList<Notification> notifies = new List<Notification>();
                    notifies.Add(notification);

                    return notifies.AsEnumerable();
                }
            }
            else if (typeof(T) == typeof(Audit.RequestDataMartMetadataChangeLog))
            {
                var log = logItem as Audit.RequestDataMartMetadataChangeLog;
                var network = db.Networks.Select(n => n.Name).FirstOrDefault();
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();
                log.Request = db.Requests.Where(r => r.ID == log.RequestID).Include("RequestType").Include("Project").FirstOrDefault();
                log.RequestDataMart.DataMart = db.DataMarts.Where(d => d.ID == log.RequestDataMart.DataMartID).FirstOrDefault();

                StringBuilder body = new StringBuilder(GenerateTimestampText(log));
                body.AppendFormat("<p>Here are your most recent <b>Datamart Request Metadata Changed</b> notifications from <b>{0}</b>.</p>", network);
                body.AppendFormat("<p>The following metadata fields of your <b>{0}</b> request <b>{1}</b> ", log.Request.RequestType.Name, log.Request.Name);
                if (!string.IsNullOrWhiteSpace(log.Request.MSRequestID))
                {
                    body.AppendFormat("(ID <b>{0}</b>) ", log.Request.MSRequestID);
                }
                body.AppendFormat("for the <b>{0}</b> ", log.RequestDataMart.DataMart.Name);
                body.AppendFormat("in the <b>{0}</b> project were modified.</p>", log.Request.Project.Name);

                var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.None,
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });

                IEnumerable<PropertyChangeDetailDTO> changedProperties = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PropertyChangeDetailDTO>>(log.ChangeDetail, serializerSettings);


                body.Append("<table cellpadding=\"3\" cellspacing=\"0\" style=\"width:90%;text-align:left;\"><colgroup><col style=\"width:20%\" /><col style=\"width:40%\" /><col style=\"width:40%\" /></colgroup><thead><tr><th style=\"border-bottom:2px solid #ddd;\"><b>MetaData Field</b></th><th style=\"border-bottom:2px solid #ddd;\"><b>Old Value</b></th><th style=\"border-bottom:2px solid #ddd;\"><b>New Value</b></strong></th></tr></thead><tbody>");
                foreach (var propChange in changedProperties)
                {
                    body.AppendFormat("<tr><td style=\"border-top:1px solid #ddd;border-right:1px solid #ddd;\">{0}</td><td style=\"border-top:1px solid #ddd;border-right:1px solid #eee;\">{1}</td><td style=\"border-top:1px solid #ddd;\">{2}</td></tr>", propChange.PropertyDisplayName, string.IsNullOrWhiteSpace(propChange.OriginalValueDisplay) ? "&nbsp;" : propChange.OriginalValueDisplay, string.IsNullOrWhiteSpace(propChange.NewValueDisplay) ? "&nbsp;" : propChange.NewValueDisplay);
                }
                body.Append("</tbody></table>"); 

                var query = from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.DataMartMetadataChange.ID, immediate)
                            join u in db.Users on s.UserID equals u.ID
                            let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.DataMartMetadataChange.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID) && e.DataMartID == log.RequestDataMart.DataMartID)
                            where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                            && (
                                projectDataMartEventAcls.Any() && projectDataMartEventAcls.All(a => a.Allowed)
                            )
                            select
                                  new Recipient
                                  {
                                      Email = u.Email,
                                      Phone = u.Phone,
                                      Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                      UserID = s.UserID
                                  };
                
                var recipients = query.ToArray();

                var notification = new Notification
                {
                    Subject = "Your DataMart Request Metadata Changed Notification",
                    Body = body.ToString(),
                    Recipients = recipients
                };

                return new List<Notification> { notification };

            }

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Request DataMart Logging Configuration");

        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {
            var logs = await FilterAuditLog(from l in db.LogsUploadedResultNeedsApproval.Include(x => x.RequestDataMart.Request) select l, db.UserEventSubscriptions, EventIdentifiers.Response.UploadedResultNeedsApproval.ID).GroupBy(g => new { g.RequestDataMartID, g.UserID }).ToArrayAsync();

            var notifications = new List<Notification>();
            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            var logsStatusChange = await FilterAuditLog(from l in db.LogsRoutingStatusChange.Include(x => x.RequestDataMart.Request).Include(x => x.RequestDataMart.DataMart) select l, db.UserEventSubscriptions, EventIdentifiers.Response.UploadedResultNeedsApproval.ID).GroupBy(g => new { g.RequestDataMartID, g.UserID }).ToArrayAsync();

            foreach (var log in logsStatusChange)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            var logsDataMartAddedToRequest = await FilterAuditLog(from l in db.LogsNewRequestSubmitted.Where(x => x.RequestDataMartID.HasValue).Include(x => x.RequestDataMart.Request).Include(x => x.RequestDataMart.DataMart) select l, db.UserEventSubscriptions, EventIdentifiers.Request.NewRequestSubmitted.ID).GroupBy(g => new { g.RequestDataMartID, g.UserID, g.RequestID }).ToArrayAsync();
            foreach (var log in logsDataMartAddedToRequest)
            {
                if (log.Key.RequestDataMartID == null || !log.Any())
                    continue;

                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications.AsEnumerable();
        }
    }
}
