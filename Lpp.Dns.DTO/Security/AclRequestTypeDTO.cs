using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request type ACL
    /// </summary>
    [DataContract]
    public class AclRequestTypeDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request Type
        /// </summary>
        [DataMember]
        public string RequestType { get; set; }
    }
}
