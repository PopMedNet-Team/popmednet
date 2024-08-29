using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Diagnosis code set types.
    /// </summary>
    [DataContract]
    public enum DiagnosisCodeTypes
    {
        /// <summary>
        /// All code types
        /// </summary>
        [EnumMember, Description("Any (all code types)")]
        Any = -1,
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember, Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// No Information
        /// </summary>
        [EnumMember, Description("No Information")]
        NoInformation = 1,
        /// <summary>
        /// Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 2,
        /// <summary>
        /// ICD-9-CM
        /// </summary>
        [EnumMember, Description("ICD-9-CM")]
        ICD9 = 3,
        /// <summary>
        /// ICD-10-CM
        /// </summary>
        [EnumMember, Description("ICD-10-CM")]
        ICD10 = 4,
        /// <summary>
        /// ICD-11-CM
        /// </summary>
        [EnumMember, Description("ICD-11-CM")]
        ICD11 = 5,
        /// <summary>
        /// SNOWMED-CT
        /// </summary>
        [EnumMember, Description("SNOMED-CT")]
        SNOWMED_CT = 6        
    }
}
