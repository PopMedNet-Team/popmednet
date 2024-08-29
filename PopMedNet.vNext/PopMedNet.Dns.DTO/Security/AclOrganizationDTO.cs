using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization ACL
    /// </summary>
    [DataContract]
    public class AclOrganizationDTO : AclDTO
    {
        /// <summary>
        /// ID of Organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
    }
}
