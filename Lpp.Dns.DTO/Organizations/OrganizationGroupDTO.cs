using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Organization Group
    /// </summary>
    [DataContract]
    public class OrganizationGroupDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Gets or set the Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; }
        /// <summary>
        /// Gets or set the ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
        /// <summary>
        /// Gets or set the Group
        /// </summary>
        [DataMember]
        public string Group { get; set; }
    }
}
