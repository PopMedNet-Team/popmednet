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
    /// Types of Tobacco uses
    /// </summary>
    [DataContract]
    public enum TobaccoUses
    {
        /// <summary>
        /// Current
        /// </summary>
        [EnumMember, Description("Current")]
        Current = 1,
        /// <summary>
        /// Former
        /// </summary>
        [EnumMember, Description("Former")]
        Former = 2,
        /// <summary>
        /// Never
        /// </summary>
        [EnumMember, Description("Never")]
        Never = 3,
        /// <summary>
        /// Passive
        /// </summary>
        [EnumMember, Description("Passive")]
        Passive = 4,
        /// <summary>
        /// Not Available
        /// </summary>
        [EnumMember, Description("Not Available")]
        NotAvailable = 5
    }
}