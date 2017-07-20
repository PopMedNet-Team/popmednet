using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model
{
    [DataContract]
    public enum Ethnicities
    {
        [EnumMember, Description("White")]
        NonHispanicWhite = 1,
        [EnumMember, Description("Black")]
        NonHispanicBlack = 2,
        [EnumMember, Description("Asian")]
        NonHispanicAsian = 3,
        [EnumMember, Description("Hispanic")]
        Hispanic = 4,
        [EnumMember, Description("Native American Indian/Alaskan")]
        NonHispanicNativeAmericanIndianAlaskan = 5
    }
}
