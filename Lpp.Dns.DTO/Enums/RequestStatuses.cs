using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    //Note this has large gaps to allow for sub statuses and additions as necessary.
    //When adding to this, make sure you leave room for additional items later

    //Note 2: When you update these statuses you must update the trigger on RequestDataMarts to know how to handle the new status.
    /// <summary>
    /// Types of Request status
    /// </summary>
    [DataContract]
    public enum RequestStatuses
    {
        /// <summary>
        /// Indicates Request Status is 3rd party Submitted Draft
        /// </summary>
        [EnumMember, Description("3rd Party Submitted Draft")]
        ThirdPartySubmittedDraft = 100,
        /// <summary>
        /// Indicates Request Status is Draft
        /// </summary>
        [EnumMember]
        Draft = 200,
        /// <summary>
        /// Indicates Request Status is Draft Review
        /// </summary>
        [EnumMember, Description("Draft Pending Review")]
        DraftReview = 250,
        /// <summary>
        /// Indicates Request Status is Awaiting Request Approval
        /// </summary>
        [EnumMember, Description("Awaiting Request Approval")]
        AwaitingRequestApproval = 300,
        /// <summary>
        /// Indicates Request Status is Pending Working Specifications
        /// </summary>
        [EnumMember, Description("Pending Working Specifications")]
        PendingWorkingSpecification = 310,
        /// <summary>
        /// Indicates Request Status is Working Specifications Pending Review
        /// </summary>
        [EnumMember, Description("Working Specifications Pending Review")]
        WorkingSpecificationPendingReview = 320,
        /// <summary>
        /// Indicates Request Status is Specifications Pending Review
        /// </summary>
        [EnumMember, Description("Specifications Pending Review")]
        SpecificationsPendingReview = 330,
        /// <summary>
        /// Indicates Request Status is Pending Specifications
        /// </summary>
        [EnumMember, Description("Pending Specifications")]
        PendingSpecifications = 340,
        /// <summary>
        /// Indicates Request Status is Pending Pre-Distribution Testing
        /// </summary>
        [EnumMember, Description("Pending Pre-Distribution Testing")]
        PendingPreDistributionTesting = 350,
        /// <summary>
        /// Indicates Request Status is Pre-Distribution Testing Pending Review
        /// </summary>
        [EnumMember, Description("Pre-Distribution Testing Pending Review")]
        PreDistributionTestingPendingReview =  360,
        /// <summary>
        /// Indicates Request Status is Request Pending Distribution
        /// </summary>
        [EnumMember, Description("Request Pending Distribution")]
        RequestPendingDistribution = 370,
        /// <summary>
        /// Indicates Request Status is Terminated prior to distribution.
        /// </summary>
        [EnumMember, Description("Terminated")]
        TerminatedPriorToDistribution = 390,
        /// <summary>
        /// Indicates Request Status is Request Rejected
        /// </summary>
        [EnumMember, Description("Request Rejected")]
        RequestRejected = 400,        
        /// <summary>
        /// Indicates Request Status Submitted
        /// </summary>
        [EnumMember]
        Submitted = 500,
        /// <summary>
        /// Indicates Request Status is Re-submitted
        /// </summary>
        [EnumMember, Description("Re-submitted")]
        Resubmitted = 600,
        /// <summary>
        /// Indicates Request Status is Pending Upload
        /// </summary>
        [EnumMember, Description("Pending Upload")]
        PendingUpload = 700,
        /// <summary>
        /// Indicates Request Status is Response Rejected Before Upload
        /// </summary>
        [EnumMember, Description("Response Rejected Before Upload")]
        ResponseRejectedBeforeUpload = 800,
        /// <summary>
        /// Indicates Request Status is Response Rejected After Upload
        /// </summary>
        [EnumMember, Description("Response Rejected After Upload")]
        ResponseRejectedAfterUpload = 900,
        /// <summary>
        /// Indicates Request Status is Examined By Investigator
        /// </summary>
        [EnumMember, Description("Examined By Investigator")]
        ExaminedByInvestigator = 1000,
        /// <summary>
        /// Indicates Request Status is Awaiting Response Approval
        /// </summary>
        [EnumMember, Description("Awaiting Response Approval")]
        AwaitingResponseApproval = 1100,
        /// <summary>
        /// Indicates Request Status is Partially Complete
        /// </summary>
        [EnumMember, Description("Partially Complete")]
        PartiallyComplete = 9000,
        /// <summary>
        /// Indicates Request Status is in Conducting Analysis
        /// </summary>
        [EnumMember, Description("Conducting Analysis")]
        ConductingAnalysis = 9050,
        /// <summary>
        /// Indicates Request Status is Pending Draft Report
        /// </summary>
        [EnumMember, Description("Pending Draft Report")]
        PendingDraftReport = 9110,
        /// <summary>
        /// Indicates Request Status is Draft Report Pending Approval
        /// </summary>
        [EnumMember, Description("Draft Report Pending Review")]
        DraftReportPendingApproval = 9120,
        /// <summary>
        /// Indicates Request Status is Pending Final Report
        /// </summary>
        [EnumMember, Description("Pending Final Report")]
        PendingFinalReport = 9130,
        /// <summary>
        /// Indicates Request Status is Final Report Pending Review
        /// </summary>
        [EnumMember, Description("Final Report Pending Review")]
        FinalReportPendingReview = 9140,
        /// <summary>
        /// Indicates Request Status is Hold
        /// </summary>
        [EnumMember]
        Hold = 9997,
        /// <summary>
        /// Indicates Request Status is Failed
        /// </summary>
        [EnumMember]
        Failed = 9998,
        /// <summary>
        /// Indicates Request Status is Cancelled
        /// </summary>
        [EnumMember]
        Cancelled = 9999,
        /// <summary>
        /// Indicates Request Status is Complete
        /// </summary>
        [EnumMember]
        Complete = 10000,
        /// <summary>
        /// Indicates Request Status is Complete with Report
        /// </summary>
        [EnumMember, Description("Complete, with Report")]
        CompleteWithReport = 10100
    }
}
