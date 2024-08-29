using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PopMedNet.Dns.DTO.Enums
{
    [DataContract]
    public enum PeriodStratification
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember, Description("Monthly")]
        Monthly = 1,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember, Description("Yearly")]
        Yearly = 2,
    }
}
