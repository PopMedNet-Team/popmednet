using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of EHRS
    /// </summary>
    [DataContract]
    public enum EHRSTypes
    {
        /// <summary>
        /// Indicates EHR type is Inpatient
        /// </summary>
        [EnumMember]
        Inpatient = 1,
        /// <summary>
        /// Indicates EHR type is Outpatient
        /// </summary>
        [EnumMember]
        Outpatient = 2
    }
}
