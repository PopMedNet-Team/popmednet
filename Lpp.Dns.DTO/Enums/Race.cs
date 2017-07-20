using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Race
    /// </summary>
    [DataContract]
    public enum Race
    {
        /// <summary>
        /// Indicates Race is UnKnown
        /// </summary>
        [EnumMember, Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// Indicates Race is American Indian or Alaska Native
        /// </summary>
        [EnumMember, Description("American Indian or Alaska Native")]
        Native = 1,
        /// <summary>
        /// Indicates Race is Asian
        /// </summary>
        [EnumMember, Description("Asian")]
        Asian = 2,
        /// <summary>
        /// Indicates Race is Black or African American
        /// </summary>
        [EnumMember, Description("Black or African American")]
        Black = 3,
        /// <summary>
        /// Indicates Race is Native Hawaiian or Other Pacific Islander
        /// </summary>
        [EnumMember, Description("Native Hawaiian or Other Pacific Islander (NHOPI)")]
        Pacific = 4,
        /// <summary>
        /// Indicates Race is White
        /// </summary>
        [EnumMember, Description("White")]
        White = 5,
        /// <summary>
        /// Indicates Race is Multiple
        /// </summary>
        [EnumMember, Description("Multiple Race")]
        Multiple = 6,
        /// <summary>
        /// Indicates Race is Refuse to answer
        /// </summary>
        [EnumMember, Description("Refuse to Answer")]
        Refuse = 7,
        /// <summary>
        /// Indicates Race is No Information
        /// </summary>
        [EnumMember, Description("No Information")]
        NI = 8,
        /// <summary>
        /// Indicates Race is Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 9
        
    }
}
