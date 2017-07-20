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
    /// Types of Querycomposer Aggregates
    /// </summary>
    [DataContract]
    public enum QueryComposerAggregates
    {
        /// <summary>
        /// Count
        /// </summary>
        [EnumMember]
        Count = 1,
        /// <summary>
        /// Min
        /// </summary>
        [EnumMember]
        Min = 2,
        /// <summary>
        /// Max
        /// </summary>
        [EnumMember]
        Max = 3,
        /// <summary>
        /// Average
        /// </summary>
        [EnumMember]
        Average = 4,
        /// <summary>
        /// Variance
        /// </summary>
        [EnumMember]
        Variance = 5,
        /// <summary>
        /// Population Variance
        /// </summary>
        [EnumMember, Description("Population Variance")]
        PopulationVariance = 6,
        /// <summary>
        /// Sum
        /// </summary>
        [EnumMember]
        Sum = 7,
        /// <summary>
        /// Standard Deviation
        /// </summary>
        [EnumMember, Description("Standard Deviation")]
        StandardDeviation = 8,
        /// <summary>
        /// Population standard Deviation
        /// </summary>
        [EnumMember, Description("Population Standard Deviation")]
        PopulationDeviation = 9,
        /// <summary>
        /// Median
        /// </summary>
        [EnumMember]
        Median = 10
    }
}
