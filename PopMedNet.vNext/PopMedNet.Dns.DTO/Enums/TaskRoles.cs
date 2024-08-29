using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
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
