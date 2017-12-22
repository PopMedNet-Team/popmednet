using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Security Entities
    /// </summary>
    [DataContract]
    public class SecurityEntityDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Security Entities types
        /// </summary>
        [DataMember]
        public SecurityEntityTypes Type { get; set; }
    }
}
