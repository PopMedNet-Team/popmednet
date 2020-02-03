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
    /// Types of Task Statuses
    /// </summary>
    [DataContract]
    public enum TaskStatuses
    {
        /// <summary>
        /// Indicates the task status is Cancelled
        /// </summary>
        [EnumMember]
        Cancelled = 0,
        /// <summary>
        /// Indicates the task status is Not Started
        /// </summary>
        [EnumMember, Description("Not Started")]
        NotStarted = 1,
        /// <summary>
        /// Indicates the task status is In Progress
        /// </summary>
        [EnumMember, Description("In Progress")]
        InProgress = 2,
        /// <summary>
        /// Indicates the task status is Deferred
        /// </summary>
        [EnumMember]
        Deferred = 3,
        /// <summary>
        /// Indicates the task status is Blocked
        /// </summary>
        [EnumMember]
        Blocked = 4,
        /// <summary>
        /// Indicates the task status is Complete
        /// </summary>
        [EnumMember]
        Complete = 5

    }
}
