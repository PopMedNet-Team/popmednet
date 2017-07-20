using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RequestCriteria.Models
{
    [DataContract]
    public class RxSupData : TermData
    {
        [DataMember]
        public RxSupTypes[] RxSups { get; set; }
    }

    [DataContract]
    public enum RxSupTypes
    {
        [EnumMember]
        LessThanZero = 0,
        [EnumMember]
        Zero = 1,
        [EnumMember]
        TwoThroughThirty = 2,
        [EnumMember]
        Thirty = 3,
        [EnumMember]
        Sixty = 4,
        [EnumMember]
        Ninety = 5,
        [EnumMember]
        Other = 6,
        [EnumMember]
        Missing = 7
    }
}
