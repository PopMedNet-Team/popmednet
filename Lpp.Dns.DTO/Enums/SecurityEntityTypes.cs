using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Security Entities
    /// </summary>
    [DataContract]
    public enum SecurityEntityTypes
    {
        /// <summary>
        /// User
        /// </summary>
        [EnumMember]
        User = 1,
        /// <summary>
        /// Security Group
        /// </summary>
        [EnumMember, Description("Security Group")]
        SecurityGroup = 2
    }
}
