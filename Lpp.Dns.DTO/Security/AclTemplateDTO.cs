using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Template ACL
    /// </summary>
    [DataContract]
    public class AclTemplateDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of template
        /// </summary>
        [DataMember]
        public Guid TemplateID { get; set; }
    }
}
