using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Registry
    /// </summary>
    [DataContract]
    public enum RegistryTypes : byte
    {
        /// <summary>
        ///Indicates RegistryType is Registry
        /// </summary>
        [EnumMember]
        Registry = 0,
        /// <summary>
        ///Indicates RegistryType is ResearchDataSet
        /// </summary>
        [EnumMember, Description("Research Data Set")]
        ResearchDataSet = 1
    }
}
