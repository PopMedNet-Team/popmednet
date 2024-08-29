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
    /// Types of Output Criteria
    /// </summary>
    [DataContract]
    public enum OutputCriteria
    {
        /// <summary>
        ///Indicates Output criteria is 5
        /// </summary>
        [EnumMember, Description("5")]
        Top5 = 5,
        /// <summary>
        /// Indicates Output criteria is 10
        /// </summary>
        [EnumMember, Description("10")]
        Top10 = 10,
        /// <summary>
        ///Indicates Output criteria is 20
        /// </summary>
        [EnumMember, Description("20")]
        Top20 = 20,
        /// <summary>
        /// Indicates Output criteria is 25
        /// </summary>
        [EnumMember, Description("25")]
        Top25 = 25,
        /// <summary>
        /// Indicates Output criteria is 50
        /// </summary>
        [EnumMember, Description("50")]
        Top50 = 50,
        /// <summary>
        /// Indicates Output criteria is 100
        /// </summary>
        [EnumMember, Description("100")]
        Top100 = 100
    }
}
