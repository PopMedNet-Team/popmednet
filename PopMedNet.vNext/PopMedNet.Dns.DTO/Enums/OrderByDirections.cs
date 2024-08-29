using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// OrderBy
    /// </summary>
    [DataContract]
    public enum OrderByDirections
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// Ascending
        /// </summary>
        [EnumMember]
        Ascending = 1,
        /// <summary>
        /// Descending
        /// </summary>
        [EnumMember]
        Descending = 2
    }
}
