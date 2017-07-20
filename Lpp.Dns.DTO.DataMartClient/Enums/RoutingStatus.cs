using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient.Enums
{
    //Note: When you update these statuses you must update the trigger on RequestDataMarts to know how to handle the new status,
    //      and also Lpp.Dns.DTO.Enums.DMCRoutingStatus to match.
    [DataContract]
    public enum DMCRoutingStatus
    {
        [EnumMember]
        Draft = 1,
        [EnumMember]
        Submitted = 2,
        [EnumMember]
        Completed = 3,
        [EnumMember, Description("Awaiting Request Approval")]
        AwaitingRequestApproval = 4,
        [EnumMember, Description("Request Rejected")]
        RequestRejected = 5,
        [EnumMember]
        Canceled = 6,
        [EnumMember, Description("Re-submitted")]
        Resubmitted = 7,
        [EnumMember, Description("Pending Upload")]
        PendingUpload = 8,
        [EnumMember, Description("Awaiting Response Approval")]
        AwaitingResponseApproval = 10,
        [EnumMember]
        Hold = 11,
        [EnumMember, Description("Response Rejected Before Upload")]
        ResponseRejectedBeforeUpload = 12,
        [EnumMember, Description("Response Rejected After Upload")]
        ResponseRejectedAfterUpload = 13,
        [EnumMember, Description("Examined By Investigator")]
        ExaminedByInvestigator = 14,
        [EnumMember, Description("Awaiting LPP Request Approval")]
        AwaitingLppRequestApproval = 15,
        [EnumMember, Description("Results Modified")]
        ResultsModified = 16,
        [EnumMember]
        Failed = 99
    }
}
