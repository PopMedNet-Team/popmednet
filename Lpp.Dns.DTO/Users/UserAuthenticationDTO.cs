using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User Authentication Summary
    /// </summary>
    [DataContract]
    public class UserAuthenticationDTO
    {
        /// <summary>
        /// The ID of the Authentication Item
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// The ID of the User
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// If the Authentication was successful or failure
        /// </summary>
        [DataMember]
        public bool Success { get; set; }
        /// <summary>
        /// The Description of the Authentication event
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// The IP Address that originated the Request
        /// </summary>
        [DataMember]
        public string IPAddress { get; set; }
        /// <summary>
        /// The Enviorment that triggered the Authentication
        /// </summary>
        [DataMember]
        public string Enviorment { get; set; }
        /// <summary>
        /// The Time the Authentication was triggered
        /// </summary>
        [DataMember]
        public DateTimeOffset DateTime { get; set; }
    }
}
