using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Base Event Permissions
    /// </summary>
    [DataContract]
    public class BaseEventPermissionDTO : EntityDto
    {
        /// <summary>
        /// ID of Security Group
        /// </summary>
        [DataMember]
        public Guid SecurityGroupID { get; set; }
        /// <summary>
        /// Security Group
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string SecurityGroup { get; set; }
        /// <summary>
        /// Determines that the Security Group is Allowed 
        /// </summary>
        [DataMember]
        public bool? Allowed { get; set; }
        /// <summary>
        /// Determines that the Security Group is Overridden
        /// </summary>
        [DataMember]
        public bool Overridden { get; set; }
        /// <summary>
        /// ID of an Event
        /// </summary>
        [DataMember]
        public Guid EventID { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [DataMember]
        public string Event { get; set; }
    }
}
