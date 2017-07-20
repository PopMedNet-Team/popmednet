using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Security Groups
    /// </summary>
    [DataContract]
    public enum SecurityGroupTypes
    {
        /// <summary>
        /// Indicates SecurityGroup Type is Organization
        /// </summary>
        [EnumMember]
        Organization = 1,
        /// <summary>
        /// Indicates SecurityGroup Type is Project
        /// </summary>
        [EnumMember]
        Project = 2
    }
}
