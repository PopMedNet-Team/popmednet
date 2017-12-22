using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient.Enums
{
    //Note this has large gaps to allow for sub statuses and additions as necessary.
    //When adding to this, make sure you leave room for additional items later

    //Note 2: When you update these statuses you must update the trigger on RequestDataMarts to know how to handle the new status.
    [DataContract]
    public enum RequestStatuses
    {
        [EnumMember, Description("3rd Party Submitted Draft")]
        ThirdPartySubmittedDraft = 100,
        [EnumMember]
        Draft = 200,
        [EnumMember, Description("Draft Review")]
        DraftReview = 250,
        [EnumMember, Description("Awaiting Request Approval")]
        AwaitingRequestApproval = 300,
        [EnumMember, Description("Awaiting LPP Request Approval")]
        AwaitingLppRequestApproval = 350,
        [EnumMember, Description("Request Rejected")]
        RequestRejected = 400,
        [EnumMember]
        Submitted = 500,
        [EnumMember, Description("Re-submitted")]
        Resubmitted = 600,
        [EnumMember, Description("Pending Upload")]
        PendingUpload = 700,
        [EnumMember, Description("Response Rejected Before Upload")]
        ResponseRejectedBeforeUpload = 800,
        [EnumMember, Description("Response Rejected After Upload")]
        ResponseRejectedAfterUpload = 900,
        [EnumMember, Description("Examined By Investigator")]
        ExaminedByInvestigator = 1000,
        [EnumMember, Description("Awaiting Response Approval")]
        AwaitingResponseApproval = 1100,
        [EnumMember, Description("Partially Complete")]
        PartiallyComplete = 9000,
        [EnumMember]
        Hold = 9997,
        [EnumMember]
        Failed = 9998,
        [EnumMember]
        Cancelled = 9999,
        [EnumMember]
        Complete = 10000
    }
}
