using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// User ACL
    /// </summary>
    [DataContract]
    public class AclUserDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
    }
}
