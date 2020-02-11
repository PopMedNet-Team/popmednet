using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Sections in the query composer
    /// </summary>
    [DataContract]
    public enum QueryComposerSections
    {
        /// <summary>
        /// Indicates the criteria block of the query composer
        /// </summary>
        [EnumMember]
        Criteria = 0,

        /// <summary>
        /// Indicates the Statification block of the query composer
        /// </summary>
        [EnumMember]
        Stratification = 1,

        /// <summary>
        /// Indicates the Criteria for Temporal Events.
        /// </summary>
        [EnumMember]
        TemportalEvents = 2,
    }
}
