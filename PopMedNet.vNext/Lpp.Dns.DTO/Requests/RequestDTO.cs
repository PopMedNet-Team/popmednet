using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Lpp.Objects;
using Lpp.Dns.DTO.Enums;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request
    /// </summary>
    [DataContract]
    public class RequestDTO : EntityDtoWithID
    {
        /// <summary>
        /// Request DTO
        /// </summary>
        public RequestDTO() { }
        /// <summary>
        /// Returns identifier
        /// </summary>

        [DataMember, ReadOnly(true)]
        public long Identifier { get; set; }

        [DataMember]
        public string MSRequestID { get; set; }

        /// <summary>
        /// Gets or sets the ID of projct
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Project
        /// </summary>
        [DataMember]
        public string Project { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Additional instructions
        /// </summary>
        [DataMember]
        public string AdditionalInstructions { get; set; }
        /// <summary>
        /// The date the request was updated on
        /// </summary>
        [DataMember]
        public DateTimeOffset UpdatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ID of user who updated the request
        /// </summary>
        [DataMember]
        public Guid UpdatedByID { get; set; }
        /// <summary>
        /// Updated By
        /// </summary>
        [DataMember]
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if source/budget box is Checked
        /// </summary>
        [DataMember]
        public bool MirrorBudgetFields { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request is scheduled
        /// </summary>
        [DataMember]
        public bool Scheduled { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if template is selected
        /// </summary>
        [DataMember]
        public bool Template { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if request is deleted
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the priorities
        /// </summary>
        [DataMember]
        public Priorities Priority { get; set; }
        /// <summary>
        /// Gets or sets the ID of rganization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; }
        /// <summary>
        /// Purpose of use
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string PurposeOfUse { get; set; }
        /// <summary>
        /// PHI Disclosure level
        /// </summary>
        [DataMember]
        public string PhiDisclosureLevel { get; set; }
        ///<summary>
        ///Gets or sets the ID of Report Aggregation Level
        /// </summary>
        [DataMember]
        public Guid? ReportAggregationLevelID { get; set; }
        ///<summary>
        /// Report Aggregation Level
        /// </summary>
        [DataMember]
        public string ReportAggregationLevel { get; set; }
        /// <summary>
        /// Schedule
        /// </summary>
        [DataMember]
        public string Schedule { get; set; }
        /// <summary>
        /// Gets or sets the schedule count
        /// </summary>
        [DataMember]
        public int ScheduleCount { get; set; }
        /// <summary>
        /// Th date the request was submitted on
        /// </summary>
        [DataMember]
        public DateTimeOffset? SubmittedOn { get; set; }
        /// <summary>
        /// The ID of the user who submitted the request
        /// </summary>
        [DataMember]
        public Guid? SubmittedByID { get; set;}
        /// <summary>
        /// Submitted By Name
        /// </summary>
        [DataMember]
        public string SubmittedByName { get; set; }
        /// <summary>
        /// Submitted By
        /// </summary>
        [DataMember]
        public string SubmittedBy { get; set; }
        /// <summary>
        /// Gets or sets the ID of Request Type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request type
        /// </summary>
        [DataMember]
        public string RequestType { get; set;}
        /// <summary>
        /// Adapter Package version
        /// </summary>
        [DataMember]
        public string AdapterPackageVersion { get; set; }
        /// <summary>
        /// IRB Approval No
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string IRBApprovalNo { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        [DataMember]
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Activity dscription
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string ActivityDescription { get; set; }
        /// <summary>
        /// Gets or sets the ID of Activity
        /// </summary>
        [DataMember]
        public Guid? ActivityID { get; set; }
        ///<summary>
        ///Gets or sets the ID of Source Activity
        /// </summary>
        [DataMember]
        public Guid? SourceActivityID { get; set; }
        ///<summary>
        /// Gets or sets the ID of Source Activity Project
        /// </summary>
        [DataMember]
        public Guid? SourceActivityProjectID { get; set; }
        ///<summary>
        /// Gets or sets the ID of Source Task Order
        /// </summary>
        [DataMember]
        public Guid? SourceTaskOrderID { get; set; }
        /// <summary>
        /// Gets or sets the ID of Requester Center
        /// </summary>        
        [DataMember]
        public Guid? RequesterCenterID { get; set; }
        /// <summary>
        /// Requester Center
        /// </summary>
        [DataMember]
        public string RequesterCenter { get; set; }
        /// <summary>
        /// Gets or sets the ID of workplan type
        /// </summary>
        [DataMember]
        public Guid? WorkplanTypeID { get; set; }
        /// <summary>
        /// Workplan Type
        /// </summary>
        [DataMember]
        public string WorkplanType { get; set; }
        /// <summary>
        /// Gets or sets the ID of workflow
        /// </summary>
        [DataMember]
        public Guid? WorkflowID { get; set; }
        /// <summary>
        /// Workflow
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Workflow { get; set; }
        /// <summary>
        /// Gets or sets the ID of current workflow activity
        /// </summary>
        [DataMember]
        public Guid? CurrentWorkFlowActivityID { get; set; }
        /// <summary>
        /// Current workflow activity
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string CurrentWorkFlowActivity { get; set; }
        /// <summary>
        /// Gets or set the request status
        /// </summary>
         [DataMember, ReadOnly(true)]
        public RequestStatuses Status { get; set; }
        /// <summary>
        /// Status Text
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string StatusText { get; set; }
        /// <summary>
        /// The Date the major event date was submitted
        /// </summary>
        [DataMember, ReadOnly(true)]
        public DateTimeOffset MajorEventDate { get; set; }
        /// <summary>
        /// ID of major event
        /// </summary>
        [DataMember, ReadOnly(true)]
        public Guid MajorEventByID { get; set; }
        /// <summary>
        /// Major EventBy
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string MajorEventBy { get; set; }
        /// <summary>
        /// The date the request was created on
        /// </summary>
        [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ID of createdby
        /// </summary>
        [DataMember]
        public Guid CreatedByID { get; set; }
        /// <summary>
        /// Created By
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }
        /// <summary>
        /// Th date the request was completed on
        /// </summary>
        [DataMember]
        public DateTimeOffset? CompletedOn { get; set; }
        /// <summary>
        /// The date the request was canceled on
        /// </summary>
        [DataMember]
        public DateTimeOffset? CancelledOn { get; set; }
        /// <summary>
        /// User identifier
        /// </summary>
        [DataMember, MaxLength(100)]
        public string UserIdentifier { get; set; }
        /// <summary>
        /// Query
        /// </summary>
        [DataMember]
        public string Query { get; set; }
        /// <summary>
        /// Gets or sets the ID of parent request
        /// </summary>
        [DataMember]
        public Guid? ParentRequestID { get; set; }
    }
}
