using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Organization ACL
    /// </summary>
    [DataContract]
    public class AclOrganizationDTO : AclDTO
    {
        /// <summary>
        /// ID of Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
    }
}
