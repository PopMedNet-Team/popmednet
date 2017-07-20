using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Registry ACL
    /// </summary>
    [DataContract]
    public class AclRegistryDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
    }
}
