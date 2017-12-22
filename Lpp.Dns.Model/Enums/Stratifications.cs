using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model.Enums
{
    [DataContract, Flags]
    public enum Stratifications
    {
        [DataMember]
        None = 0,
        [DataMember]
        Ethnicity = 1,
        [DataMember]
        Age = 2,
        [DataMember]
        Gender = 4
    }
}
