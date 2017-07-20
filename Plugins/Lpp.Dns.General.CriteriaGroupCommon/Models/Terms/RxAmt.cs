using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RequestCriteria.Models
{
    [DataContract]
    public class RxAmountData : TermData
    {
        [DataMember]
        public RxAmountTypes[] RxAmounts { get; set; }
    }

    [DataContract]
    public enum RxAmountTypes
    {
        [EnumMember]
        LessThanZero = 0,      // <0
        [EnumMember]
        Zero = 1,              // 0-1
        [EnumMember]
        TwoThroughThirty = 2,  // 2-30
        [EnumMember]
        Thirty = 3,            // 31-60
        [EnumMember]
        Sixty = 4,             // 61-90
        [EnumMember]
        Ninety = 5,            // 91-120
        [EnumMember]
        OneHundredTwenty = 6,  // 121-180
        [EnumMember]
        OneHundredEighty = 7,  // >180
        [EnumMember]
        Other = 8,
        [EnumMember]
        Missing = 9
    }
}
