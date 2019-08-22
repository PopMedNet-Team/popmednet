using System.Linq.Expressions;
using LinqKit;
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
using Lpp.Utilities;
using Lpp.Utilities.Objects;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Logging;
using Lpp.Dns.DTO.Events;
using Lpp.Workflow.Engine.Interfaces;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;

namespace Lpp.Dns.Data
{
    [Table("Requests")]
    public class Request : EntityWithID, IEntityWithName, IEntityWithDeleted, Lpp.Security.ISecurityObject, IWorkflowEntity
    {

        public Request() {
            this.CreatedOn = DateTime.UtcNow;
            this.UpdatedOn = DateTime.UtcNow;
            this.Private = true;
            this.Description = string.Empty;
            this.MirrorBudgetFields = false;
            //this.Statistics = new RequestStatistics() { RequestID = this }; <- This crashes queries if it's enabled.

            this.DataMarts = new HashSet<RequestDataMart>();
            this.RequestAcls = new HashSet<AclRequest>();
            this.Folders = new HashSet<RequestSharedFolderRequest>();
            this.Users = new HashSet<RequestUser>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long Identifier { get; set; }

        [MaxLength(255)]
        public string Name {get; set;}
        
        [MaxLength(255)]
        public string MSRequestID { get; set; }

        [MaxLength]
        public string Description { get; set; }

        public string AdditionalInstructions { get; set; }

        public bool MirrorBudgetFields { get; set; }
        
        [Column("isScheduled")]
        public bool Scheduled { get; set; }

        [Column("isTemplate")]
        public bool Template { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        public Priorities Priority { get; set; }

        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        [MaxLength(100)]
        public string PurposeOfUse { get; set; }

        public string PhiDisclosureLevel { get; set; }

        public string Schedule { get; set; }

        public int ScheduleCount { get; set; }

        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }        

        public Guid RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }

        [MaxLength(20)]
        public string AdapterPackageVersion { get; set; }

        [MaxLength(100)]
        public string IRBApprovalNo { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(255)]
        public string ActivityDescription { get; set; }

        public Guid? RequesterCenterID { get; set; }
        public virtual RequesterCenter RequesterCenter { get; set; }
        public Guid? WorkplanTypeID { get; set; }
        public virtual WorkplanType WorkplanType { get; set; }    

        public Guid? ReportAggregationLevelID { get; set; }
        public virtual ReportAggregationLevel ReportAggregationLevel { get; set; }

        public Guid? ActivityID { get; set; }
        public virtual Activity Activity { get; set; }

        public Guid? SourceActivityID { get; set; }
        public virtual Activity SourceActivity { get; set; }
        public Guid? SourceActivityProjectID { get; set; }
        public virtual Activity SourceActivityProject { get; set; }
        public Guid? SourceTaskOrderID { get; set; }
        public virtual Activity SourceTaskOrder { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedByID { get; set; }
        public virtual User CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedByID { get; set; }
        public virtual User UpdatedBy { get; set;}

        public DateTime? SubmittedOn { get; set; }
        public Guid? SubmittedByID { get; set; }
        public virtual User SubmittedBy { get; set; }

        public DateTime? ApprovedForDraftOn { get; set; }
        public Guid? ApprovedForDraftByID { get; set; }
        public virtual User ApprovedForDraftBy { get; set; }

        public DateTime? RejectedOn { get; set; }
        public Guid? RejectedByID { get; set; }
        public virtual User RejectedBy { get; set; }

        public DateTime? CancelledOn { get; set; }
        public Guid? CancelledByID { get; set; }
        public virtual User CancelledBy { get; set; }

        public DateTimeOffset? CompletedOn { get; set; }
        [MaxLength(100)]
        public string UserIdentifier { get; set; }
        public Guid? WorkflowID { get; set; }
        public virtual Workflow Workflow { get; set; }
        public Guid? WorkFlowActivityID { get; set; }
        public virtual WorkflowActivity WorkflowActivity { get; set; }

        public bool Private { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed), Index]
        public RequestStatuses Status { get; set; }

        [MaxLength]
        public string Query { get; set; }

        public Guid? ParentRequestID { get; set; }
        public virtual Request ParentRequest { get; set; }

        public virtual RequestStatistics Statistics { get; set; }

        public virtual ICollection<RequestDataMart> DataMarts { get; set; }
        public virtual ICollection<RequestSharedFolderRequest> Folders { get; set; }
        public virtual ICollection<AclRequest> RequestAcls { get; set; }
        public virtual ICollection<RequestSearchTerm> SearchTerms { get; set; }
        public virtual ICollection<Audit.SubmittedRequestNeedsApprovalLog> SubmittedRequestNeedsApprovalLogs { get; set; }
        public virtual ICollection<Audit.SubmittedRequestAwaitsResponseLog> SubmittedRequestAwaitsResponseLogs { get; set; }
        public virtual ICollection<Audit.NewRequestSubmittedLog> NewRequestSubmittedLogs { get; set; }
        public virtual ICollection<Audit.RequestStatusChangedLog> RequestStatusChangeLogs { get; set; }
        public virtual ICollection<Audit.ResultsReminderLog> ResultsReminderLogs { get; set; }
        public virtual ICollection<Audit.RequestMetadataChangeLog> MetadataChangeLogs { get; set; }

        public virtual ICollection<RequestUser> Users { get; set; }
        public virtual ICollection<RequestObserver> RequestObservers { get; set; }

        //NOTE: legacy support
        public static readonly Lpp.Security.SecurityObjectKind ObjectKind = new Lpp.Security.SecurityObjectKind("Request");
        [NotMapped]
        Lpp.Security.SecurityObjectKind Lpp.Security.ISecurityObject.Kind
        {
            get
            {
                return ObjectKind;
            }
        }


        public IQueryable<DataMart> GetGrantedDataMarts(DataContext dbContext, ApiIdentity identity)
        {
            var securityGroups = dbContext.SecurityGroupUsers.Where(u => u.UserID == identity.ID).Select(u => u.SecurityGroupID).ToArray();
            var datamarts = dbContext.DataMarts
                                            .Include(dm => dm.Organization)
                                            .Include(dm => dm.Models)
                                            .Include(dm => dm.Projects)
                                            .Secure<DataMart>(dbContext, identity)
                                            .Where(dm => !dm.Deleted                                                
                                                && (
                                                    //legacy requests
                                                    dm.Models.Any(m => m.Model.RequestTypes.Any(r => r.RequestTypeID == this.RequestTypeID) && dm.AdapterID.HasValue == false) ||
                                                    //QE with specified models
                                                    dm.Adapter.RequestTypes.Any(rtm => rtm.RequestTypeID == this.RequestTypeID && rtm.RequestType.WorkflowID.HasValue) ||
                                                    //QE without specified models
                                                    dbContext.RequestTypes.Where(rt => rt.ID == this.RequestTypeID && rt.Models.Any() == false && rt.WorkflowID.HasValue).Any()
                                                )
                                                && dm.Projects.Any(p => p.ProjectID == ProjectID)
                                                && (dm.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == this.RequestTypeID && a.Permission > 0 && securityGroups.Contains(a.SecurityGroupID))
                                                || dm.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == this.RequestTypeID && a.ProjectID == ProjectID && a.Permission > 0 && securityGroups.Contains(a.SecurityGroupID))
                                                || dbContext.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == this.RequestTypeID && a.ProjectID == ProjectID && a.Permission > 0 && securityGroups.Contains(a.SecurityGroupID))
                )
                );

            return datamarts;
        }

    }

    internal class RequestConfiguration : EntityTypeConfiguration<Request>
    {
        public RequestConfiguration()
        {
            HasMany(t => t.DataMarts).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.Folders).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestAcls).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.SearchTerms).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.SubmittedRequestNeedsApprovalLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.SubmittedRequestAwaitsResponseLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.NewRequestSubmittedLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestStatusChangeLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.ResultsReminderLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.Users).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.RequestObservers).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
            HasMany(t => t.MetadataChangeLogs).WithRequired(t => t.Request).HasForeignKey(t => t.RequestID).WillCascadeOnDelete(true);
        }
    }

    internal class RequestSecurityConfiguration : DnsEntitySecurityConfiguration<Request>
    {

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Request[] requests)
        {
            var projectID = requests.Select(r => r.ProjectID).Distinct();

            ExtendedQuery filter = new ExtendedQuery
            {
                Projects = a => projectID.Contains(a.ProjectID),
                ProjectOrganizations = a => projectID.Contains(a.ProjectID)
            };

            var result = await db.Filter(db.Projects, identity, filter, PermissionIdentifiers.Request.Edit).AnyAsync();
            return result;
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Request.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Request.Edit);
        }

        public override IQueryable<Request> SecureList(DataContext db, IQueryable<Request> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return db.FilteredRequestList(identity.ID);
            }
            else
            {
                var predicate = PredicateBuilder.False<Request>();

                //You get to see your own reguardless of security.
                //Data mart administrators get to see any request that is submitted to their data mart.
                var result = query.Where(r => r.CreatedByID == identity.ID || r.SubmittedByID == identity.ID);

                //Standard security applies
                var result2 = db.Filter(query, identity, permissions).Where(request => !request.Private || (request.Private && request.CreatedByID == identity.ID));

                var result3 = (from q in query.SelectMany(r => r.DataMarts) join dm in db.Filter(db.DataMarts, identity, PermissionIdentifiers.DataMartInProject.SeeRequests) on q.DataMartID equals dm.ID select q.Request).Where(r => r.SubmittedOn.HasValue);


                return result.Concat(result2).Concat(result3).Distinct();
            }
        }

        public override Expression<Func<AclRequest, bool>> RequestFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.RequestID);
        }

        public override Expression<Func<AclProject, bool>> ProjectFilter(params Guid[] objIDs)
        {
            return a => a.Project.Requests.Any(r => objIDs.Contains(r.ID));
        }

        public override Expression<Func<AclDataMart, bool>> DataMartFilter(params Guid[] objIDs)
        {
            return a => a.DataMart.Requests.Any(r => objIDs.Contains(r.RequestID));
        }

        public override Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Requests.Any(r => objIDs.Contains(r.ID));
        }

        public override Expression<Func<AclProjectOrganization, bool>> ProjectOrganzationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Requests.Any(r => objIDs.Contains(r.ID) && a.Project.Requests.Any(rq => objIDs.Contains(rq.ID)));
        }

        public override Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMartFilter(Guid[] objIDs)
        {
            return a => a.Organization.Requests.Any(r => objIDs.Contains(r.ID)) && a.DataMart.Requests.Any(r => objIDs.Contains(r.RequestID));
        }

        public override Expression<Func<AclUser, bool>> UserFilter(params Guid[] objIDs)
        {
            return a => a.User.CreatedRequests.Any(cu => objIDs.Contains(cu.ID));
        }

        public override Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return a => a.Project.Requests.Any(r => objIDs.Contains(r.ID)) && a.DataMart.Requests.Any(r => objIDs.Contains(r.RequestID));
        }

        public override Expression<Func<AclRequestSharedFolder, bool>> RequestSharedFolderFilter(params Guid[] objIDs)
        {
            return a => a.RequestSharedFolder.Requests.Any(f => objIDs.Contains(f.RequestID));
        }
        
    }

    internal class RequestDtoMappingConfiguration : EntityMappingConfiguration<Request, RequestDTO>
    {
        public override Expression<Func<Request, RequestDTO>> MapExpression
        {
            /**NOTE: When adding new properties to Request make sure to update dbo.FilteredRequestList tvf to return the new column.
             * Otherwise you get an invalid column name when trying to map to RequestDTO going through the table value function. **/
            get
            {
                return r => new RequestDTO
                {
                    ActivityDescription = r.ActivityDescription,
                    ActivityID = r.ActivityID,
                    Deleted = r.Deleted,
                    Description = r.Description,
                    AdditionalInstructions = r.AdditionalInstructions,
                    ID = r.ID,
                    Identifier = r.Identifier,
                    MSRequestID = r.MSRequestID,
                    UserIdentifier = r.UserIdentifier,
                    IRBApprovalNo = r.IRBApprovalNo,
                    Name = r.Name,
                    CreatedOn = r.CreatedOn,
                    CreatedBy = r.CreatedBy.FirstName + " " + r.CreatedBy.LastName,
                    CreatedByID = r.CreatedByID,
                    CancelledOn = r.CancelledOn,
                    CompletedOn = r.CompletedOn,
                    DueDate = r.DueDate,
                    Organization = r.Organization.Name,
                    OrganizationID = r.OrganizationID,
                    MajorEventDate = r.CancelledOn ?? r.RejectedOn ?? r.SubmittedOn ?? r.CreatedOn,
                    MajorEventByID = r.CancelledByID ?? r.RejectedByID ?? r.SubmittedByID ?? r.CreatedByID,
                    MajorEventBy = r.CancelledByID != null ? r.CancelledBy.UserName : r.RejectedByID != null ? r.RejectedBy.UserName : r.SubmittedByID != null ? r.SubmittedBy.UserName : r.CreatedBy.UserName,
                    MirrorBudgetFields = r.MirrorBudgetFields,
                    PhiDisclosureLevel = r.PhiDisclosureLevel,
                    Priority = r.Priority,
                    Project = r.Project.Name,
                    ProjectID = r.ProjectID,
                    PurposeOfUse = r.PurposeOfUse,
                    RequesterCenter = r.RequesterCenter.Name,
                    RequesterCenterID = r.RequesterCenterID,
                    RequestTypeID = r.RequestTypeID,
                    RequestType = r.RequestType.Name,
                    ReportAggregationLevelID = r.ReportAggregationLevelID,
                    ReportAggregationLevel = r.ReportAggregationLevel.Name,
                    AdapterPackageVersion = r.AdapterPackageVersion,
                    Schedule = r.Schedule,
                    ScheduleCount = r.ScheduleCount,
                    Scheduled = r.Scheduled,
                    SourceActivityID = r.SourceActivityID,
                    SourceActivityProjectID = r.SourceActivityProjectID,
                    SourceTaskOrderID = r.SourceTaskOrderID,
                    Status = r.Status,
                    Query = r.Query,
                    StatusText = r.Status == RequestStatuses.AwaitingRequestApproval ? "Awaiting Request Approval" :
                                r.Status == RequestStatuses.AwaitingResponseApproval ? "Awaiting Response Approval" :
                                r.Status == RequestStatuses.Cancelled ? "Cancelled" :
                                r.Status == RequestStatuses.Complete ? "Complete" :
                                r.Status == RequestStatuses.Draft ? (r.Scheduled ? "Scheduled" : "Draft") :
                                r.Status == RequestStatuses.DraftReview ? "Draft Pending Review" :
                                r.Status == RequestStatuses.ExaminedByInvestigator ? "Examined By Investigator" :
                                r.Status == RequestStatuses.Failed ? "Failed" :
                                r.Status == RequestStatuses.Hold ? "Hold" :
                                r.Status == RequestStatuses.PartiallyComplete ? r.DataMarts.Where(d => d.Status == RoutingStatus.Completed || d.Status == RoutingStatus.ResultsModified || d.Status == RoutingStatus.ResponseRejectedAfterUpload || d.Status == RoutingStatus.ResponseRejectedBeforeUpload || d.Status == RoutingStatus.RequestRejected).Count().ToString() + " / " + r.DataMarts.Where(d => d.Status != RoutingStatus.Canceled).Count().ToString() + " Responses Received" :
                                r.Status == RequestStatuses.PendingUpload ? "Pending Upload" :
                                r.Status == RequestStatuses.RequestRejected ? "Request Rejected" :
                                r.Status == RequestStatuses.ResponseRejectedAfterUpload ? "Response Rejected After Upload" :
                                r.Status == RequestStatuses.ResponseRejectedBeforeUpload ? "Response Rejected Before Upload" :
                                r.Status == RequestStatuses.Resubmitted ? "Resubmitted" : 
                                r.Status == RequestStatuses.Submitted ? "Submitted" :
                                r.Status == RequestStatuses.ThirdPartySubmittedDraft ? "3rd Party Draft" :
                                r.Status == RequestStatuses.PendingWorkingSpecification ? "Pending Working Specifications" :
                                r.Status == RequestStatuses.WorkingSpecificationPendingReview ? "Working Specification Pending Review" :
                                r.Status == RequestStatuses.SpecificationsPendingReview ? "Specifications Pending Review" :
                                r.Status == RequestStatuses.PendingSpecifications ? "Pending Specifications" :
                                r.Status == RequestStatuses.PendingPreDistributionTesting ? "Pending Pre-Distribution Testing" :
                                r.Status == RequestStatuses.PreDistributionTestingPendingReview ? "Pre-Distribution Testing Pending Review" :
                                r.Status == RequestStatuses.RequestPendingDistribution ? "Request Pending Distribution" :
                                r.Status == RequestStatuses.PendingDraftReport ? "Pending Draft Report" :
                                r.Status == RequestStatuses.DraftReportPendingApproval ? "Draft Report Pending Review" :
                                r.Status == RequestStatuses.PendingFinalReport ? "Pending Final Report" :
                                r.Status == RequestStatuses.PendingFinalReport ? "Final Report Pending Review" :
                                r.Status == RequestStatuses.CompleteWithReport ? "Complete, with Report" :
                                r.Status == RequestStatuses.TerminatedPriorToDistribution ? "Terminated" :
                                r.WorkFlowActivityID.HasValue ? r.WorkflowActivity.Name :
                                "Unknown",
                    SubmittedBy = r.SubmittedBy.UserName,
                    SubmittedByName = r.SubmittedBy.FirstName + " " + r.SubmittedBy.LastName,
                    SubmittedByID = r.SubmittedByID,
                    SubmittedOn = r.SubmittedOn,
                    Template = r.Template,
                    Timestamp = r.Timestamp,
                    UpdatedBy = r.UpdatedBy.UserName,
                    UpdatedByID = r.UpdatedByID,
                    UpdatedOn = r.UpdatedOn,
                    WorkplanType = r.WorkplanType.Name,
                    WorkplanTypeID = r.WorkplanTypeID,
                    CurrentWorkFlowActivityID = r.WorkFlowActivityID,
                    CurrentWorkFlowActivity = r.WorkflowActivity.Name,
                    WorkflowID = r.WorkflowID,
                    Workflow = r.Workflow.Name,
                    ParentRequestID = r.ParentRequestID
                };
            /**NOTE: When adding new properties to Request make sure to update dbo.FilteredRequestList tvf to return the new column.
            * Otherwise you get an invalid column name when trying to map to RequestDTO going through the table value function. **/
            }
        }
    }

    public  class RequestLogConfiguration : EntityLoggingConfiguration<DataContext, Request>
    {

        public override IEnumerable<AuditLog> ProcessEvents(System.Data.Entity.Infrastructure.DbEntityEntry obj, DataContext db, ApiIdentity identity, bool read)
        {
            var logs = new List<AuditLog>();

            var request = obj.Entity as Request;
            if (request == null)
                throw new InvalidCastException("The entity passed is not a request");

            Guid? currentTaskID = null;
            if (request.WorkFlowActivityID.HasValue)
            {
                currentTaskID = PmnTask.GetActiveTaskIDForRequestActivity(request.ID, request.WorkFlowActivityID, db);
            }

            //var test = (RequestStatuses)obj.CurrentValues["Status"];
            //var testb = obj.CurrentValues["Status"];

            if (request.WorkFlowActivityID.HasValue && 
                obj.State == System.Data.Entity.EntityState.Added &&
                (RequestStatuses)obj.CurrentValues["Status"] == 0)
            {

                var orgUser = db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();

                var requestType = db.RequestTypes.Where(rt => rt.ID == request.RequestTypeID).Select(rt => rt.Name).FirstOrDefault();

                //New Request Draft Submitted
                var logItem = new Audit.NewRequestDraftSubmittedLog
                {
                    Description = string.Format("New request draft of type '{0}' has been submitted by {1}", requestType, (orgUser.Acronym + @"\" + orgUser.UserName)),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestID = request.ID,
                    Request = request
                };

                db.LogsNewRequestDraftSubmitted.Add(logItem);
                logs.Add(logItem);
            }

            if ((RequestStatuses) obj.CurrentValues["Status"] == RequestStatuses.AwaitingRequestApproval && (obj.State == System.Data.Entity.EntityState.Added || !obj.OriginalValues["Status"].Equals(obj.CurrentValues["Status"])))
            {
                //Submitted Request Needs Approval
                var logItem = new Audit.SubmittedRequestNeedsApprovalLog
                {
                    Description = string.Format("Request '{0}' needs approval in order to be processed", request.Name),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestID = request.ID,
                    Request = request,
                    TaskID = currentTaskID
                };

                db.LogsSubmittedrequestNeedsApproval.Add(logItem);
                logs.Add(logItem);
            }
            
            //only send the new request submitted notification at this level when it is the initial submit and it doesn't have to be to specific datamarts.
            if (((RequestStatuses)obj.CurrentValues["Status"] == RequestStatuses.Submitted) && 
                (obj.State == System.Data.Entity.EntityState.Added || !obj.OriginalValues["Status"].Equals(obj.CurrentValues["Status"]))
               )
            {

                //Submitted Request Awaiting Response
                foreach (var dataMart in request.DataMarts.DistinctBy(dm => dm.RequestID))
                {
                    var userID = identity == null ? Guid.Empty : identity.ID;
                    if (dataMart.Status == RoutingStatus.Submitted && !db.LogsSubmittedRequestAwaitsResponse.Any(l => l.UserID == userID && l.RequestID == request.ID))
                    {
                        if (db.Entry(dataMart).Reference(r => r.DataMart).IsLoaded == false)
                            db.Entry(dataMart).Reference(r => r.DataMart).Load();

                        var logItem = new Audit.SubmittedRequestAwaitsResponseLog
                        {
                            Description = string.Format("{0} has not responded to {1}. ", dataMart.DataMart.Name, request.Name),
                            UserID = userID,
                            RequestID = request.ID,                            
                            Request = request,
                            TaskID = currentTaskID
                        };

                        db.LogsSubmittedRequestAwaitsResponse.Add(logItem);
                        logs.Add(logItem);
                    }
                }

                var orgUser = identity == null ? new { UserName = "", Acronym = "" } : db.Users.Where(u => u.ID == identity.ID).Select(u => new { u.UserName, u.Organization.Acronym }).FirstOrDefault();
                
                if (db.Entry(request).Reference(r => r.RequestType).IsLoaded == false)
                    db.Entry(request).Reference(r => r.RequestType).Load();

                //New Request Submitted
                var newRequestSubmittedLogItem = new Audit.NewRequestSubmittedLog
                {
                    Description = string.Format("New request of type '{0}' has been submitted by {1}", request.RequestType.Name, (orgUser.Acronym + @"\" + orgUser.UserName)),
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    RequestID = request.ID,
                    Request = request,
                    TaskID = currentTaskID
                };

                db.LogsNewRequestSubmitted.Add(newRequestSubmittedLogItem);
                logs.Add(newRequestSubmittedLogItem);

            }

            if (obj.State != EntityState.Added 
                && (!obj.OriginalValues["Status"].Equals(obj.CurrentValues["Status"])) 
                && (((RequestStatuses) obj.OriginalValues["Status"]) != RequestStatuses.Draft) 
                && (((RequestStatuses) obj.OriginalValues["Status"]) != RequestStatuses.DraftReview)
                && (((RequestStatuses) obj.OriginalValues["Status"]) != RequestStatuses.AwaitingRequestApproval)
                && !request.WorkflowID.HasValue
               )
            {
                //Only allow logs for request status changed if the request is legacy. WF request status changed logs and notifications are fired on the activity 
                //See PMNDEV-5523, PMNDEV-5772, and PMNDEV-5790 for notification sending rules.
                               
                Audit.RequestStatusChangedLog logItem;
                var userID = identity == null ? Guid.Empty : identity.ID;

                logItem = db.LogsRequestStatusChanged.Find(userID, DateTimeOffset.UtcNow, request.ID);

                if (logItem == null)
                {
                    //var rdms = request.DataMarts.Select(dm => dm).OrderByDescending(dm => dm.ResponseTime).GroupBy(dm => dm.ResponseTime);
                    logItem = new Audit.RequestStatusChangedLog
                    {
                        Description = request.Name + " status changed from " + ((RequestStatuses)obj.OriginalValues["Status"]).ToString(true) + " to " + ((RequestStatuses)obj.CurrentValues["Status"]).ToString(true),
                        NewStatus = (RequestStatuses)obj.CurrentValues["Status"],
                        OldStatus = (RequestStatuses)obj.OriginalValues["Status"],
                        Request = request,
                        RequestID = request.ID,
                        UserID = userID,
                        TaskID = currentTaskID
                    };
                    db.LogsRequestStatusChanged.Add(logItem);
                }
                else
                {
                    logItem.TimeStamp = DateTime.UtcNow;
                    logItem.Description = request.Name + " status changed from " + ((RequestStatuses)obj.OriginalValues["Status"]).ToString(true) + " to " + ((RequestStatuses)obj.CurrentValues["Status"]).ToString(true);
                    logItem.NewStatus = (RequestStatuses)obj.CurrentValues["Status"];
                    logItem.OldStatus = (RequestStatuses)obj.OriginalValues["Status"];
                    logItem.TaskID = currentTaskID;
                }


                if (!logs.Contains(logItem))
                    logs.Add(logItem);
                
            }

            if (obj.State == EntityState.Modified)
            {
                //only log metadata changes on modifications, not first save
                LogMetadataChanges(obj, db, identity, logs, request, currentTaskID);
            }

            return logs.AsEnumerable();
        }

        void LogMetadataChanges(DbEntityEntry obj, DataContext db, ApiIdentity identity, List<AuditLog> logs, Request request, Guid? taskID){

            if(!request.Private)
            { 
                var changedProperties = GetMetadataFieldsWithChanges(obj).ToList();
                if (changedProperties.Count == 0)
                    return;

                if (!db.Entry(request).Reference(r => r.Project).IsLoaded)
                    db.Entry(request).Reference(r => r.Project).Load();

                if (!db.Entry(request).Reference(r => r.RequestType).IsLoaded)
                    db.Entry(request).Reference(r => r.RequestType).Load();


                //for the changes that reference only an id, need to hit the db and get the display values
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

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "RequesterCenterID");
                if (changeDetail != null)
                {
                    if (changeDetail.OriginalValue != null)
                    {
                        changeDetail.OriginalValueDisplay = db.RequesterCenters.Where(rc => rc.ID == (Guid)changeDetail.OriginalValue).Select(rc => rc.Name).FirstOrDefault();
                    }
                    if (changeDetail.NewValue != null)
                    {
                        changeDetail.NewValueDisplay = db.RequesterCenters.Where(rc => rc.ID == (Guid)changeDetail.NewValue).Select(rc => rc.Name).FirstOrDefault();
                    }
                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "WorkplanTypeID");
                if (changeDetail != null)
                {
                    if (changeDetail.OriginalValue != null)
                    {
                        changeDetail.OriginalValueDisplay = db.WorkplanTypes.Where(w => w.ID == (Guid)changeDetail.OriginalValue).Select(w => w.Name).FirstOrDefault();
                    }
                    if (changeDetail.NewValue != null)
                    {
                        changeDetail.NewValueDisplay = db.WorkplanTypes.Where(w => w.ID == (Guid)changeDetail.NewValue).Select(w => w.Name).FirstOrDefault();
                    }
                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "ReportAggregationLevelID");
                if (changeDetail != null)
                {
                    if (changeDetail.OriginalValue != null)
                    {
                        changeDetail.OriginalValueDisplay = db.ReportAggregationLevels.Where(ral => ral.ID == (Guid)changeDetail.OriginalValue).Select(ral => ral.Name).FirstOrDefault();
                    }
                    if (changeDetail.NewValue != null)
                    {
                        changeDetail.NewValueDisplay = db.ReportAggregationLevels.Where(ral => ral.ID == (Guid)changeDetail.NewValue).Select(ral => ral.Name).FirstOrDefault();
                    }
                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "ActivityID");
                if (changeDetail != null)
                {
                    var activities = db.Activities.Where(a => a.ProjectID == request.ProjectID && a.Deleted == false).Select(a => new { a.ID, a.ParentActivityID, a.Name, a.TaskLevel }).ToArray();
                
                    changedProperties.Remove(changeDetail);

                    PropertyChangeDetailDTO taskOrderChangeDetail = new PropertyChangeDetailDTO { Property = "TaskOrder", PropertyDisplayName= "Budget Task Order"};
                    PropertyChangeDetailDTO activityChangeDetail = new PropertyChangeDetailDTO { Property = "Activity", PropertyDisplayName = "Budget Activity" };
                    PropertyChangeDetailDTO activityProjectChangeDetail = new PropertyChangeDetailDTO { Property = "ActivityProject", PropertyDisplayName = "Budget Activity Project" };

                    if (changeDetail.OriginalValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.OriginalValue);
                        if (activity != null)
                        {
                            if (activity.TaskLevel == 1)
                            {
                                taskOrderChangeDetail.OriginalValue = activity.ID;
                                taskOrderChangeDetail.OriginalValueDisplay = activity.Name;
                            } 
                            else if (activity.TaskLevel == 2)
                            {
                                activityChangeDetail.OriginalValue = activity.ID;
                                activityChangeDetail.OriginalValueDisplay = activity.Name;

                                var parent = activities.First(a => a.ID == activity.ParentActivityID.Value);
                                taskOrderChangeDetail.OriginalValue = parent.ID;
                                taskOrderChangeDetail.OriginalValueDisplay = parent.Name;
                            }
                            else if (activity.TaskLevel == 3)
                            {
                                activityProjectChangeDetail.OriginalValue = activity.ID;
                                activityProjectChangeDetail.OriginalValueDisplay = activity.Name;

                                var parent = activities.First(a => a.ID == activity.ParentActivityID.Value);
                                activityChangeDetail.OriginalValue = parent.ID;
                                activityChangeDetail.OriginalValueDisplay = parent.Name;

                                var parentParent = activities.First(a => a.ID == parent.ParentActivityID.Value);
                                taskOrderChangeDetail.OriginalValue = parentParent.ID;
                                taskOrderChangeDetail.OriginalValueDisplay = parentParent.Name;
                            }
                        }
                    }

                    if (changeDetail.NewValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.NewValue);
                        if (activity != null)
                        {
                            if (activity.TaskLevel == 1)
                            {
                                taskOrderChangeDetail.NewValue = activity.ID;
                                taskOrderChangeDetail.NewValueDisplay = activity.Name;
                            }
                            else if (activity.TaskLevel == 2)
                            {
                                activityChangeDetail.NewValue = activity.ID;
                                activityChangeDetail.NewValueDisplay = activity.Name;

                                var parent = activities.First(a => a.ID == activity.ParentActivityID.Value);
                                taskOrderChangeDetail.NewValue = parent.ID;
                                taskOrderChangeDetail.NewValueDisplay = parent.Name;
                            }
                            else if (activity.TaskLevel == 3)
                            {
                                activityProjectChangeDetail.NewValue = activity.ID;
                                activityProjectChangeDetail.NewValueDisplay = activity.Name;

                                var parent = activities.First(a => a.ID == activity.ParentActivityID.Value);
                                activityChangeDetail.NewValue = parent.ID;
                                activityChangeDetail.NewValueDisplay = parent.Name;

                                var parentParent = activities.First(a => a.ID == parent.ParentActivityID.Value);
                                taskOrderChangeDetail.NewValue = parentParent.ID;
                                taskOrderChangeDetail.NewValueDisplay = parentParent.Name;
                            }
                        }
                    }

                    if (taskOrderChangeDetail.OriginalValue != null || taskOrderChangeDetail.NewValue != null)
                        changedProperties.Add(taskOrderChangeDetail);
                    if (activityChangeDetail.OriginalValue != null || activityChangeDetail.NewValue != null)
                        changedProperties.Add(activityChangeDetail);
                    if (activityProjectChangeDetail.OriginalValue != null || activityProjectChangeDetail.NewValue != null)
                        changedProperties.Add(activityProjectChangeDetail);

                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "SourceActivityID");
                if (changeDetail != null)
                {
                    var activities = db.Activities.Where(a => a.ProjectID == request.ProjectID && a.Deleted == false).ToArray();

                    changedProperties.Remove(changeDetail);
                    
                    PropertyChangeDetailDTO sourceActivityChangeDetail = new PropertyChangeDetailDTO { Property = "SourceActivity", PropertyDisplayName = "Source Activity" };
                    if (changeDetail.OriginalValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.OriginalValue);
                        if (activity != null)
                        {
                            sourceActivityChangeDetail.OriginalValue = activity.ID;
                            sourceActivityChangeDetail.OriginalValueDisplay = activity.Name;
                        }
                    }
                    if (changeDetail.NewValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.NewValue);
                        sourceActivityChangeDetail.NewValue = activity.ID;
                        sourceActivityChangeDetail.NewValueDisplay = activity.Name;
                    }
                    if (sourceActivityChangeDetail.OriginalValue != null || sourceActivityChangeDetail.NewValue != null)
                        changedProperties.Add(sourceActivityChangeDetail);
                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "SourceActivityProjectID");
                if (changeDetail != null)
                {
                    var activities = db.Activities.Where(a => a.ProjectID == request.ProjectID && a.Deleted == false).ToArray();

                    changedProperties.Remove(changeDetail);
                    
                    PropertyChangeDetailDTO sourceActivityProjectChangeDetail = new PropertyChangeDetailDTO { Property = "SourceActivityProject", PropertyDisplayName = "Source Activity Project" };
                    if (changeDetail.OriginalValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.OriginalValue);
                        if (activity != null)
                        {
                            sourceActivityProjectChangeDetail.OriginalValue = activity.ID;
                            sourceActivityProjectChangeDetail.OriginalValueDisplay = activity.Name;
                        }
                    }
                    if (changeDetail.NewValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.NewValue);
                        sourceActivityProjectChangeDetail.NewValue = activity.ID;
                        sourceActivityProjectChangeDetail.NewValueDisplay = activity.Name;
                    }
                    if (sourceActivityProjectChangeDetail.OriginalValue != null || sourceActivityProjectChangeDetail.NewValue != null)
                        changedProperties.Add(sourceActivityProjectChangeDetail);
                }

                changeDetail = changedProperties.FirstOrDefault(f => f.Property == "SourceTaskOrderID");
                if (changeDetail != null)
                {
                    var activities = db.Activities.Where(a => a.ProjectID == request.ProjectID && a.Deleted == false).ToArray();

                    changedProperties.Remove(changeDetail);
                    
                    PropertyChangeDetailDTO sourceTaskOrderChangeDetail = new PropertyChangeDetailDTO { Property = "SourceTaskOrder", PropertyDisplayName = "Source Task Order" };
                    if (changeDetail.OriginalValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.OriginalValue);
                        if (activity != null)
                        {
                            sourceTaskOrderChangeDetail.OriginalValue = activity.ID;
                            sourceTaskOrderChangeDetail.OriginalValueDisplay = activity.Name;
                        }
                    }
                    if (changeDetail.NewValue != null)
                    {
                        var activity = activities.FirstOrDefault(a => a.ID == (Guid)changeDetail.NewValue);
                        sourceTaskOrderChangeDetail.NewValue = activity.ID;
                        sourceTaskOrderChangeDetail.NewValueDisplay = activity.Name;
                    }
                    if (sourceTaskOrderChangeDetail.OriginalValue != null || sourceTaskOrderChangeDetail.NewValue != null)
                        changedProperties.Add(sourceTaskOrderChangeDetail);
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

                var log = new Audit.RequestMetadataChangeLog
                {
                    UserID = identity == null ? Guid.Empty : identity.ID,
                    Request = request,
                    RequestID = request.ID,
                    TaskID = taskID,
                    Description = description.ToString()
                };

                var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings{
                    Formatting = Newtonsoft.Json.Formatting.None,
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter{ DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });

                log.ChangeDetail = Newtonsoft.Json.JsonConvert.SerializeObject(changedProperties, serializerSettings);

                db.LogsRequestMetadataChange.Add(log);
                logs.Add(log);
            }
        }

        /// <summary>
        /// Property names that are monitored for metadata changes.
        /// </summary>
        //readonly string[] MetadataPropertyNames = new[] { "Name", "MSRequestID", "Priority", "DueDate", "RequesterCenterID", "PurposeOfUse", "PhiDisclosureLevel", "WorkplanTypeID", "ActivityID", "Description" };

        readonly Dictionary<string, string> MetadataPropertyNames = new Dictionary<string, string> {
            {"Name", "Name"},
            {"MSRequestID", "Request ID"},
            {"Priority", "Priority"},
            {"DueDate", "Due Date"},
            {"RequesterCenterID", "Request Center"},
            {"PurposeOfUse", "Purpose Of Use"},
            {"PhiDisclosureLevel", "Level of PHI"},
            {"WorkplanTypeID", "Workplan Type"},
            {"ActivityID", "Activity"},
            {"Description", "Description"},
            {"SourceActivityID", "Source Activity"},
            {"SourceActivityProjectID", "Source Activity Project"},
            {"SourceTaskOrderID", "Source Task Order"},
            {"ReportAggregationLevelID", "Level of Report Aggregation"}
        };
        
        /// <summary>
        /// Gets a collection of metadata property names that have changed.
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
            if (typeof(T) == typeof(Audit.SubmittedRequestNeedsApprovalLog))
            {
                var log = logItem as Audit.SubmittedRequestNeedsApprovalLog;
                var actingUser = db.Users.Where(u => u.ID == log.UserID).FirstOrDefault();

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                log.Request = db.Requests.Where(r => r.ID == log.RequestID).Include("RequestType").Include("DataMarts").FirstOrDefault();

                var body = GenerateTimestampText(log) +
                    "<p>Here are your most recent <b>Request Approval</b> notifications from <b>" + networkName + "</b>.</p>" +
                    "<p><b>" + (actingUser != null ? actingUser.FullName : "Unknown User") + "</b> has submitted a new <b>" + log.Request.RequestType.Name + "</b> request <b>" + log.Request.Name +
                           "</b> requiring your approval.";

                var dataMartIDs = log.Request.DataMarts.Select(d => d.DataMartID);

                var notification = new Notification
                {
                    Subject = "Request Approval Notification",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID && !s.User.Deleted && s.User.Active &&
                                        (
                                            (db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active))
                                          || db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                          || db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active) && a.OrganizationID == log.Request.OrganizationID).Any()
                                          || db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).Any()
                                          || db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active) && dataMartIDs.Contains(a.DataMartID)).Any()
                                            )
                                        &&
                                            (db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                          && db.GlobalEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                          && db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active) && a.OrganizationID == log.Request.OrganizationID).All(a => a.Allowed)
                                          && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active)).All(a => a.Allowed)
                                          && db.ProjectDataMartEvents.Where(a => a.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID
                                                                  && a.SecurityGroup.Users.Any(u => u.UserID == s.UserID && !u.User.Deleted && u.User.Active) && dataMartIDs.Contains(a.DataMartID)).All(a => a.Allowed)
                                            )
                                        )
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  select new Recipient
                                  {
                                      Email = s.User.Email,
                                      Phone = s.User.Phone,
                                      Name = ((s.User.FirstName + " " + s.User.MiddleName).Trim() + " " + s.User.LastName).Trim(),
                                      UserID = s.UserID
                                  }).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.SubmittedRequestAwaitsResponseLog))
            {
                var log = logItem as Audit.SubmittedRequestAwaitsResponseLog;

                var q = (from rdm in db.RequestDataMarts
                         let n = db.Networks.Select(n => n.Name).FirstOrDefault()
                         join pp in db.Projects on rdm.Request.ProjectID equals pp.ID into projects
                         from prj in projects.DefaultIfEmpty()
                         join rrt in db.RequestTypes on rdm.Request.RequestTypeID equals rrt.ID into requestTypes
                         from rt in requestTypes.DefaultIfEmpty()
                         where rdm.RequestID == log.RequestID
                         select new {
                             RequestName = rdm.Request.Name,
                             NetworkName = n,
                             RequestType = rt.Name,
                             Project = prj.Name,
                             DataMartName = rdm.DataMart.Name
                         })
                        .GroupBy(k => new { k.RequestName, k.RequestType, k.NetworkName, k.Project })
                        .Select(k => new
                        {
                            k.Key.RequestName,
                            k.Key.NetworkName,
                            k.Key.RequestType,
                            k.Key.Project,
                            DataMarts = k.Select(v => v.DataMartName)
                        });

                var details = q.FirstOrDefault();

                if (details == null)
                {
                    return Enumerable.Empty<Notification>();
                }

                var body = GenerateTimestampText(log) +
                           "<p>Here are your most recent <b>Submitted Request Awaits a Response</b> notifications from <b>" + details.NetworkName + "</b>.</p>" +
                           "<p>This is a reminder that <b>" + (details.RequestType ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + "</b> request <b>" + details.RequestName + "</b>, submitted to the following DataMarts" +
                           " in project <b>" + (details.Project ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + "</b>, has not yet been responded to. </p>";

                body += "<p>Please respond to the request using the DataMart Client.</p>";
                body += "<h3>DataMarts</h3><p>" + string.Join(",", details.DataMarts) + "</p>";

                var notification = new Notification
                {
                    Subject = "Submitted Request Awaits a Response Notification",
                    Body = body,
                    Recipients = (from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID, immediate)
                                  join u in db.Users on s.UserID equals u.ID
                                  let projectEventAcls = db.ProjectEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let organizationEventAcls = db.OrganizationEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Organization.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                                   && (
                                        (projectEventAcls.Any() || organizationEventAcls.Any() || projectDataMartEventAcls.Any())
                                        &&
                                        (projectEventAcls.All(a => a.Allowed) && organizationEventAcls.All(a => a.Allowed) && projectDataMartEventAcls.All(a => a.Allowed))
                                   )
                                  select
                                        new Recipient
                                        {
                                            Email = u.Email,
                                            Phone = u.Phone,
                                            Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                            UserID = s.UserID
                                        }).ToArray()
                };

                return new[] { notification };
            }

            else if (typeof(T) == typeof(Audit.NewRequestDraftSubmittedLog))
            {
                var log = logItem as Audit.NewRequestDraftSubmittedLog;

                var q = from r in db.Requests
                        join u in db.Users on log.UserID equals u.ID into users
                        from u in users.DefaultIfEmpty()
                        join p in db.Projects on r.ProjectID equals p.ID into projects
                        from p in projects.DefaultIfEmpty()
                        join rt in db.RequestTypes on r.RequestTypeID equals rt.ID into requestTypes
                        from rt in requestTypes.DefaultIfEmpty()
                        let network = db.Networks.Select(n => n.Name).FirstOrDefault()
                        where r.ID == log.RequestID
                        select new
                        {
                            RequestName = r.Name,
                            ProjectName = p.Name,
                            NetworkName = network,
                            RequestTypeName = rt.Name,
                            User = u
                        };

                var details = q.FirstOrDefault();
                if(details == null)
                {
                    return Enumerable.Empty<Notification>();
                }
            

                var body = GenerateTimestampText(log) +
                           "<p>Here are your most recent <b>New Request Draft Submitted</b> notifications from " + details.NetworkName + ".</p>" +
                           "<p> A new <b>" + (details.RequestTypeName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + "</b> request draft <b>" + details.RequestName +
                           "</b> was submitted by " + (details.User == null ? System.Web.HttpUtility.HtmlEncode("<<Not Found>>") : details.User.FullName) + " in project <b>" + (details.ProjectName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + "</b>.";

                var notification = new Notification
                {
                    Subject = "New Request Draft Submitted Notification",
                    Body = body,
                    Recipients = (from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.NewRequestDraftSubmitted.ID, immediate)
                                  join u in db.Users on s.UserID equals u.ID
                                  let projectEventAcls = db.ProjectEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestDraftSubmitted.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let organizationEventAcls = db.OrganizationEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestDraftSubmitted.ID && e.Organization.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestDraftSubmitted.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                                   && (
                                        (projectEventAcls.Any() || organizationEventAcls.Any() || projectDataMartEventAcls.Any())
                                        &&
                                        (projectEventAcls.All(a => a.Allowed) && organizationEventAcls.All(a => a.Allowed) && projectDataMartEventAcls.All(a => a.Allowed))
                                   )
                                  select
                                        new Recipient
                                        {
                                            Email = u.Email,
                                            Phone = u.Phone,
                                            Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                            UserID = s.UserID
                                        }).ToArray()
                };

                return new[] { notification };

            }

            else if (typeof(T) == typeof(Audit.NewRequestSubmittedLog))
            {
                var log = logItem as Audit.NewRequestSubmittedLog;

                var networkName = db.Networks.Select(n => n.Name).FirstOrDefault();
                log.Request = db.Requests.Where(r => r.ID == log.RequestID).Include("RequestType").Include("Activity.ParentActivity.ParentActivity")
                    .Include("RequesterCenter").Include("WorkplanType").Include("ReportAggregationLevel").Include("Project").Include("SubmittedBy").FirstOrDefault();

                string userName = string.Empty;
                if (db.Users.Any(p => p.ID == log.UserID))
                    userName = db.Users.First(p => p.ID == log.UserID).FullName;
                else if (log.Request.SubmittedByID.HasValue && db.Users.Any(p => p.ID == log.Request.SubmittedByID))
                    userName = db.Users.First(p => p.ID == log.Request.SubmittedByID).FullName;

                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>New Request Submitted</b> notifications from " + networkName + ".</p>" +
                           "<p>" + userName + " has submitted a new <b>" + log.Request.RequestType.Name + "</b> request " + log.Request.Name +
                           " in project <b>" + log.Request.Project.Name + "</b>.";

                //get source activities text for body of email
                var activities = db.Activities.Where(a => a.ProjectID == log.Request.ProjectID && a.Deleted == false).ToArray();
                var sourceActivity = activities.FirstOrDefault(a => a.ID == log.Request.SourceActivityID);
                var sourceActivityProject = activities.FirstOrDefault(a => a.ID == log.Request.SourceActivityProjectID);
                var sourceTaskOrder = activities.FirstOrDefault(a => a.ID == log.Request.SourceTaskOrderID);

                body += "<h3>Description</h3><p>" + log.Request.Description + "</p>";
                body += "<h3>Request Details</h3>" +
                        "Request type: <b>" + log.Request.RequestType.Name + "</b><br/>" +
                        "Request Name: <b>" + log.Request.Name + "</b><br/>" +
                        "System ID: <b>" + log.Request.Identifier + "</b><br/>" +
                        "Request ID: <b>" + log.Request.MSRequestID + "</b><br/>" +
                        "Project: <b>" + log.Request.Project.Name + "</b><br/>" +
                        "Budget Item: <b>" + (log.Request.ActivityID != null && log.Request.Activity.ParentActivityID != null && log.Request.Activity.ParentActivity.ParentActivity != null ? log.Request.Activity.ParentActivity.ParentActivity.Name : "") +
                                            (log.Request.ActivityID != null && log.Request.Activity.ParentActivityID != null ? "; " + log.Request.Activity.ParentActivity.Name : "") +
                                            (log.Request.ActivityID != null ? "; " +  log.Request.Activity.Name : "") + "</b><br/>" +
                        "Source Item: <b>" + (sourceTaskOrder != null ? sourceTaskOrder.Name : "") +
                                            (sourceActivity != null ?  "; " + sourceActivity.Name : "") +
                                            (sourceActivityProject != null ? "; " + sourceActivityProject.Name : "") + "</b><br/>" +
                        "Request Due Date: <b>" + (log.Request.DueDate != null ? log.Request.DueDate.Value.ToShortDateString() : "") + "</b><br/>" +
                        "Contact Person: <b>" + log.Request.SubmittedBy.FullName + " (" + log.Request.SubmittedBy.Email + ")" + "</b><br/>" +
                        "Priority: <b>" + log.Request.Priority + "</b><br/>" +
                        "Purpose of Use: <b>" + (TranslatePurposeOfUse(log.Request.PurposeOfUse) ?? "") + "</b><br/>" +
                        "Level of PHI Disclosure: <b>" + (log.Request.PhiDisclosureLevel ?? "") + "</b><br/>" +
                        "Level of Report Aggregation: <b>" + (log.Request.ReportAggregationLevelID != null && log.Request.ReportAggregationLevelID != Guid.Empty ? log.Request.ReportAggregationLevel.Name : "" ) + "</b><br/>" +
                        "Requester Center: <b>" + (log.Request.RequesterCenterID != null && log.Request.RequesterCenterID != Guid.Empty ? log.Request.RequesterCenter.Name : "") + "</b><br/>" +
                        "Workplan Type: <b>" + (log.Request.WorkplanTypeID != null && log.Request.WorkplanTypeID != Guid.Empty ? log.Request.WorkplanType.Name : "") + "</b><br/>" +
                        "<h3>Additional Instructions</h3><br/><pre>" + log.Request.AdditionalInstructions + "</pre><br/>";

                body += "<h3>DataMarts</h3><p>" + string.Join(",<br/>", db.RequestDataMarts.Where(dm => dm.RequestID == log.RequestID).Select(dm => dm.DataMart.Name).ToArray()) + "</p>";

                var recipients = (from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.NewRequestSubmitted.ID, immediate)
                                  join u in db.Users on s.UserID equals u.ID
                                  let projectEventAcls = db.ProjectEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestSubmitted.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let organizationEventAcls = db.OrganizationEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestSubmitted.ID && e.Organization.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.NewRequestSubmitted.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                                   && (
                                        (projectEventAcls.Any() || organizationEventAcls.Any() || projectDataMartEventAcls.Any())
                                        &&
                                        (projectEventAcls.All(a => a.Allowed) && organizationEventAcls.All(a => a.Allowed) && projectDataMartEventAcls.All(a => a.Allowed))
                                   )
                                  select new Recipient
                                  {
                                      Email = u.Email,
                                      Phone = u.Phone,
                                      Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                      UserID = u.ID
                                  }).ToArray();

                var userObservers = (from u in db.Users
                                     let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == log.RequestID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.NewRequestSubmitted.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                where requestObservers.Any()
                                select new Recipient
                                {
                                    Email = u.Email,
                                    Phone = u.Phone,
                                    Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                    UserID = u.ID
                                }).ToArray();

                var emailObservers = (from o in db.RequestObservers
                                      where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.NewRequestSubmitted.ID)
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

                var notification = new Notification
                {
                    Subject = "New Request Submitted Notification",
                    Body = body,
                    Recipients = recipients
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.RequestStatusChangedLog))
            {
                var log = logItem as Audit.RequestStatusChangedLog;

                string[] emailText;

                try
                {
                    emailText = AsyncHelpers.RunSync<string[]>(() => GenerateRequestStatusChangedEmailContent(db, log.RequestID, log.UserID, log.OldStatus, log.NewStatus));
                }
                catch 
                {
                    IList< Notification > notify = new List<Notification>();
                    return notify.AsEnumerable();
                }

                string body = emailText[0];
                string mybody = emailText[1];

                //user is not a requestUser and has subscribed to the general notification
                var recipients = (from s in db.UserEventSubscriptions
                                  join u in db.Users on s.UserID equals u.ID
                                  where (s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                                      (
                                      //Additional Check: if the user does not have the Request Status Changed event enabled at Organization, Project, or Project Organization level, then notification is only sent if the user is the one who submitted the request
                                            (
                                                (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                    || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                    || db.ProjectOrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                )
                                              &&
                                                (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                    && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                    && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                )
                                            )
                                           || (log.Request.SubmittedByID == s.UserID && s.FrequencyForMy == null)
                                        )
                                     //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                                     && (!db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                                        ||
                                        db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                                     )
                                     && ((!immediate && s.NextDueTime <= DateTime.UtcNow && (Frequencies)s.Frequency != Frequencies.Immediately) 
                                            || ( immediate && (Frequencies)s.Frequency == Frequencies.Immediately))
                                       
                                  )
                                  select new Recipient
                                        {
                                            Email = s.User.Email,
                                            Phone = s.User.Phone,
                                            Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                            UserID = s.UserID
                                        }).ToArray();

                //user is a request user and has subscribed to the "My" notification
                var requestUsersQuery = (from s in db.UserEventSubscriptions
                                         join u in db.Users on s.UserID equals u.ID
                                         where (s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && !s.User.Deleted && s.User.Active && s.FrequencyForMy != null &&
                                             (
                                                   //Additional Check: if the user does not have the Request Status Changed event enabled at Organization, Project, or Project Organization level, then notification is only sent if the user is the one who submitted the request
                                                   (
                                                       (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                           || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                           || db.ProjectOrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                                                       )
                                                     &&
                                                       (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                           && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                           && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                                                       )
                                                   )
                                                  || (log.Request.SubmittedByID == s.UserID && s.FrequencyForMy != null)
                                               )
                                            && db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                                            && ((!immediate && s.NextDueTimeForMy <= DateTime.UtcNow && (Frequencies)s.FrequencyForMy != Frequencies.Immediately) 
                                                || (immediate && (Frequencies)s.FrequencyForMy == Frequencies.Immediately))

                                         )
                                         select new Recipient
                                         {
                                             Email = s.User.Email,
                                             Phone = s.User.Phone,
                                             Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                             UserID = s.UserID
                                         }).ToArray();

                var userObservers = (from u in db.Users
                                     let requestObservers = db.RequestObservers.Where(obs => obs.RequestID == log.RequestID && obs.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestStatusChanged.ID) && (obs.UserID == u.ID || (obs.SecurityGroupID.HasValue && obs.SecurityGroup.Users.Any(sgu => sgu.UserID == u.ID))))
                                     where requestObservers.Any()
                                     select new Recipient
                                     {
                                         Email = u.Email,
                                         Phone = u.Phone,
                                         Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                         UserID = u.ID
                                     }).ToArray();

                var emailObservers = (from o in db.RequestObservers
                                      where o.EventSubscriptions.Any(e => e.EventID == EventIdentifiers.Request.RequestStatusChanged.ID)
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

                var notification = new Notification
                {
                    Subject = "Request Status Changed Notification",
                    Body = body,
                    Recipients = recipients
                };

                var myNotification = new Notification
                {
                    Subject = "Your Request Status Changed Notification",
                    Body = mybody,
                    Recipients = requestUsersQuery
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
            else if (typeof(T) == typeof(Audit.ResultsReminderLog))
            {
                var log = logItem as Audit.ResultsReminderLog;

                var q = (from r in db.Requests
                         join rdm in db.RequestDataMarts on r.ID equals rdm.RequestID
                         join p in db.Projects on rdm.Request.ProjectID equals p.ID into projects
                         from p in projects.DefaultIfEmpty()
                         join rt in db.RequestTypes on rdm.Request.RequestTypeID equals rt.ID into requestTypes
                         from rt in requestTypes.DefaultIfEmpty()
                         let network = db.Networks.Select(n => n.Name).FirstOrDefault()
                         where r.ID == log.RequestID
                         select new
                         {
                             RequestName = r.Name,
                             NetworkName = network,
                             RequestTypeName = rt.Name,
                             ProjectName = p.Name,
                             DataMartName = rdm.DataMart.Name
                         }).GroupBy(k => new { k.RequestName, k.NetworkName, k.RequestTypeName, k.ProjectName })
                        .Select(k => new
                        {
                            k.Key.RequestName,
                            k.Key.NetworkName,
                            k.Key.RequestTypeName,
                            k.Key.ProjectName,
                            DataMarts = k.OrderBy(v => v.DataMartName).Select(v => v.DataMartName)
                        });

                var details = q.FirstOrDefault();
                if(details == null)
                {
                    return Enumerable.Empty<Notification>();
                }

                var body = GenerateTimestampText(log) + 
                           "<p>Here are your most recent <b>Results Reminder</b> notifications from <b>" + details.NetworkName + "</b>.</p>" +
                           "<p>This is a reminder you have not reviewed all available results for <b>" + (details.RequestTypeName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + 
                           "</b> request <b>" + details.RequestName + "</b> from <b>" + string.Join(", ", details.DataMarts.ToArray()) +
                           "</b> in project <b>" + (details.ProjectName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")) + "</b>.</p>";

                var notification = new Notification
                {
                    Subject = "The request '" + log.Request.Name + "' (" + log.Request.Identifier + ") has unviewed results",
                    Body = body,
                    Recipients = (from s in db.UserEventSubscriptions
                                  let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.ResultsReminder.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.ResultsReminder.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                                  where s.EventID == EventIdentifiers.Request.ResultsReminder.ID && !s.User.Deleted && s.User.Active
                                  && !db.RequestDataMarts.Where(rdm => rdm.RequestID == log.RequestID).All(dm => db.LogsResponseViewed.Any(rv => rv.UserID == s.UserID && rv.Response.RequestDataMart.RequestID == log.RequestID && rv.Response.RequestDataMart.DataMartID == dm.ID))
                                  && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                  && (
                                      (orgAcls.Any() || projectAcls.Any())
                                      &&
                                      (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed))
                                 )
                                  from r in db.FilteredRequestListForEvent(s.UserID, null)
                                  where r.ID == log.RequestID
                                  select new { s, r }).Where(sub => sub.r != null).Select(sub =>
                                        new Recipient
                                        {
                                            Email = sub.s.User.Email,
                                            Phone = sub.s.User.Phone,
                                            Name = ((sub.s.User.FirstName + " " + sub.s.User.MiddleName).Trim() + " " + sub.s.User.LastName).Trim(),
                                            UserID = sub.s.UserID
                                        }).Union(
                            from s in db.UserEventSubscriptions
                            join u in db.Users on s.UserID equals u.ID
                            let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                            let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                            let projectOrgAcls = db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                            where s.UserID == log.UserID && s.EventID == EventIdentifiers.Response.ResultsViewed.ID && !s.User.Deleted && s.User.Active
                                        && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                        && (
                                                (orgAcls.Any() || projectAcls.Any() || projectOrgAcls.Any())
                                                &&
                                                (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectOrgAcls.All(a => a.Allowed))
                                           )
                            select new Recipient
                            {
                                Email = u.Email,
                                Phone = u.Phone,
                                Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                UserID = u.ID
                            }
                        ).ToArray()
                };
                IList<Notification> notifies = new List<Notification>();
                notifies.Add(notification);

                return notifies.AsEnumerable();
            }
            else if (typeof(T) == typeof(Audit.RequestMetadataChangeLog))
            {
                var log = logItem as Audit.RequestMetadataChangeLog;

                var q = from r in db.Requests
                        join rt in db.RequestTypes on r.RequestTypeID equals rt.ID into requestTypes
                        from rt in requestTypes.DefaultIfEmpty()
                        join p in db.Projects on r.ProjectID equals p.ID into projects
                        from p in projects.DefaultIfEmpty()
                        join u in db.Users on log.UserID equals u.ID into users
                        from u in users
                        let network = db.Networks.Select(n => n.Name).FirstOrDefault()
                        where r.ID == log.RequestID
                        select new {
                            RequestName = r.Name,
                            MSRequestID = r.MSRequestID,
                            RequestTypeName = rt.Name,
                            ProjectName = p.Name,
                            User = u,
                            NetworkName = network
                        };

                var details = q.FirstOrDefault();
                if (details == null)
                {
                    return Enumerable.Empty<Notification>();
                }

                StringBuilder body = new StringBuilder(GenerateTimestampText(log));
                body.AppendFormat("<p>Here are your most recent My <b>Request Metadata Changed</b> notifications from <b>{0}</b>.</p>", details.NetworkName);
                body.AppendFormat("<p>The following metadata fields of your <b>{0}</b> request <b>{1}</b> ", (details.RequestTypeName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")), details.RequestName);
                if (!string.IsNullOrWhiteSpace(details.MSRequestID))
                {
                    body.AppendFormat("(ID <b>{0}</b>) ", details.MSRequestID);
                }
                body.AppendFormat("in the <b>{0}</b> project were modified.</p>", (details.ProjectName ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")));

                var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings{
                    Formatting = Newtonsoft.Json.Formatting.None,
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter{ DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });

                IEnumerable<PropertyChangeDetailDTO> changedProperties = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PropertyChangeDetailDTO>>(log.ChangeDetail, serializerSettings);


                body.Append("<table cellpadding=\"3\" cellspacing=\"0\" style=\"width:90%;text-align:left;\"><colgroup><col style=\"width:20%\" /><col style=\"width:40%\" /><col style=\"width:40%\" /></colgroup><thead><tr><th style=\"border-bottom:2px solid #ddd;\"><b>MetaData Field</b></th><th style=\"border-bottom:2px solid #ddd;\"><b>Old Value</b></th><th style=\"border-bottom:2px solid #ddd;\"><b>New Value</b></strong></th></tr></thead><tbody>");
                foreach (var propChange in changedProperties)
                {
                    body.AppendFormat("<tr><td style=\"border-top:1px solid #ddd;border-right:1px solid #ddd;\">{0}</td><td style=\"border-top:1px solid #ddd;border-right:1px solid #eee;\">{1}</td><td style=\"border-top:1px solid #ddd;\">{2}</td></tr>", propChange.PropertyDisplayName, string.IsNullOrWhiteSpace(propChange.OriginalValueDisplay) ? "&nbsp;" : propChange.OriginalValueDisplay, string.IsNullOrWhiteSpace(propChange.NewValueDisplay) ? "&nbsp;" : propChange.NewValueDisplay);
                }
                body.Append("</tbody></table>");                
                
                var query = from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.MetadataChange.ID, immediate)
                            join u in db.Users on s.UserID equals u.ID
                            let projectEventAcls = db.ProjectEvents.Where(e => e.EventID == EventIdentifiers.Request.MetadataChange.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            let organizationEventAcls = db.OrganizationEvents.Where(e => e.EventID == EventIdentifiers.Request.MetadataChange.ID && e.Organization.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.MetadataChange.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.RequestID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                            where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                             && (
                                  (projectEventAcls.Any() || organizationEventAcls.Any() || projectDataMartEventAcls.Any())
                                  &&
                                  (projectEventAcls.All(a => a.Allowed) && organizationEventAcls.All(a => a.Allowed) && projectDataMartEventAcls.All(a => a.Allowed))
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
                    Subject = "Your Request Metadata Changed Notification",
                    Body = body.ToString(),
                    Recipients = recipients
                };

                return new List<Notification>{ notification };
            }

            throw new ArgumentOutOfRangeException("A notification cannot be created for the type " + typeof(T).FullName + " using the Request Logging Configuration");

        }

        public override async Task<IEnumerable<Notification>> GenerateNotificationsFromLogs(DataContext db)
        {

            //Submitted Request Needs approval nag
            var results = await (from r in db.Requests
                                 where r.Status == RequestStatuses.AwaitingResponseApproval &&
                                     (
                                     (db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).Any() &&
                                     db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).All(a => a.Allowed))
                                     ||
                                     (db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(req => req.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).Any() &&
                                     db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(requests => requests.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).All(a => a.Allowed))
                                     ||
                                     (db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).Any() &&
                                     db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ApproveRejectSubmission && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID select s).Any()).All(a => a.Allowed))
                                     )
                                 select r).ToArrayAsync();


            var notifications = new List<Notification>();
            foreach (var request in results)
            {
                var notification = CreateNotifications(new Audit.SubmittedRequestNeedsApprovalLog
                {
                    Description = string.Format("Request '{0}' needs approval in order to be processed", request.Name),
                    Request = request,
                    RequestID = request.ID,
                    TimeStamp = DateTime.UtcNow,
                    UserID = request.SubmittedByID ?? Guid.Empty
                }, db, false);

                if (notification != null)
                {
                    foreach (Notification notify in notification)
                    {
                        notifications.Add(notify);
                    }
                }
            }

            //Awaiting response nag
            results = await (from r in db.Requests
                             where (r.Status == RequestStatuses.Submitted || r.Status == RequestStatuses.Resubmitted) &&
                                 (
                                 (db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                 db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed))
                                 ||
                                 (db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(req => req.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                 db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(requests => requests.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed)
                                 )
                                 ||
                                 (db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                 db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed)
                                 )
                                 )
                             select r).Include("DataMarts").ToArrayAsync();


            foreach (var request in results)
            {
                foreach (var dataMart in request.DataMarts)
                {
                    var dmName = db.DataMarts.Find(dataMart.DataMartID).Name;
                    var notification = CreateNotifications(new Audit.SubmittedRequestAwaitsResponseLog
                    {
                        Description = string.Format("{0} has not responded to {1}. ", dmName, request.Name),
                        Request = request,
                        RequestID = request.ID,
                        TimeStamp = DateTime.UtcNow,
                        UserID = request.SubmittedByID ?? Guid.Empty
                    }, db, false);
                    if (notification != null)
                    {
                        foreach (Notification notify in notification)
                        {
                            notifications.Add(notify);
                        }
                    }
                }
            }

            //Results awaiting viewing nag
            results = await (from r in db.Requests
                             where (r.Status == RequestStatuses.Complete || r.Status == RequestStatuses.ExaminedByInvestigator || r.Status == RequestStatuses.PartiallyComplete || r.Status == RequestStatuses.Submitted) && r.DataMarts.Any(dm => dm.Responses.Any()) &&
                                 (
                                 (db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                 db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed))
                                 ||
                                 (db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(req => req.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                 db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(requests => requests.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed)
                                 )
                                 ||
                                 (db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                 db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed)
                                 )
                                 ||
                                 (db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.OrganizationID == r.OrganizationID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                 db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.OrganizationID == r.OrganizationID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed))
                                 )
                             select r).Include("DataMarts").ToArrayAsync();


            foreach (var request in results)
            {
                foreach (var dataMart in request.DataMarts)
                {
                    var dmName = db.DataMarts.Find(dataMart.DataMartID).Name;
                    var resultsReminderLog = new Audit.ResultsReminderLog
                    {
                        Description = string.Format("Results from {0} for request {1} have not been viewed. ", dmName, request.Name),
                        Request = request,
                        RequestID = request.ID,
                        TimeStamp = DateTime.UtcNow,
                        UserID = Guid.Empty
                    };

                    db.LogsResultsReminder.Add(resultsReminderLog);

                    var notification = CreateNotifications(resultsReminderLog, db, false);
                    if (notification != null && notification.Any())
                    {
                        notifications.AddRange(notification);
                    }
                }
            }


            //get only the new request submitted logs for ones generated at the request level (ie datamart not specified on the log item)
            var logs = await FilterAuditLog(from l in db.LogsNewRequestSubmitted.Include(x => x.Request).Where(l => l.RequestDataMartID == null) select l, db.UserEventSubscriptions, EventIdentifiers.Request.NewRequestSubmitted.ID).GroupBy(g => new { g.RequestID, g.UserID }).ToArrayAsync();

            foreach (var log in logs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            var statusChangedlogs = await FilterAuditLog(from l in db.LogsRequestStatusChanged.Include(x => x.Request) select l, db.UserEventSubscriptions, EventIdentifiers.Request.RequestStatusChanged.ID).GroupBy(g => new { g.RequestID, g.UserID }).ToArrayAsync();

            foreach (var log in statusChangedlogs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            var metadataChangedLogs = await FilterAuditLog(from l in db.LogsRequestMetadataChange.Include(x => x.Request) select l, db.UserEventSubscriptions, EventIdentifiers.Request.MetadataChange.ID).GroupBy(g => new { g.RequestID, g.UserID }).ToArrayAsync();
            foreach (var log in metadataChangedLogs)
            {
                var notification = CreateNotifications(log.First(), db, false);
                if (notification != null && notification.Any())
                    notifications.AddRange(notification);
            }

            return notifications.AsEnumerable();

        }

        public IEnumerable<AuditLog> GenerateRequestStatusEvents(DataContext db, ApiIdentity identity, bool read, RequestStatuses oldStatus, RequestStatuses newStatus, Guid requestID, string myEmailBody, string emailBody, string subject)
        {
            var details = (from r in db.Requests
                           where r.ID == requestID
                           select new
                           {
                               RequestName = r.Name,
                               RequestIdentifier = r.Identifier,
                               OldStatus = oldStatus,
                               NewStatus = newStatus
                           }).FirstOrDefault();

            var logs = new List<AuditLog>();
            logs.Add(
                db.LogsRequestStatusChanged.Add(
                    new Audit.RequestStatusChangedLog
                    {
                        RequestID = requestID,
                        Description = string.Format("Status of request {0} (ID {1}) was changed from {2} to {3}", details.RequestName, details.RequestIdentifier, details.OldStatus, details.NewStatus),
                        Reason = EntityState.Modified,
                        OldStatus = oldStatus,
                        NewStatus = newStatus,
                        UserID = identity == null ? Guid.Empty : identity.ID,
                        MyEmailBody = myEmailBody,
                        EmailBody = emailBody,
                        Subject = subject
                    }
                )
            );
            return logs;
        }

        /// <summary>
        /// Generates the request status email text for general and My emails, the first string will be for general, the second for MY.
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GenerateRequestStatusChangedEmailContent(DataContext db, Guid requestID, Guid userID, RequestStatuses originalStatus, RequestStatuses newStatus )
        {
            var details = await (from r in db.Requests
                                 let network = db.Networks.FirstOrDefault()
                                 let user = db.Users.Where(u => u.ID == userID).FirstOrDefault()
                                 join p in db.Projects on r.ProjectID equals p.ID into projects
                                 from p in projects.DefaultIfEmpty()
                                 join rt in db.RequestTypes on r.RequestTypeID equals rt.ID into requestTypes
                                 from rt in requestTypes.DefaultIfEmpty()
                                 where r.ID == requestID
                                 select new
                                 {
                                     RequestName = r.Name,
                                     RequestIdentifier = r.Identifier,
                                     NetworkName = network.Name,
                                     RequestType = rt.Name,
                                     Project = p.Name ?? "<<Not Found>>",
                                     Username = user.UserName
                                 }).FirstOrDefaultAsync();

            string currentStatusText = Lpp.Utilities.ObjectEx.ToString(originalStatus, true);
            string newStatusText = Lpp.Utilities.ObjectEx.ToString(newStatus, true);

            string emailBody = string.Format(RequestLogConfiguration.RequestStatusEmailTemplate,
                TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")),
                details.NetworkName,
                details.RequestType,
                details.RequestName,
                details.RequestIdentifier,
                currentStatusText,
                newStatusText,
                details.Username,
               (details.Project ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")));

            string myEmailBody = string.Format(RequestLogConfiguration.RequestStatusEmailTemplate,
                TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")),
                details.NetworkName,
                details.RequestType,
                details.RequestName,
                details.RequestIdentifier,
                currentStatusText,
                newStatusText,
                details.Username,
                (details.Project ?? System.Web.HttpUtility.HtmlEncode("<<Not Found>>")));

            return new string[] { emailBody, myEmailBody };
        }

        public void SendNotification(IEnumerable<Notification> notifications)
        {
            using (var smtp = new SmtpClient())
            {
                foreach (var notification in notifications)
                {
                    foreach (var recipient in notification.Recipients.Where(d => !string.IsNullOrWhiteSpace(d.Email)).DistinctBy(d => d.Email))
                    {
                        using (var msg = new MailMessage())
                        {
                            var salutation = "Dear " + recipient.Name + ",\r\n<br/><br/>";
                            var postscript = "\r\n<br/><br/><p style=\"font-size: 0.8em;\">We are notifying you of this change because you have subscribed to this notification. If you do not wish to receive this notification again, please login and change your subscription settings in your profile.</p>";
                            msg.To.Add(new MailAddress(recipient.Email, recipient.Name));
                            msg.Subject = notification.Subject;
                            msg.Body = salutation + notification.Body + postscript;
                            msg.IsBodyHtml = true;

                            try
                            {
                                smtp.Send(msg);
                            }
                            catch (SmtpFailedRecipientsException e)
                            {
                                System.Diagnostics.Debug.WriteLine(e);
                                //Do nothing right now, should batch up all of the failed recipients and then send a note to the administrator saying that they're invalid.
                            }
                            catch (SmtpException se)
                            {
                                //Record exception in log file.
                                //log.Error("There was an error sending the notification: '" + notification.Subject + "' to '" + recipient.Email + "'.", se);
                            }
                        }
                    }
                }
            }

        }

        public async Task SendNotificationAsync(IEnumerable<Notification> notifications)
        {
            using (var smtp = new SmtpClient())
            {
                foreach (var notification in notifications)
                {
                    foreach (var recipient in notification.Recipients.Where(d => !string.IsNullOrWhiteSpace(d.Email)).DistinctBy(d => d.Email))
                    {
                        using (var msg = new MailMessage())
                        {
                            var salutation = "Dear " + recipient.Name + ",\r\n<br/><br/>";
                            var postscript = "\r\n<br/><br/><p style=\"font-size: 0.8em;\">We are notifying you of this change because you have subscribed to this notification. If you do not wish to receive this notification again, please login and change your subscription settings in your profile.</p>";
                            msg.To.Add(new MailAddress(recipient.Email, recipient.Name));
                            msg.Subject = notification.Subject;
                            msg.Body = salutation + notification.Body + postscript;
                            msg.IsBodyHtml = true;

                            try
                            {
                                await smtp.SendMailAsync(msg);
                            }
                            catch (SmtpFailedRecipientsException e)
                            {
                                System.Diagnostics.Debug.WriteLine(e);
                                //Do nothing right now, should batch up all of the failed recipients and then send a note to the administrator saying that they're invalid.
                            }
                            catch (SmtpException se)
                            {
                                //Record exception in log file.
                                //log.Error("There was an error sending the notification: '" + notification.Subject + "' to '" + recipient.Email + "'.", se);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The email template for general request status change email.
        /// </summary>
        public const string RequestStatusEmailTemplate = "<p>{0:G} EST<br/></p><p>Here are your most recent <b>Request Status Changed</b> notifications from <b>{1}</b>. </p><p>The status of <b>{2}</b> request <b>{3}</b> (ID <b>{4}</b>) was changed from <b>{5}</b> to <b>{6}</b> by <b>{7}</b> in project <b>{8}</b>.</p>";
        /// <summary>
        /// The email template for 'My' request status change email.
        /// </summary>
        public const string MyRequestStatusEmailTemplate = "<p>{0:G} EST<br/></p><p>Here are your most recent <b>My Request Status Changed</b> notifications from <b>{1}</b>. </p><p>The status of your <b>{2}</b> request <b>{3}</b> (ID <b>{4}</b>) was changed from <b>{5}</b> to <b>{6}</b> by <b>{7}</b> in project <b>{8}</b>.</p>";





    }
}
