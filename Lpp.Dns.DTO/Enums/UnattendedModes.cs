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
    /// Types of Unattended modes
    /// </summary>
    [DataContract]
    public enum UnattendedModes
    {
        /// <summary>
        /// No Unattended Operation
        /// </summary>
        [EnumMember, Description("No Unattended Operation")]
        NoUnattendedOperation = 0,
        /// <summary>
        /// Notify Only
        /// </summary>
        [EnumMember, Description("Notify Only")]
        NotifyOnly = 1,
        /// <summary>
        /// Process but No Upload
        /// </summary>
        [EnumMember, Description("Process; No Upload")]
        ProcessNoUpload = 2,
        /// <summary>
        /// Process and Upload
        /// </summary>
        [EnumMember, Description("Process And Upload")]
        ProcessAndUpload = 3
    }
}
