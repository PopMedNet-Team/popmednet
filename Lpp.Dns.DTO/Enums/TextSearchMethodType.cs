using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Defines the way text is searched.
    /// </summary>
    [DataContract]
    public enum TextSearchMethodType
    {
        /// <summary>
        /// Values should be an exact match to the specified value.
        /// </summary>
        [EnumMember, Description("Exact Match")]
        ExactMatch = 0,
        /// <summary>
        /// Values should start with the specified value.
        /// </summary>
        [EnumMember, Description("Starts With")]
        StartsWith = 1
    }
}
