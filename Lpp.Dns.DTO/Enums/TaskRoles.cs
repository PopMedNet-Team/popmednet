using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Types of Task Roles
    /// </summary>
    [DataContract, Flags]
    public enum TaskRoles
    {
        /// <summary>
        /// Indicates the task role is a Worker
        /// </summary>
        [EnumMember]
        Worker = 1,
        /// <summary>
        /// Indicates the task role is a Supervisor
        /// </summary>
        [EnumMember]
        Supervisor = 2,
        /// <summary>
        /// Indicates the task role is an Administrator
        /// </summary>
        [EnumMember]
        Administrator = 4
    }
}
