using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Task types
    /// </summary>
    [DataContract, Flags]
    public enum TaskTypes
    {
        /// <summary>
        /// Task
        /// </summary>
        [EnumMember]
        Task = 1,
        /// <summary>
        /// Appointment
        /// </summary>
        [EnumMember]
        Appointment = 2,
        /// <summary>
        /// Indicates the task type is Project
        /// </summary>
        [EnumMember]
        Project = 4,
        /// <summary>
        /// Indicates the task type is New User Registration
        /// </summary>
        [EnumMember, Description("New User Registration")]
        NewUserRegistration = 8
    }
}
