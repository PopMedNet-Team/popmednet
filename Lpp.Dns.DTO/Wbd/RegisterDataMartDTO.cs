using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Register DataMart
    /// </summary>
    [DataContract]
    public class RegisterDataMartDTO
    {
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        [DataMember]
                public string Token { get; set; }
    }
}
