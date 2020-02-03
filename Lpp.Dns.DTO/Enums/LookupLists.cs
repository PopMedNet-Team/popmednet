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
    /// Types of Lookups
    /// </summary>
    [DataContract]
    public enum Lists
    {
        /// <summary>
        /// Drug Names
        /// </summary>
        [EnumMember, Description("Drug Names")]
        GenericName = 1,
        /// <summary>
        /// Drug Classes
        /// </summary>
        [EnumMember, Description("Drug Classes")]
        DrugClass = 2,
        /// <summary>
        /// Drug Codes
        /// </summary>
        [EnumMember, Description("Drug Codes")]
        DrugCode = 3,
        /// <summary>
        /// ICD9 Diagnosis
        /// </summary>
        [EnumMember, Description("ICD9 Diagnosis")]
        ICD9Diagnosis = 4,
        /// <summary>
        /// ICD9 Procedures
        /// </summary>
        [EnumMember, Description("ICD9 Procedures")]
        ICD9Procedures = 5,
        /// <summary>
        /// HCPCS Procedures
        /// </summary>
        [EnumMember, Description("HCPCS Procedures")]
        HCPCSProcedures = 6,
        /// <summary>
        /// ICD9 Diagnosis 4 Digits
        /// </summary>
        [EnumMember, Description("ICD9 Diagnosis 4 Digits")]
        ICD9Diagnosis4Digits = 7,
        /// <summary>
        /// ICD9 Diagnosis 5 Digits
        /// </summary>
        [EnumMember, Description("ICD9 Diagnosis 5 Digits")]
        ICD9Diagnosis5Digits = 8,
        /// <summary>
        /// ICD9 Procedures 4 Digits
        /// </summary>
        [EnumMember, Description("ICD9 Procedures 4 Digits")]
        ICD9Procedures4Digits = 9,
        /// <summary>
        /// SPAN Diagnosis
        /// </summary>
        [EnumMember, Description("SPAN Diagnosis")]
        SPANDiagnosis = 10,
        /// <summary>
        /// SPAN Procedures
        /// </summary>
        [EnumMember, Description("SPAN Procedures")]
        SPANProcedure = 11,
        /// <summary>
        /// SPAN Drugs
        /// </summary>
        [EnumMember, Description("SPAN Drugs")]
        SPANDRUG = 12,
        /// <summary>
        /// ZIP Codes
        /// </summary>
        [EnumMember, Description("ZIP Codes")]
        ZipCodes = 13,
        [EnumMember, Description("ICD10 Procedures")]
        ICD10Procedures = 14,
        [EnumMember, Description("ICD10 Diagnosis")]
        ICD10Diagnosis = 15,
        [EnumMember, Description("National Drug Codes")]
        NationalDrugCodes = 16,
        [EnumMember, Description("Revenue Codes")]
        RevenueCodes = 17,
        [EnumMember, Description("ESP ICD10 Codes")]
        ESPICD10 = 18,
    }
}
