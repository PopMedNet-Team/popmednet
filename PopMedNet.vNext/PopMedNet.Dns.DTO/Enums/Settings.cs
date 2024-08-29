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
    /// Types of Settings
    /// </summary>
    [DataContract]
    public enum Settings
    {
        /// <summary>
        /// Inpatient Hospital Stay
        /// </summary>
        [EnumMember, Description("Inpatient Hospital Stay")]
        IP = 1,
        /// <summary>
        /// Outpatient(Ambulatory Visit)
        /// </summary>
        [EnumMember, Description("Outpatient (Ambulatory Visit)")]
        AV = 2,
        /// <summary>
        /// Emergency Department
        /// </summary>
        [EnumMember, Description("Emergency Department")]
        ED = 3,
        /// <summary>
        /// Any Setting
        /// </summary>
        [EnumMember, Description("Any Setting")]
        AN = 4,
        /// <summary>
        /// Emergency Department Admit to Inpatient Hospital Stay
        /// </summary>
        [EnumMember, Description("Emergency Department Admit to Inpatient Hospital Stay")]
        EI = 5,
        /// <summary>
        /// Non-Acute Institutional Stay
        /// </summary>
        [EnumMember, Description("Non-Acute Institutional Stay")]
        IS = 6,
        /// <summary>
        /// Observation Stay
        /// </summary>
        [EnumMember, Description("Observation Stay")]
        OS = 11,
        /// <summary>
        /// Institutional Professional Consult
        /// </summary>
        [EnumMember, Description("Institutional Professional Consult")]
        IC = 12,
        /// <summary>
        /// Other Ambulatory Visit
        /// </summary>
        [EnumMember, Description("Other Ambulatory Visit")]
        OA = 7,
        /// <summary>
        /// No information
        /// </summary>
        [EnumMember, Description("No information")]
        NI = 8,
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember, Description("Unknown")]
        UN = 9,
        /// <summary>
        /// Other
        /// </summary>
        [EnumMember, Description("Other")]
        OT = 10
    }
}
