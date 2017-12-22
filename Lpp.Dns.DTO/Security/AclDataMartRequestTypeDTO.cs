using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DataMart Request type ACL
    /// </summary>
    [DataContract]
    public class AclDataMartRequestTypeDTO : BaseAclRequestTypeDTO
    {
        /// <summary>
        /// ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
