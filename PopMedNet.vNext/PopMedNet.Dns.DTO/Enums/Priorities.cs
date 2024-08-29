using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of priorities
    /// </summary>
    [DataContract]
    public enum Priorities : byte
    {
        /// <summary>
        /// Low
        /// </summary>
        [EnumMember]
        Low = 0,
        /// <summary>
        /// Medium
        /// </summary>
        [EnumMember]
        Medium = 1,
        /// <summary>
        /// High
        /// </summary>
        [EnumMember]
        High = 2,
        /// <summary>
        /// Urgent
        /// </summary>
        [EnumMember]
        Urgent = 3
    }
}
