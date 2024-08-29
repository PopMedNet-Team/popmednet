using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Types of field option permissions
    /// </summary>
    [DataContract, Flags]
    public enum FieldOptionPermissions
    {
        /// <summary>
        /// Indicates the FieldOption Permission should inherit it's value from Global.
        /// </summary>
        [EnumMember]
        Inherit = -1,
        /// <summary>
        /// Indicates FieldOption Permission is Optional
        /// </summary>
        [EnumMember]
        Optional = 0,
        /// <summary>
        /// Indicates FieldOption Permission is Required
        /// </summary>
        [EnumMember]
        Required = 1,
        /// <summary>
        /// Indicates FieldOption Permission is Hidden
        /// </summary>
        [EnumMember]
        Hidden = 2
    }
}
