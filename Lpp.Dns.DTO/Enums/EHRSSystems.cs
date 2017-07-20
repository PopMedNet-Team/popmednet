using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of EHR systems
    /// </summary>
    [DataContract]
    public enum EHRSSystems
    {
        /// <summary>
        /// Indicates EHR Syatem is None
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// Indicates EHR Syatem is Epic
        /// </summary>
        [EnumMember]
        Epic = 1,
        /// <summary>
        /// Indicates EHR Syatem is AllScripts
        /// </summary>
        [EnumMember]
        AllScripts = 2,
        /// <summary>
        /// Indicates EHR Syatem is EClinicalWorks
        /// </summary>
        [EnumMember]
        EClinicalWorks = 3,
        /// <summary>
        /// Indicates EHR Syatem is NextGenHealthCare
        /// </summary>
        [EnumMember]
        NextGenHealthCare = 4,
        /// <summary>
        /// Indicates EHR Syatem is GEHealthcare
        /// </summary>
        [EnumMember]
        GEHealthCare = 5,
        /// <summary>
        /// Indicates EHR Syatem is McKesson
        /// </summary>
        [EnumMember]
        McKesson = 6,
        /// <summary>
        /// Indicates EHR Syatem is Care360
        /// </summary>
        [EnumMember]
        Care360 = 7,
        /// <summary>
        /// Indicates EHR Syatem is Cerner
        /// </summary>
        [EnumMember]
        Cerner = 8,
        /// <summary>
        /// Indicates EHR Syatem is CPSI
        /// </summary>
        [EnumMember]
        CPSI = 9,
        /// <summary>
        /// Indicates EHR Syatem is Meditech
        /// </summary>
        [EnumMember]
        Meditech = 10,
        /// <summary>
        /// Indicates EHR Syatem is Siemens
        /// </summary>
        [EnumMember]
        Siemens = 11,
        /// <summary>
        /// Indicates EHR Syatem is VistA
        /// </summary>
        [EnumMember]
        VistA = 12,
        /// <summary>
        /// Indicates EHR Syatem is Other
        /// </summary>
        [EnumMember]
        Other = 13        
    }
}
