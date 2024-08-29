using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization Group
    /// </summary>
    [DataContract]
    public class OrganizationGroupDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Gets or set the Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; } = string.Empty;
        /// <summary>
        /// Gets or set the ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
        /// <summary>
        /// Gets or set the Group
        /// </summary>
        [DataMember]
        public string Group { get; set; } = string.Empty;
    }
}
