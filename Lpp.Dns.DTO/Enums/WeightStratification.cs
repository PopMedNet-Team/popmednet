using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    [DataContract]
    public enum WeightStratification
    {
        /// <summary>
        /// Indicates 2 inch Group Stratification
        /// </summary>
        [EnumMember, Description("10 lb Groups")]
        TenLbs = 10,
        /// <summary>
        /// Indicates 2 inch Group Stratification
        /// </summary>
        [EnumMember, Description("20 lb Groups")]
        TwentyLbs = 20,
    }
}
