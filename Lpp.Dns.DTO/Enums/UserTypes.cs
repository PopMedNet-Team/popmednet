using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// User types
    /// </summary>
    [DataContract]
    public enum UserTypes
    {
        /// <summary>
        /// User
        /// </summary>
        [EnumMember]
        User = 0,
        /// <summary>
        /// Sso
        /// </summary>
        [EnumMember]
        Sso = 1,
        /// <summary>
        /// BackgroundTask
        /// </summary>
        [EnumMember]
        BackgroundTask = 2
    }
}
