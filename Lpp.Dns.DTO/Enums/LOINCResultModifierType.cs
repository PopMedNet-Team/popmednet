using System.ComponentModel;
using System.Runtime.Serialization;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Result Modifiers for LOIC Lab Records
    /// </summary>
    [DataContract]
    public enum LOINCResultModifierType
    {
        /// <summary>
        /// Equal
        /// </summary>
        [EnumMember, Description("Equal")]
        EQ = 1,
        /// <summary>
        /// Greater than or equal
        /// </summary>
        [EnumMember, Description("Greater than or equal")]
        GE = 2,
        /// <summary>
        /// Greater than
        /// </summary>
        [EnumMember, Description("Greater than")]
        GT = 3,
        /// <summary>
        /// Less than or equal to
        /// </summary>
        [EnumMember, Description("Less than or equal to")]
        LE = 4,
        /// <summary>
        /// Equal
        /// </summary>
        [EnumMember, Description("Less than")]
        LT = 5,
        /// <summary>
        /// Text
        /// </summary>
        [EnumMember, Description("Text")]
        Text = 6,
        /// <summary>
        /// Equal
        /// </summary>
        [EnumMember, Description("No Information")]
        NI = 7,
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember, Description("Unknown")]
        UN = 8,
        /// <summary>
        /// Other
        /// </summary>
        [EnumMember, Description("Other")]
        OT = 9,
    }
}
