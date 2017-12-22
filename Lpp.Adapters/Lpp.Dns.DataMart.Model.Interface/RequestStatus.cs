using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Encapsulates current status code and message if any after a Request is made. 
    /// </summary>
    [DataContract, Serializable]
    public class RequestStatus
    {
        [DataContract]
        public enum StatusCode
        {
            [EnumMember]
            Complete = 66,
            [EnumMember]
            CompleteWithMessage = 67,
            [EnumMember]
            Pending = 4,
            [EnumMember]
            InProgress = 8,
            [EnumMember]
            AwaitingResponseApproval = 16,
            [EnumMember]
            Canceled = 80,
            [EnumMember]
            Error = 96,
            [EnumMember]
            STOP = 64
        }

        [DataMember]
        public StatusCode Code
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }

        [DataMember]
        public bool PostProcess
        {
            get;
            set;
        }

        public RequestStatus()
        {
            PostProcess = false;
        }

        public RequestStatus(StatusCode code)
            : base()
        {
            Code = code;
        }
    }
}
