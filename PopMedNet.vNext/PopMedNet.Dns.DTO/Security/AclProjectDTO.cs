using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project ACL
    /// </summary>
    [DataContract]
    public class AclProjectDTO : AclDTO
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
