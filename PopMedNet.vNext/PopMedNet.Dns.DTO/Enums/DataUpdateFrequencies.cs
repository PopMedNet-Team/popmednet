using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Data Update frequencies
    /// </summary>
    [DataContract]
    public enum DataUpdateFrequencies
    {
        /// <summary>
        /// Indicates DataUpdateFrequency is None
        /// </summary>
        [EnumMember, Description("None")]
        None = 1,
        /// <summary>
        /// Indicates DataUpdateFrequency is Daily
        /// </summary>
        [EnumMember, Description("Daily")]
        Daily = 2,
        /// <summary>
        /// Indicates DataUpdateFrequency is Weekly
        /// </summary>
        [EnumMember, Description("Weekly")]
        Weekly = 3,
        /// <summary>
        /// Indicates DataUpdateFrequency is Monthly
        /// </summary>
        [EnumMember, Description("Monthly")]
        Monthly = 4,
        /// <summary>
        /// Indicates DataUpdateFrequency is Quarterly
        /// </summary>
        [EnumMember, Description("Quarterly")]
        Quarterly = 5,
        /// <summary>
        /// Indicates DataUpdateFrequency is Semi-Annually
        /// </summary>
        [EnumMember, Description("Semi-Annually")]
        SemiAnnually = 6,
        /// <summary>
        /// Indicates DataUpdateFrequency is Annually
        /// </summary>
        [EnumMember, Description("Annually")]
        Annually = 7,
        /// <summary>
        /// Indicates DataUpdateFrequency is Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 8,
    }
}
