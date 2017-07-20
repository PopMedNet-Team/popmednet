using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User ACL
    /// </summary>
    [DataContract]
    public class AclUserDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
    }
}
