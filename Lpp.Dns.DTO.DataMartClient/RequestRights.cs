using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [Flags]
    [DataContract]
    public enum RequestRights
    {
        [EnumMember]
        Run = 0x01,
        [EnumMember]
        Hold = 0x02,
        [EnumMember]
        Reject = 0x04,
        [EnumMember]
        ModifyResults = 0x08,
        [EnumMember]
        All = Run | Hold | Reject | ModifyResults
    }
}
