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
    /// Password scores
    /// </summary>
    [DataContract]
    public enum PasswordScores
    {
        /// <summary>
        /// Invalid
        /// </summary>
        [EnumMember]
        Invalid = 0,
        /// <summary>
        /// Blank
        /// </summary>
        [EnumMember]
        Blank = 1,
        /// <summary>
        /// Very Week
        /// </summary>
        [EnumMember, Description("Very Week")]
        VeryWeak = 2,
        /// <summary>
        /// Weak
        /// </summary>
        [EnumMember]
        Weak = 3,
        /// <summary>
        /// Average
        /// </summary>
        [EnumMember]
        Average = 4,
        /// <summary>
        /// Strong
        /// </summary>
        [EnumMember]
        Strong = 5,
        /// <summary>
        /// Very Strong
        /// </summary>
        [EnumMember, Description("Very Strong")]
        VeryStrong = 6
    }

}
