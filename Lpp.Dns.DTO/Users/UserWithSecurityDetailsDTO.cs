using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User with security details
    /// </summary>
    [DataContract]
    public class UserWithSecurityDetailsDTO : UserDTO
    {
        /// <summary>
        /// Password Hash
        /// </summary>
        [DataMember]
        public string PasswordHash { get; set; }
    }
}
