using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
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
