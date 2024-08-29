using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
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
