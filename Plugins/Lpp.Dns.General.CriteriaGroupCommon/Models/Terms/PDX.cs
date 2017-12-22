using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RequestCriteria.Models
{
    [DataContract]
    public class PDXData : TermData
    {
        [DataMember]
        public PDXTypes[] PDXes { get; set; }
    }

    [DataContract]
    public enum PDXTypes
    {
        [EnumMember]
        Principle = 0,
        [EnumMember]
        Secondary = 1,
        [EnumMember]
        Other = 2,
        [EnumMember]
        Missing = 3,
    }
}
