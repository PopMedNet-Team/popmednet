using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    //Note: When you update these statuses you must update the trigger on RequestDataMarts to know how to handle the new status,
    //      and also Lpp.Dns.DTO.DataMartClient.Enums.RoutingStatus to match.
    /// <summary>
    /// Types of Routing Status
    /// </summary>
    [DataContract]
    public enum RoutingStatus
    {
        /// <summary>
        /// Indicates Routing status is Draft
        /// </summary>
        [EnumMember]
        Draft = 1,
        /// <summary>
        /// Indicates Routing status is Submitted
        /// </summary>
        [EnumMember]
        Submitted = 2,
        /// <summary>
        /// Indicates Routing status is Completed
        /// </summary>
        [EnumMember]
        Completed = 3,
        /// <summary>
        /// Indicates Routing status is Awaiting Request Approval
        /// </summary>
        [EnumMember, Description("Awaiting Request Approval")]
        AwaitingRequestApproval = 4,
        /// <summary>
        /// Indicates Routing status is Request Rejected
        /// </summary>
        [EnumMember, Description("Request Rejected")]
        RequestRejected = 5,
        /// <summary>
        /// Indicates Routing status is Canceled
        /// </summary>
        [EnumMember]
        Canceled = 6,
        /// <summary>
        /// Indicates Routing status is Re-submitted
        /// </summary>
        [EnumMember, Description("Re-submitted")]
        Resubmitted = 7,
        /// <summary>
        /// Indicates Routing status is Pending Upload
        /// </summary>
        [EnumMember, Description("Pending Upload")]
        PendingUpload = 8,
        /// <summary>
        /// Indicates Routing status is Awaiting Response Approval
        /// </summary>
        [EnumMember, Description("Awaiting Response Approval")]
        AwaitingResponseApproval = 10,
        /// <summary>
        /// Indicates Routing status is Hold
        /// </summary>
        [EnumMember]
        Hold = 11,
        /// <summary>
        /// Indicates Routing status is Response Rejected Before Upload
        /// </summary>
        [EnumMember, Description("Response Rejected Before Upload")]
        ResponseRejectedBeforeUpload = 12,
        /// <summary>
        /// Indicates Routing status is Response Rejected After Upload
        /// </summary>
        [EnumMember, Description("Response Rejected After Upload")]
        ResponseRejectedAfterUpload = 13,
        /// <summary>
        /// Indicates Routing status is Examined By Investigator
        /// </summary>
        [EnumMember, Description("Examined By Investigator")]
        ExaminedByInvestigator = 14,
        /// <summary>
        /// Indicates that the results have been modified/re-uploaded. (PMNDEV-4303)
        /// </summary>
        [EnumMember, Description("Results Modified")]
        ResultsModified = 16,
        /// <summary>
        /// Indicates Routing status is Failed
        /// </summary>
        [EnumMember]
        Failed = 99
    }
}
