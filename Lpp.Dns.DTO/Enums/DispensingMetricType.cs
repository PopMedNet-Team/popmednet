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
    /// Types of Dispensing metrics
    /// </summary>
    [DataContract]
    public enum DispensingMetric
    {
        /// <summary>
        /// Users
        /// </summary>
        [XmlEnum("2"), EnumMember, Description("Users")]
        Users = 2,
        /// <summary>
        /// Dispensing _ Drug Only
        /// </summary>
        [XmlEnum("3"), EnumMember, Description("Dispensing (Drug Only)")]
        Dispensing_DrugOnly = 3,
        /// <summary>
        /// Days Supply _ Drug Only
        /// </summary>
        [XmlEnum("4"), EnumMember, Description("Days Suppy (Drug Only)")]
        DaysSupply_DrugOnly = 4
    }
}
