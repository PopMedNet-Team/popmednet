using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Project Organization Events
    /// </summary>
    [DataContract]
    public class ProjectOrganizationEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
    }
}
