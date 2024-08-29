using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization Events
    /// </summary>
    [DataContract]
    public class OrganizationEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID {get; set;}
    }
}
