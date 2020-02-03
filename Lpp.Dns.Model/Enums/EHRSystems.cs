using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model
{
    [DataContract]
    public enum EHRSystems
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Epic = 1,
        [EnumMember]
        AllScripts = 2,
        [EnumMember]
        EClinicalWorks = 3,
        [EnumMember]
        NextGenHealthCare = 4,
        [EnumMember]
        GEHealthCare = 5,
        [EnumMember]
        McKesson = 6,
        [EnumMember]
        Care360 = 7,
        [EnumMember]
        Cerner = 8,
        [EnumMember]
        CPSI = 9,
        [EnumMember]
        Meditech = 10,
        [EnumMember]
        Other = 11,
        [EnumMember]
        Siemens = 12,
        [EnumMember]
        VistA = 13
    }

}
