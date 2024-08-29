using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Age Stratifications
    /// </summary>
    [DataContract]
    public enum AgeStratifications
    {
        /// <summary>
        /// 10 stratifications
        /// </summary>
        [EnumMember, Description("10 Stratifications (0-1,2-4,5-9,10-14,15-18,19-21,22-44,45-64,65-74,75+)")]
        Ten = 1,
        /// <summary>
        /// 7 Stratifications
        /// </summary>
        [EnumMember, Description("7 Stratifications (0-4,5-9,10-18,19-21,22-44,45-64,65+)")]
        Seven = 2,
        /// <summary>
        /// 4 Stratifications
        /// </summary>
        [EnumMember, Description("4 Stratifications (0-21,22-44,45-64,65+)")]
        Four = 3,
        /// <summary>
        /// 2 Stratifications
        /// </summary>
        [EnumMember, Description("2 Stratifications (Under 65,65+)")]
        Two = 4,
        /// <summary>
        /// No Stratifications
        /// </summary>
        [EnumMember, Description("No Stratifications (0+)")]
        None = 5,
        /// <summary>
        /// Five year groupings
        /// </summary>
        [EnumMember, Description("5 Year Groupings")]
        FiveYearGrouping = 6,
        /// <summary>
        /// Ten year groupings
        /// </summary>
        [EnumMember, Description("10 Year Groupings")]
        TenYearGrouping = 7
    }
}
