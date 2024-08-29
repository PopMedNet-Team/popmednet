
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    [DataContract]
    public enum ClinicalObservationsCodeSet
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember, Description("Unknown")]
        UN = 0,
        /// <summary>
        /// No Information
        /// </summary>
        [EnumMember, Description("No Information")]
        NI = 1,
        /// <summary>
        /// Other
        /// </summary>
        [EnumMember, Description("Other")]
        OT = 2,
        /// <summary>
        /// ICD-9-CM
        /// </summary>
        [EnumMember, Description("LOINC")]
        LC = 3,
        /// <summary>
        /// ICD-10-CM
        /// </summary>
        [EnumMember, Description("SNOMED CT")]
        SM = 4,
    }
}
