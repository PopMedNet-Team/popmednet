using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Forgot Password
    /// </summary>
    [DataContract]
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// The Name of the User
        /// </summary>
        [DataMember, MaxLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// User Email address
        /// </summary>
        [DataMember, EmailAddress, MaxLength(255)]
        public string Email { get; set; }
    }
}
