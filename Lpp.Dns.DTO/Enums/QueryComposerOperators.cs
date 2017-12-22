using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Query composer operators
    /// </summary>

    [DataContract]
    public enum QueryComposerOperators
    {
        /// <summary>
        /// And
        /// </summary>
        [EnumMember]
        And = 0,
        /// <summary>
        /// Or
        /// </summary>
        [EnumMember]
        Or = 1,
        /// <summary>
        /// And Not
        /// </summary>
        [EnumMember, Description("And Not")]
        AndNot = 2,
        /// <summary>
        /// Or not
        /// </summary>
        [EnumMember, Description("Or Not")]
        OrNot = 3
    }
}
