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
    /// Types of Templates
    /// </summary>
    [DataContract]
    public enum TemplateTypes
    {
        /// <summary>
        /// Request Template
        /// </summary>
        [EnumMember, Description("Request Template")]
        Request = 1,
        /// <summary>
        /// Criteria Group
        /// </summary>
        [EnumMember, Description("Criteria Group")]
        CriteriaGroup = 2
    }
}
