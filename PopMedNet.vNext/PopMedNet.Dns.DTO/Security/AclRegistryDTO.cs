using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Registry ACL
    /// </summary>
    [DataContract]
    public class AclRegistryDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
    }
}
