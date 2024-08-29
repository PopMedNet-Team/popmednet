using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Lookup Metrics
    /// </summary>
    [DataContract]
    public enum Metrics
    {
        /// <summary>
        /// Not Applicable
        /// </summary>
        [XmlEnum("0"), EnumMember, Description("Not Applicable")]
        NotApplicable = 0,
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
        /// <summary>
        /// Dispensing (Drug Only)
        /// </summary>
        [XmlEnum("3"), EnumMember, Description("Dispensing (Drug Only)")]
        Dispensing_DrugOnly = 3,
        /// <summary>
        /// Days Supply (Drug Only)
        /// </summary>
        [XmlEnum("4"), EnumMember, Description("Days Suppy (Drug Only)")]
        DaysSupply_DrugOnly = 4
    }
}
