using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization Registry
    /// </summary>
    [DataContract]
    public class OrganizationRegistryDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Gets or set the Organization
        /// </summary>
        [DataMember]
        public string? Organization { get; set; }
        /// <summary>
        /// Gets or set the Acronym
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? Acronym { get; set; }
        /// <summary>
        /// Gets or set the parent organization
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string? OrganizationParent { get; set; }
        /// <summary>
        /// Gets or set the ID of registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
        /// <summary>
        /// Gets or set the Registry
        /// </summary>
        [DataMember]
        public string? Registry { get; set; }
        /// <summary>
        /// Gets or set the Description
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
        /// <summary>
        /// Gets or set the Registry types type
        /// </summary>
        [DataMember]
        public RegistryTypes Type { get; set; }
    }
}
