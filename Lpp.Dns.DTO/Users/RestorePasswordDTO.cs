using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Restore Password
    /// </summary>
    [DataContract]
    public class RestorePasswordDTO
    {
        /// <summary>
        /// Gets or sets the password restore tokens
        /// </summary>
        [DataMember]
        public Guid PasswordRestoreToken { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
