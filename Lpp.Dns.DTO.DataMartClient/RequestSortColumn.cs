using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public enum RequestSortColumn
    {
        [EnumMember]
        RequestId = 0,
        [EnumMember]
        RequestName = 1,
        [EnumMember]
        RequestModelType = 2,
        [EnumMember]
        RequestType = 3,
        [EnumMember]
        RequestPriority = 4,
        [EnumMember]
        RequestDueDate = 5,
        [EnumMember]
        RequestStatus = 6,
        [EnumMember]
        RequestTime = 7,
        [EnumMember]
        DataMartName = 8,
        [EnumMember]
        CreatedByUsername = 9,
        [EnumMember]
        ResponseTime = 10,
        [EnumMember]
        RespondedByUsername = 11,
        [EnumMember]
        ProjectName = 12,
        [EnumMember]
        MSRequestID = 13
    }
}
