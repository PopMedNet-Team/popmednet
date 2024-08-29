using System.Runtime.Serialization;
using Lpp.Dns.DTO.Enums;

namespace RequestCriteria.Models
{
    [DataContract]
    public class RequestStatusData : TermData
    {
        [DataMember]
        public RequestStatuses? RequestStatus { get; set; }
    }

    [DataContract]
    public enum RequestStatusTypes
    {
        [EnumMember]
        NotSpecified = -1,
        [EnumMember]
        Submitted = 0,
        [EnumMember]
        PartiallyComplete = 1,
        [EnumMember]
        Completed = 2,
        [EnumMember]
        Cancelled = 3,
        [EnumMember]
        Hold = 4,
        [EnumMember]
        Failed = 5,
        [EnumMember]
        AwaitingRequestApproval = 6,
        [EnumMember]
        RequestRejected = 7,
        [EnumMember]
        AwaitingResponseApproval = 8,
        [EnumMember]
        ResponseRejected = 9,
    }
}
