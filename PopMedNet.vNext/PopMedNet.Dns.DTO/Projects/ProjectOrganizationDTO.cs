using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Organization
    /// </summary>
    [DataContract]
    public class ProjectOrganizationDTO : EntityDto
    {
        /// <summary>
        /// Gets or set the ID of project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Gets or sets the project
        /// </summary>
        [DataMember]
        public string Project { get; set; }
        /// <summary>
        /// Gets or set the ID of an organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Get or set the Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; }
    }
}
