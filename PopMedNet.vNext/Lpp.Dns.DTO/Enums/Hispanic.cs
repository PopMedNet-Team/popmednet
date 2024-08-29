using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    [DataContract]
    public enum Hispanic
    {
        /// <summary>
        /// Indicates Hispanic is UnKnown
        /// </summary>
        [EnumMember, Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// Indicates Hispanic is Yes
        /// </summary>
        [EnumMember, Description("Yes")]
        Yes = 1,
        /// <summary>
        /// Indicates Hispanic is Asian
        /// </summary>
        [EnumMember, Description("No")]
        No = 2,
        /// <summary>
        /// Indicates Hispanic is Refuse to Answer
        /// </summary>
        [EnumMember, Description("Refuse to Answer")]
        Refuse = 3,
        /// <summary>
        /// Indicates Hispanic is No Information
        /// </summary>
        [EnumMember, Description("No Information")]
        NI = 4,
        /// <summary>
        /// Indicates Hispanic is Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 5
    }
}
