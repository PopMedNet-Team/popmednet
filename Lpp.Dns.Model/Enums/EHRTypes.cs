using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model
{
    [DataContract]
    public enum EHRTypes
    {
        [EnumMember]
        Inpatient = 1,
        [EnumMember]
        Outpatient = 2
    }

}
