using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Security Group with user
    /// </summary>
    [DataContract]
    public class SecurityGroupWithUsersDTO : SecurityGroupDTO
    {
        /// <summary>
        /// List of users
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Users { get; set; }
    }
}
