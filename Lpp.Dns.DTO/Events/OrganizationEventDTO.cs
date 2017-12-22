using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Organization Events
    /// </summary>
    [DataContract]
    public class OrganizationEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID {get; set;}
    }
}
