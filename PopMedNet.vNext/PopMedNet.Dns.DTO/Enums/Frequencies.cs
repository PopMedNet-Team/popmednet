using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Frequencies
    /// </summary>
    [DataContract]
    public enum Frequencies
    {
        /// <summary>
        /// Immediately
        /// </summary>
        [EnumMember]
        Immediately = 0,
        /// <summary>
        /// Daily
        /// </summary>
        [EnumMember]
        Daily = 1,
        /// <summary>
        /// Weekly
        /// </summary>
        [EnumMember]
        Weekly = 2,
        /// <summary>
        /// Monthly
        /// </summary>
        [EnumMember]
        Monthly = 3
    }
}
