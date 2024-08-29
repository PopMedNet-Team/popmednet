using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Templates
    /// </summary>
    [DataContract]
    public enum TemplateTypes
    {
        /// <summary>
        /// Request Template
        /// </summary>
        [EnumMember, Description("Request Template")]
        Request = 1,
        /// <summary>
        /// Criteria Group
        /// </summary>
        [EnumMember, Description("Criteria Group")]
        CriteriaGroup = 2
    }
}
