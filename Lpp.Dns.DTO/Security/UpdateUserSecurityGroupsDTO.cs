using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User Security Groups update
    /// </summary>
    [DataContract]
    public class UpdateUserSecurityGroupsDTO
    {
        /// <summary>
        /// Gets or sets the ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// list of Groups
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityGroupDTO> Groups { get; set; }
    }
}
