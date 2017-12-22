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
    /// Types of Supported Data Models
    /// </summary>
    [DataContract]
    public enum SupportedDataModels
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember, Description("None")]
        None = 1,
        /// <summary>
        /// ESP
        /// </summary>
        [EnumMember, Description("ESP")]
        ESP = 2,
        /// <summary>
        /// HMORN VDW
        /// </summary>
        [EnumMember, Description("HMORN VDW")]
        HMORN_VDW = 3,
        /// <summary>
        /// MSCDM
        /// </summary>
        [EnumMember, Description("MSCDM")]
        MSCDM = 4,
        /// <summary>
        /// I2b2
        /// </summary>
        [EnumMember, Description("I2b2")]
        I2b2 = 5,
        /// <summary>
        /// OMOP
        /// </summary>
        [EnumMember, Description("OMOP")]
        OMOP = 6,
        /// <summary>
        /// Other
        /// </summary>
        [EnumMember, Description("Other")]
        Other = 7,
        /// <summary>
        /// PCORnet
        /// </summary>
        [EnumMember, Description("PCORnet CDM")]
        PCORI = 8,
    }
}
