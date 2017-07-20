using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// ACL Group
    /// </summary>
    [DataContract]
    public class AclGroupDTO : AclDTO
    {
        /// <summary>
        /// ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
    }
}
