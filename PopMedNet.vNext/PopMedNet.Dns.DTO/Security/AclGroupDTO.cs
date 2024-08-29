using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// ACL Group
    /// </summary>
    [DataContract]
    public class AclGroupDTO : AclDTO
    {
        /// <summary>
        /// ID of Group
        /// </summary>
        [DataMember]
        public Guid GroupID { get; set; }
    }
}
