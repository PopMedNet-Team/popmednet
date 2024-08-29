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
        /// Types of Coverages
        /// </summary>
    [DataContract]
    public enum Coverages
    {
        /// <summary>
        /// Drug and Medical Coverage
        /// </summary>
        [EnumMember, Description("Drug and Medical Coverage")]
        DRUG_MED = 1,
        /// <summary>
        /// Drug Coverage Only
        /// </summary>
        [EnumMember, Description("Drug Coverage Only")]
        DRUG = 2,
        /// <summary>
        /// Medical Coverage Only
        /// </summary>
        [EnumMember, Description("Medical Coverage Only")]
        MED = 3,
        /// <summary>
        /// All Members
        /// </summary>
        [EnumMember, Description("All Members")]
        ALL = 4
    }
}
