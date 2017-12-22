using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Query Composer Interface Capabilities
    /// </summary>
    [DataContract]
    public enum QueryComposerInterface
    {
        /// <summary>
        /// Flexible Menu Driven Query
        /// </summary>
        [EnumMember, Description("Flexible Menu Driven Query")]
        FlexibleMenuDrivenQuery = 0,
        /// <summary>
        /// Preset Query
        /// </summary>
        [EnumMember, Description("Preset Query")]
        PresetQuery = 1,
        /// <summary>
        /// File Distribution
        /// </summary>
        [EnumMember, Description("File Distribution")]
        FileDistribution = 2,
    }
}
