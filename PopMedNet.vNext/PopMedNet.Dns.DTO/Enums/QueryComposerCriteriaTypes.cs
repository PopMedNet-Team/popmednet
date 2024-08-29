using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    [DataContract]
    public enum QueryComposerCriteriaTypes
    {
        [EnumMember]
        Paragraph = 0,
        [EnumMember]
        Event = 1,
        [EnumMember, Description("Index Event")]
        IndexEvent = 2
    }
}
