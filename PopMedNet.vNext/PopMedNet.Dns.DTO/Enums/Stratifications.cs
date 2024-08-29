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
    /// Types of Stratifications
    /// </summary>
    [DataContract, Flags]
    public enum Stratifications
    {
        /// <summary>
        /// Indicates Stratification is None
        /// </summary>
        [EnumMember, Description("None")]
        None = 0,
        /// <summary>
        /// Indicates Stratification is Ethnicity
        /// </summary>
        [EnumMember, Description("Ethnicity")]
        Ethnicity = 1,
        /// <summary>
        /// Indicates Stratification is Age
        /// </summary>
        [EnumMember, Description("Age")]
        Age = 2,
        /// <summary>
        /// Indicates Stratification is Gender
        /// </summary>
        [EnumMember, Description("Gender")]
        Gender = 4,

        /// <summary>
        /// Indicates stratification by zip for a defined location.
        /// </summary>
        [EnumMember, Description("Location")]
        Location = 8
    }
}
