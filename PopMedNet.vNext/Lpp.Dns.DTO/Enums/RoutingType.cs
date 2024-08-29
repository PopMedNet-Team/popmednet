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