using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using PopMedNet.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.Data.Requests
{
    public class FilteredRequestListItem : EntityWithID, IEntityWithName, IEntityWithDeleted
    {
        public FilteredRequestListItem()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long Identifier { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? MSRequestID { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? AdditionalInstructions { get; set; }

        public bool MirrorBudgetFields { get; set; } = false;

        public bool Scheduled { get; set; }

        public bool Template { get; set; }

        public bool Deleted { get; set; } = false;

        public Priorities Priority { get; set; } = Priorities.Medium;

        public Guid OrganizationID { get; set; }
        //public virtual Organization? Organization { get; set; }

        public string? PurposeOfUse { get; set; }

        public string? PhiDisclosureLevel { get; set; }

        public string? Schedule { get; set; }

        public int ScheduleCount { get; set; }

        public Guid ProjectID { get; set; }
        //public virtual Project Project { get; set; }

        public Guid RequestTypeID { get; set; }
        //public virtual RequestType? RequestType { get; set; }

        public string? AdapterPackageVersion { get; set; }

        public string? IRBApprovalNo { get; set; }

        public DateTime? DueDate { get; set; }

        public string? ActivityDescription { get; set; }

        public Guid? RequesterCenterID { get; set; }
        //public virtual RequesterCenter? RequesterCenter { get; set; }
        public Guid? WorkplanTypeID { get; set; }
        //public virtual WorkplanType? WorkplanType { get; set; }

        public Guid? ReportAggregationLevelID { get; set; }
        //public virtual ReportAggregationLevel? ReportAggregationLevel { get; set; }

        public Guid? ActivityID { get; set; }
        //public virtual Activity? Activity { get; set; }

        public Guid? SourceActivityID { get; set; }
        //public virtual Activity? SourceActivity { get; set; }
        public Guid? SourceActivityProjectID { get; set; }
        //public virtual Activity? SourceActivityProject { get; set; }
        public Guid? SourceTaskOrderID { get; set; }
        //public virtual Activity? SourceTaskOrder { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid CreatedByID { get; set; }
        //public virtual User CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public Guid UpdatedByID { get; set; }
        //public virtual User? UpdatedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }
        public Guid? SubmittedByID { get; set; }
        //public virtual User? SubmittedBy { get; set; }

        public DateTime? ApprovedForDraftOn { get; set; }
        public Guid? ApprovedForDraftByID { get; set; }
        //public virtual User? ApprovedForDraftBy { get; set; }

        public DateTime? RejectedOn { get; set; }
        public Guid? RejectedByID { get; set; }
        //public virtual User? RejectedBy { get; set; }

        public DateTime? CancelledOn { get; set; }
        public Guid? CancelledByID { get; set; }
        //public virtual User? CancelledBy { get; set; }

        public DateTimeOffset? CompletedOn { get; set; }
        public string? UserIdentifier { get; set; }
        public Guid? WorkflowID { get; set; }
        //public virtual Workflow? Workflow { get; set; }
        public Guid? WorkFlowActivityID { get; set; }
        //public virtual WorkflowActivity? WorkflowActivity { get; set; }

        public bool Private { get; set; } = true;

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public RequestStatuses Status { get; set; }

        public string? Query { get; set; }

        public virtual Guid? ParentRequestID { get; set; }
        //public virtual Request? ParentRequest { get; set; }
    }

    internal class FilteredRequestListItemConfiguration : IEntityTypeConfiguration<FilteredRequestListItem>
    {
        public void Configure(EntityTypeBuilder<FilteredRequestListItem> builder)
        {
            builder.Property(e => e.Priority).HasConversion<byte>();
            builder.Property(e => e.Status).HasConversion<int>();
        }
    }

    internal class FilteredRequestListItemMappingProfile : AutoMapper.Profile
    {
        public FilteredRequestListItemMappingProfile()
        {
            CreateMap<FilteredRequestListItem, Request>();
        }
    }
}
