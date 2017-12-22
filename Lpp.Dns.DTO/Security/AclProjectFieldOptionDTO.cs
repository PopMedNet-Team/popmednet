using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Project Field Option ACL
    /// </summary>
    [DataContract]
    public class AclProjectFieldOptionDTO : BaseFieldOptionAclDTO
    {
        /// <summary>
        /// Gets or sets the ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
