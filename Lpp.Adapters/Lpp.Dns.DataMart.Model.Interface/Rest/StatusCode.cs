using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Rest
{
    /// <summary>
    /// Corresponds to IModelProcessor's RequestStatus.StatusCode enum.
    /// </summary>
    [DataContract(Namespace = "http://lincolnpeak.com/schemas/DNS4/API")]
    public enum StatusCode
    {
        [EnumMember]
        Complete = 66,            // 1000010
        [EnumMember]
        CompleteWithMessage = 67, // 1000011
        [EnumMember]
        Pending = 4,              // 0000100
        [EnumMember]
        InProgress = 8,           // 0001000
        [EnumMember]
        AwaitingApproval = 16,    // 0010000
        [EnumMember]
        Canceled = 80,            // 1010000
        [EnumMember]
        Error = 96,               // 1100000
    }
}
