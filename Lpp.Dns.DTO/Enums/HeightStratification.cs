using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    [DataContract]
    public enum HeightStratification
    {
        /// <summary>
        /// Indicates 2 inch Group Stratification
        /// </summary>
        [EnumMember, Description("2 inch groups")]
        TwoInch = 2,
        /// <summary>
        /// Indicates 4 inch Group Stratification
        /// </summary>
        [EnumMember, Description("4 inch groups")]
        FourInch = 4,
    }
}
