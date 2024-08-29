using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Organization update
    /// </summary>
    [DataContract]
    public class ProjectOrganizationUpdateDTO 
    {
        /// <summary>
        /// Gets or sets the ID of Project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// Available Organizations in Project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectOrganizationDTO> Organizations { get; set; }
    }
}
