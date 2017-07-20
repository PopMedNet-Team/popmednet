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
    /// ESP Diagnosis code set types.
    /// </summary>
    [DataContract]
    public enum ESPCodes
    {
        /// <summary>
        /// ICD9 Diagnosis Codes
        /// </summary>
        [EnumMember, Description("ICD9 Diagnosis Codes")]
        ICD9 = 10,
        /// <summary>
        ///ICD10 Diagnosis Codes
        /// </summary>
        [EnumMember, Description("ICD10 Diagnosis Codes")]
        ICD10 = 18,
    }
}
