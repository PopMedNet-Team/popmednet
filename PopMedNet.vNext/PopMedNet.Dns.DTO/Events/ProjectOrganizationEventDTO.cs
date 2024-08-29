using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Organization Events
    /// </summary>
    [DataContract]
    public class ProjectOrganizationEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// ID of an Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
    }
}
