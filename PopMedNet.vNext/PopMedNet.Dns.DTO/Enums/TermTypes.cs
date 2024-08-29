using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Term types
    /// </summary>
    [Flags, DataContract]
    public enum TermTypes
    {
        /// <summary>
        /// Indicates the term type is Criteria
        /// </summary>
        [EnumMember]
        Criteria = 1,
        /// <summary>
        /// Indicates the term type is Selector
        /// </summary>
        [EnumMember]
        Selector = 2
    }
}
