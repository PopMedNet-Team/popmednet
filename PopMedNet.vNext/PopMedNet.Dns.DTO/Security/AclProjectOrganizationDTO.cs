using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Organization ACL
    /// </summary>
    [DataContract]
    public class AclProjectOrganizationDTO : AclDTO
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
