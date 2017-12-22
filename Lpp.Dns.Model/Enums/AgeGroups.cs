using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.Model
{
    [DataContract]
    public enum AgeGroups
    {
        [EnumMember, Description("0-9")]
        Age_0_9 = 1,
        [EnumMember, Description("10-19")]
        Age_10_19 = 2,
        [EnumMember, Description("20-29")]
        Age_20_29 = 3,
        [EnumMember, Description("30-39")]
        Age_30_39 = 4,
        [EnumMember, Description("40-49")]
        Age_40_49 = 5,
        [EnumMember, Description("50-59")]
        Age_50_59 = 6,
        [EnumMember, Description("60-69")]
        Age_60_69 = 7,
        [EnumMember, Description("70-79")]
        Age_70_79 = 8,
        [EnumMember, Description("80-89")]
        Age_80_89 = 9,
        [EnumMember, Description("90-99")]
        Age_90_99 = 10,
    }

}
