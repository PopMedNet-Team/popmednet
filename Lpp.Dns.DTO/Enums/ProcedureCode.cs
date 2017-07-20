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
    /// Types of Procedure COdes
    /// </summary>
    [DataContract]
    public enum ProcedureCode
    {
        /// <summary>
        /// Indicates Any Code Types
        /// </summary>
        [DataMember, Description("Any (all Code Types)")]
        Any = 1,
        /// <summary>
        /// Indicates ICD 9 Code Types
        /// </summary>
        [DataMember, Description("ICD-9-CM")]
        ICD9 = 2,
        /// <summary>
        /// Indicates ICD 10 Code Types
        /// </summary>
        [DataMember, Description("ICD-10-PCS")]
        ICD10 = 3,
        /// <summary>
        /// Indicates ICD 11 Code Types
        /// </summary>
        [DataMember, Description("ICD-11-PCS")]
        ICD11 = 4,
        /// <summary>
        /// Indicates CPT or HCPCS Code Types
        /// </summary>
        [DataMember, Description("CPT or HCPCS")]
        CPT = 5,
        /// <summary>
        /// Indicates LOINC Code Types
        /// </summary>
        [DataMember, Description("LOINC")]
        LOINC = 6,
        /// <summary>
        /// Indicates NDC Code Types
        /// </summary>
        [DataMember, Description("NDC")]
        NDC = 7,
        /// <summary>
        /// Indicates Revenue Code Types
        /// </summary>
        [DataMember, Description("Revenue")]
        Revenue = 8,
        /// <summary>
        /// Indicates No Information Code Types
        /// </summary>
        [DataMember, Description("No Information")]
        NoInformation = 9,
        /// <summary>
        /// Indicates Unknown Code Types
        /// </summary>
        [DataMember, Description("Unknown")]
        Unknown = 10,
        /// <summary>
        /// Indicates Other Code Types
        /// </summary>
        [DataMember, Description("Other")]
        Other = 11
    }
}
