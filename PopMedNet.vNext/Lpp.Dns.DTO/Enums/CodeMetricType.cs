using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
        ///Types of Code Metric types
        /// </summary>
    [DataContract]
    public enum CodeMetric
    {
        /// <summary>
        /// Events
        /// </summary>
        [XmlEnum("1"), EnumMember, Description("Events")]
        Events = 1,
        /// <summary>
        /// Users
        /// </summary>
        [XmlEnum("2"), EnumMember, Description("Users")]
        Users = 2,
    }
}
