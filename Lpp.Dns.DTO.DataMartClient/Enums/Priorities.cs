using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient.Enums
{
    [DataContract]
    public enum Priorities : byte
    {
        [EnumMember]
        Low = 0,
        [EnumMember]
        Medium = 1,
        [EnumMember]
        High = 2,
        [EnumMember]
        Urgent = 3
    }
}
