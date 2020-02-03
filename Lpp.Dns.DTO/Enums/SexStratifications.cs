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
    /// Types of Sex Stratifications
    /// </summary>
    [DataContract]
    public enum SexStratifications
    {
        /// <summary>
        /// Indicates Sex Stratification is Female Only
        /// </summary>
        [EnumMember, Description("Female Only")]
        FemaleOnly = 1,
        /// <summary>
        /// Indicates Sex Stratification is Male Only
        /// </summary>
        [EnumMember, Description("Male Only")]
        MaleOnly = 2,
        /// <summary>
        ///Indicates Sex Stratification is Male and Female
        /// </summary>
        [EnumMember, Description("Male and Female")]
        MaleAndFemale = 3,
        /// <summary>
        /// Indicates Sex Stratification is Male and Female Aggregated
        /// </summary>
        [EnumMember, Description("Male and Female Aggregated")]
        MaleAndFemaleAggregated = 4,
        /// <summary>
        /// Indicates Sex Stratification is Ambiguous - individuals who are physically undifferentiated from birth.
        /// </summary>
        [EnumMember, Description("Ambiguous")]
        Ambiguous = 5,
        /// <summary>
        /// Indicates Sex Stratification is No Information.
        /// </summary>
        [EnumMember, Description("No Information")]
        NoInformation = 6,
        /// <summary>
        /// Indicates Sex Stratification is Unknown.
        /// </summary>
        [EnumMember, Description("Unknown")]
        Unknown = 7,
        /// <summary>
        /// Indicates Sex Stratification Other - individuals who are undergoing gender re-assignment.
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 8
    }
}
