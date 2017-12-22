using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Ethnicities
    /// </summary>
    [DataContract]
    public enum Ethnicities
    {
        /// <summary>
        ///Indicates Ethnicity is Unknown
        /// </summary>
        [EnumMember, Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// Indicates Ethnicity is American Indian or Alaska Native
        /// </summary>
        [EnumMember, Description("American Indian or Alaska Native")]
        Native = 1,
        /// <summary>
        /// Indicates Ethnicity is Asian
        /// </summary>
        [EnumMember, Description("Asian")]
        Asian = 2,
        /// <summary>
        /// Indicates Ethnicity is Black or African American
        /// </summary>
        [EnumMember, Description("Black or African American")]
        Black = 3,
        /// <summary>
        /// Indicates Ethnicity is White
        /// </summary>
        [EnumMember, Description("White")]
        White = 4,
        /// <summary>
        /// Indicates Ethnicity is Hispanic
        /// </summary>
        [EnumMember, Description("Hispanic")]
        Hispanic = 6,
        /// <summary>
        /// Indicates Race is Multiple
        /// </summary>
        [EnumMember, Description("Multiple Race")]
        Multiple = 7,
        /// <summary>
        /// Indicates Race is Refuse to answer
        /// </summary>
        [EnumMember, Description("Refuse to Answer")]
        Refuse = 8,
        /// <summary>
        /// Indicates Race is No Information
        /// </summary>
        [EnumMember, Description("No Information")]
        NI = 9,
        /// <summary>
        /// Indicates Race is Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 10
    }
}
