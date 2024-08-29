using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request type ACL
    /// </summary>
    [DataContract]
    public class AclRequestTypeDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of Request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Request Type
        /// </summary>
        [DataMember]
        public string? RequestType { get; set; }
    }
}
