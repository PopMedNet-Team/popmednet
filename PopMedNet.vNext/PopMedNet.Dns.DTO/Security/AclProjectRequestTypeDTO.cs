using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project Request type ACL
    /// </summary>
    [DataContract]
    public class AclProjectRequestTypeDTO : BaseAclRequestTypeDTO
    {
        /// <summary>
        /// Gets or sets the ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }

    }
}
