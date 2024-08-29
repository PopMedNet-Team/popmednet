using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Events
    /// </summary>
    [DataContract]
    public class EventDTO : EntityDtoWithID
    {
        /// <summary>
        /// Event Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Event Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Available locations
        /// </summary>
        [DataMember]
        public IEnumerable<PermissionAclTypes> Locations { get; set; }
        /// <summary>
        /// Supports My Notifications
        /// </summary>
        [DataMember]
        public bool SupportsMyNotifications { get; set; }

    }
}
