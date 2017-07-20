using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Base ACL Request type
    /// </summary>
    [DataContract]
    public abstract class BaseAclRequestTypeDTO : BaseAclDTO
    {
        /// <summary>
        /// Gets or sets the ID of Request Type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request Type permissions
        /// </summary>
                [DataMember]
        public Enums.RequestTypePermissions? Permission { get; set; }
    }
}
