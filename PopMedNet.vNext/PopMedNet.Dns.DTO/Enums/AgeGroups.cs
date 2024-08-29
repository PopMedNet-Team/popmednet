using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Age Groups
    /// </summary>
    [DataContract]
    public enum AgeGroups
    {
        /// <summary>
        /// Indicates Age Group is 0-9
        /// </summary>
        [EnumMember, Description("0-9")]
        Age_0_9 = 1,
        /// <summary>
        /// Indicates Age Group is 10-19
        /// </summary>
        [EnumMember, Description("10-19")]
        Age_10_19 = 2,
        /// <summary>
        /// Indicates Age Group is 20-29
        /// </summary>
        [EnumMember, Description("20-29")]
        Age_20_29 = 3,
        /// <summary>
        /// Indicates Age Group is 30-39
        /// </summary>
        [EnumMember, Description("30-39")]
        Age_30_39 = 4,
        /// <summary>
        /// Indicates Age Group is 40-49
        /// </summary>
        [EnumMember, Description("40-49")]
        Age_40_49 = 5,
        /// <summary>
        /// Indicates Age Group is 50-59
        /// </summary>
        [EnumMember, Description("50-59")]
        Age_50_59 = 6,
        /// <summary>
        /// Indicates Age Group is 60-69
        /// </summary>
        [EnumMember, Description("60-69")]
        Age_60_69 = 7,
        /// <summary>
        /// Indicates Age Group is 70-79
        /// </summary>
        [EnumMember, Description("70-79")]
        Age_70_79 = 8,
        /// <summary>
        /// Indicates Age Group is 80-89
        /// </summary>
        [EnumMember, Description("80-89")]
        Age_80_89 = 9,
        /// <summary>
        /// Indicates Age Group is 90-99
        /// </summary>
        [EnumMember, Description("90-99")]
        Age_90_99 = 10,
    }
}
