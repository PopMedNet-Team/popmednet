using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
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
