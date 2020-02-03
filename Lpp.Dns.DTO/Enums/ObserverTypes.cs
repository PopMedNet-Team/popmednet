using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Observer Types
    /// </summary>
    [DataContract]
    public enum ObserverTypes
    {
        /// <summary>
        /// The observer is a user
        /// </summary>
        [EnumMember]
        User = 1,
        /// <summary>
        /// The observer is a security group
        /// </summary>
        [EnumMember]
        SecurityGroup = 2
    }
}
