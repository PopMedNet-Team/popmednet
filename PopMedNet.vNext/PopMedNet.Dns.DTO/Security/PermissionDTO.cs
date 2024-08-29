using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Permission
    /// </summary>
    [DataContract]
    public class PermissionDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(250)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Available Locations
        /// </summary>

        [DataMember]
        public IEnumerable<PermissionAclTypes> Locations { get; set; }
    }
}
