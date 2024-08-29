using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
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
