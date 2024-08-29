using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Objects;
using PopMedNet.Workflow.Engine.Interfaces;
using PopMedNet.Dns.DTO.Enums;
using LinqKit;
using PopMedNet.Dns.DTO.Security;
using System.Linq.Expressions;

namespace PopMedNet.Dns.Data
{
    [Table("Requests")]
    public class Request : EntityWithID, IEntityWithName, IEntityWithDeleted, IWorkflowEntity
    {

        public Request()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long Identifier { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? MSRequestID { get; set; }

        [MaxLength]
        public string Description { get; set; } = string.Empty;

        public string? AdditionalInstructions { get; set; }

        public bool MirrorBudgetFields { get; set; } = false;

        public bool Scheduled { get; set; }

        public bool Template { get; set; }

        public bool Deleted { get; set; } = false;

        public Priorities Priority { get; set; } = Priorities.Medium;

        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }

        [MaxLength(100)]
        public string? PurposeOfUse { get; set; }

        public string? PhiDisclosureLevel { get; set; }

        public string? Schedule { get; set; }

        public int ScheduleCount { get; set; }

        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }

        [MaxLength(20)]
        public string? AdapterPackageVersion { get; set; }

        [MaxLength(100)]
        public string? IRBApprovalNo { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(255)]
        public string? ActivityDescription { get; set; }

        public Guid? RequesterCenterID { get; set; }
        public virtual RequesterCenter? RequesterCenter { get; set; }
        public Guid? WorkplanTypeID { get; set; }
        public virtual WorkplanType? WorkplanType { get; set; }

        public Guid? ReportAggregationLevelID { get; set; }
        public virtual ReportAggregationLevel? ReportAggregationLevel { get; set; }

        public Guid? ActivityID { get; set; }
        public virtual Activity? Activity { get; set; }

        public Guid? SourceActivityID { get; set; }
        public virtual Activity? SourceActivity { get; set; }
        public Guid? SourceActivityProjectID { get; set; }
        public virtual Activity? SourceActivityProject { get; set; }
        public Guid? SourceTaskOrderID { get; set; }
        public virtual Activity? SourceTaskOrder { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid CreatedByID { get; set; }
        public virtual User CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public Guid UpdatedByID { get; set; }
        public virtual User? UpdatedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }
        public Guid? SubmittedByID { get; set; }
        public virtual User? SubmittedBy { get; set; }

        public DateTime? ApprovedForDraftOn { get; set; }
        public Guid? ApprovedForDraftByID { get; set; }
        public virtual User? ApprovedForDraftBy { get; set; }

        public DateTime? RejectedOn { get; set; }
        public Guid? RejectedByID { get; set; }
        public virtual User? RejectedBy { get; set; }

        public DateTime? CancelledOn { get; set; }
        public Guid? CancelledByID { get; set; }
        public virtual User? CancelledBy { get; set; }

        public DateTimeOffset? CompletedOn { get; set; }
        [MaxLength(100)]
        public string? UserIdentifier { get; set; }
        public Guid? WorkflowID { get; set; }
        public virtual Workflow? Workflow { get; set; }
        public Guid? WorkFlowActivityID { get; set; }
        public virtual WorkflowActivity? WorkflowActivity { get; set; }

        public bool Private { get; set; } = true;

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public RequestStatuses Status { get; set; }

        [MaxLength]
        public string? Query { get; set; }

        public virtual Guid? ParentRequestID { get; set; }
        public virtual Request? ParentRequest { get; set; }

        public virtual RequestStatistics Statistics { get; set; }

        public virtual ICollection<RequestDataMart> DataMarts { get; set; } = new HashSet<RequestDataMart>();
        public virtual ICollection<RequestSharedFolderRequest> Folders { get; set; } = new HashSet<RequestSharedFolderRequest>();
        public virtual ICollection<AclRequest> RequestAcls { get; set; } = new HashSet<AclRequest>();
        public virtual ICollection<RequestSearchTerm> SearchTerms { get; set; } = new HashSet<RequestSearchTerm>();
        public virtual ICollection<Audit.SubmittedRequestNeedsApprovalLog> SubmittedRequestNeedsApprovalLogs { get; set; } = new HashSet<Audit.SubmittedRequestNeedsApprovalLog>();
        public virtual ICollection<Audit.SubmittedRequestAwaitsResponseLog> SubmittedRequestAwaitsResponseLogs { get; set; } = new HashSet<Audit.SubmittedRequestAwaitsResponseLog>();
        public virtual ICollection<Audit.NewRequestSubmittedLog> NewRequestSubmittedLogs { get; set; } = new HashSet<Audit.NewRequestSubmittedLog>();
        public virtual ICollection<Audit.RequestStatusChangedLog> RequestStatusChangeLogs { get; set; } = new HashSet<Audit.RequestStatusChangedLog>();
        public virtual ICollection<Audit.ResultsReminderLog> ResultsReminderLogs { get; set; } = new HashSet<Audit.ResultsReminderLog>();
        public virtual ICollection<Audit.RequestMetadataChangeLog> MetadataChangeLogs { get; set; } = new HashSet<Audit.RequestMetadataChangeLog>();
        public virtual ICollection<RequestUser> Users { get; set; } = new HashSet<RequestUser>();
        public virtual ICollection<RequestObserver> RequestObservers { get; set; } = new HashSet<RequestObserver>();


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

    internal class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasIndex(r => r.Status, "IX_Status").IsUnique(false).IsClustered(false);
            builder.Property(r => r.Deleted).HasColumnName("isDeleted");
            builder.Property(r => r.Scheduled).HasColumnName("isScheduled");
            builder.Property(r => r.Template).HasColumnName("isTemplate");

            builder.HasMany(t => t.DataMarts)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Folders)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequestAcls)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.SearchTerms)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.SubmittedRequestNeedsApprovalLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.SubmittedRequestAwaitsResponseLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.NewRequestSubmittedLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequestStatusChangeLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ResultsReminderLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Users)
                .WithOne(t => t.Request)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequestObservers)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.MetadataChangeLogs)
                .WithOne(t => t.Request)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Statistics)
                .WithOne(s => s.Request)
                .HasForeignKey<RequestStatistics>(s => s.RequestID);

            builder.Property(e => e.Priority).HasConversion<byte>();
            builder.Property(e => e.Status).HasConversion<int>();
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
                //return db.FilteredRequestList(identity.ID);
                return query.Join(db.FilteredRequestList(identity.ID), r => r.ID, rr => rr.ID, (r, k) => r);
            }
            else
            {
                var predicate = PredicateBuilder.New<Request>(false);

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

    public class RequestMappingProfile : AutoMapper.Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<Request, DTO.RequestDTO>()
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.FirstName + " " + src.CreatedBy.LastName))
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.Organization.Name))
                .ForMember(d => d.MajorEventDate, opt => opt.MapFrom(src => src.CancelledOn ?? src.RejectedOn ?? src.SubmittedOn ?? src.CreatedOn))
                .ForMember(d => d.MajorEventByID, opt => opt.MapFrom(src => src.CancelledByID ?? src.RejectedByID ?? src.SubmittedByID ?? src.CreatedByID))
                .ForMember(d => d.MajorEventBy, opt => opt.MapFrom(src => src.CancelledByID.HasValue ? src.CancelledBy.UserName : src.RejectedByID.HasValue ? src.RejectedBy.UserName : src.SubmittedByID.HasValue ? src.SubmittedBy.UserName : src.CreatedBy.UserName))
                .ForMember(d => d.Project, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(d => d.RequesterCenter, opt => opt.MapFrom(src => src.RequesterCenter.Name))
                .ForMember(d => d.RequestType, opt => opt.MapFrom(src => src.RequestType.Name))
                .ForMember(d => d.ReportAggregationLevel, opt => opt.MapFrom(src => src.ReportAggregationLevel.Name))
                .ForMember(d => d.StatusText, opt => opt.MapFrom(StatusText()))
                .ForMember(d => d.SubmittedBy, opt => opt.MapFrom(src => src.SubmittedByID.HasValue ? src.SubmittedBy.UserName : null))
                .ForMember(d => d.SubmittedByName, opt => opt.MapFrom(src => src.SubmittedByID.HasValue ? src.SubmittedBy.FirstName + " " + src.SubmittedBy.LastName : null))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.UserName))
                .ForMember(d => d.WorkplanType, opt => opt.MapFrom(src => src.WorkplanType.Name))
                .ForMember(d => d.Workflow, opt => opt.MapFrom(src => src.Workflow.Name))
                .ForMember(d => d.CurrentWorkFlowActivity, opt => opt.MapFrom(src => src.WorkflowActivity.Name));

            CreateMap<DateTime, DateTimeOffset>().ConvertUsing(d => new DateTimeOffset(d));
            CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(d => d!.Value);
        }

        Expression<Func<Request, string>> StatusText()
        {
            return (r) => r.Status == RequestStatuses.AwaitingRequestApproval ? "Awaiting Request Approval" :
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
                                "Unknown";
        }
    }
}
