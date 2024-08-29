using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Enum to choose Yes or No
    /// </summary>
    [DataContract]
    public enum YesNo
    {
        /// <summary>
        /// Yes
        /// </summary>
        [EnumMember]
        Yes = 1,
        /// <summary>
        /// No
        /// </summary>
        [EnumMember]
        No = 0
    }
}
