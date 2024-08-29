using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Field Option ACL
    /// </summary>
    [DataContract]
    public class AclProjectFieldOptionDTO : BaseFieldOptionAclDTO
    {
        /// <summary>
        /// Gets or sets the ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
