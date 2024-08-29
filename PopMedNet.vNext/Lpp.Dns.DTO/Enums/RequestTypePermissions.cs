using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Request type permissions
    /// </summary>
    [DataContract, Flags]
    public enum RequestTypePermissions
    {
        /// <summary>
        /// Indicates RequestType Permission is Deny
        /// </summary>
        [EnumMember]
        Deny = 0,
        /// <summary>
        /// Indicates RequestType Permission is Manual
        /// </summary>
        [EnumMember]
        Manual = 1,
        /// <summary>
        /// Indicates RequestType Permission is Automatic
        /// </summary>
        [EnumMember]
        Automatic = 2
    }
}
