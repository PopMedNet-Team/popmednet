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
    /// Types of Data available period categories
    /// </summary>
    [DataContract]
    public enum DataAvailabilityPeriodCategories
    {
        /// <summary>
        /// Years
        /// </summary>
        [EnumMember, Description("Available period for selection defined Annually")]
        Years = 1,
        /// <summary>
        /// Quarters
        /// </summary>
        [EnumMember, Description("Available period for selection defined Quarterly")]
        Quarters = 2
    }
}
