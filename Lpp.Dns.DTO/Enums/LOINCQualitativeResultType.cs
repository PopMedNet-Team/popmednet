using System.ComponentModel;
using System.Runtime.Serialization;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Qualitative Results for LOIC Lab Records
    /// </summary>
    [DataContract]
    public enum LOINCQualitativeResultType
    {
        /// <summary>
        /// Positive
        /// </summary>
        [EnumMember, Description("Positive")]
        Positive = 1,
        /// <summary>
        /// Negative
        /// </summary>
        [EnumMember, Description("Negative")]
        Negative = 2,
        /// <summary>
        /// Borderline
        /// </summary>
        [EnumMember, Description("Borderline")]
        Borderline = 3,
        /// <summary>
        /// Elevated
        /// </summary>
        [EnumMember, Description("Elevated")]
        Elevated = 4,
        /// <summary>
        /// High
        /// </summary>
        [EnumMember, Description("High")]
        High = 5,
        /// <summary>
        /// Low
        /// </summary>
        [EnumMember, Description("Low")]
        Low = 6,
        /// <summary>
        /// Normal
        /// </summary>
        [EnumMember, Description("Normal")]
        Normal = 7,
        /// <summary>
        /// Abnormal
        /// </summary>
        [EnumMember, Description("Abnormal")]
        Abnormal = 8,
        /// <summary>
        /// Undetermined
        /// </summary>
        [EnumMember, Description("Undetermined")]
        Undetermined = 9,
        /// <summary>
        /// Undetectable
        /// </summary>
        [EnumMember, Description("Undetectable")]
        Undetectable = 10,
        /// <summary>
        /// NI
        /// </summary>
        [EnumMember, Description("NI")]
        NI = 11,
        /// <summary>
        /// UN
        /// </summary>
        [EnumMember, Description("UN")]
        UN = 12,
        /// <summary>
        /// OT
        /// </summary>
        [EnumMember, Description("OT")]
        OT = 13
    }
}
