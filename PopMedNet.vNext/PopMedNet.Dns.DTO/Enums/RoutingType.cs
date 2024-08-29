using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of routings
    /// </summary>
    [DataContract]
    public enum RoutingType
    {
        /// <summary>
        /// Analysis Center routing
        /// </summary>
        [EnumMember, Description("Analysis Center")]
        AnalysisCenter = 0,

        /// <summary>
        /// Data Partner routing
        /// </summary>
        [EnumMember, Description("Data Partner")]
        DataPartner = 1
    }
}
