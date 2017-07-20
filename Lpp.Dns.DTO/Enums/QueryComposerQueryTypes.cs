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
    /// Describes the sub request type of the query.
    /// </summary>
    [DataContract]
    public enum QueryComposerQueryTypes
    {
        /// <summary>
        /// Indicates the query is for cenus projections.
        /// </summary>
        [EnumMember, Description("Census Projections")]
        CenusProjections = 1,
        /// <summary>
        /// Indicates the query is Data Characterization: AgeRange.
        /// </summary>
        [EnumMember, Description("Data Characterization: Demographic - Age Range")]
        DataCharacterization_Demographic_AgeRange = 10,
        /// <summary>
        /// Indicates the query is Data Characterization: Ethnicity.
        /// </summary>
        [EnumMember, Description("Data Characterization: Demographic - Ethnicity")]
        DataCharacterization_Demographic_Ethnicity = 11,
        /// <summary>
        /// Indicates the query is Data Characterization: Race.
        /// </summary>
        [EnumMember, Description("Data Characterization: Demographic - Race")]
        DataCharacterization_Demographic_Race = 12,
        /// <summary>
        /// Indicates the query is Data Characterization: Sex.
        /// </summary>
        [EnumMember, Description("Data Characterization: Demographic - Sex")]
        DataCharacterization_Demographic_Sex = 13,
        /// <summary>
        /// Indicates the query is Data Characterization: Procedure Codes.
        /// </summary>
        [EnumMember, Description("Data Characterization: Procedure - Procedure Codes")]
        DataCharacterization_Procedure_ProcedureCodes = 14,
        /// <summary>
        /// Indicates the query is Data Characterization: Diagnosis Codes.
        /// </summary>
        [EnumMember, Description("Data Characterization: Diagnosis - Diagnosis Codes")]
        DataCharacterization_Diagnosis_DiagnosisCodes = 15,
        /// <summary>
        /// Indicates the query is Data Characterization: PDX.
        /// </summary>
        [EnumMember, Description("Data Characterization: Diagnosis - PDX")]
        DataCharacterization_Diagnosis_PDX = 16,
        /// <summary>
        /// Indicates the query is Data Characterization: NDC.
        /// </summary>
        [EnumMember, Description("Data Characterization: Dispensing - NDC")]
        DataCharacterization_Dispensing_NDC = 17,
        /// <summary>
        /// Indicates the query is Data Characterization: Rx Amount.
        /// </summary>
        [EnumMember, Description("Data Characterization: Dispensing - Rx Amount")]
        DataCharacterization_Dispensing_RxAmount = 18,
        /// <summary>
        /// Indicates the query is Data Characterization: Rx Supply.
        /// </summary>
        [EnumMember, Description("Data Characterization: Dispensing - Rx Supply")]
        DataCharacterization_Dispensing_RxSupply = 19,
        /// <summary>
        /// Indicates the query is Data Characterization: Data Completeness.
        /// </summary>
        [EnumMember, Description("Data Characterization: Metadata - Data Completeness")]
        DataCharacterization_Metadata_DataCompleteness = 20,
        /// <summary>
        /// Indicates the query is Data Characterization: Height.
        /// </summary>
        [EnumMember, Description("Data Characterization: Vital - Height")]
        DataCharacterization_Vital_Height = 21,
        /// <summary>
        /// Indicates the query is Data Characterization: Weight.
        /// </summary>
        [EnumMember, Description("Data Characterization: Vital - Weight")]
        DataCharacterization_Vital_Weight = 22,
        /// <summary>
        /// Indicates the query is Data Characterization: Prevalence.
        /// </summary>
        [EnumMember, Description("Summary Table: Prevalence")]
        SummaryTable_Prevalence = 40,
        /// <summary>
        /// Indicates the query is Data Characterization: Incidence.
        /// </summary>
        [EnumMember, Description("Summary Table: Incidence")]
        SummaryTable_Incidence = 41,
        /// <summary>
        /// Indicates the query is Data Characterization: MFU.
        /// </summary>
        [EnumMember, Description("Summary Table: MFU")]
        SummaryTable_MFU = 42,
        /// <summary>
        /// Indicates the query is a Raw Sql Query.
        /// </summary>
        [EnumMember, Description("Sql Query")]
        Sql = 50
    }
}
