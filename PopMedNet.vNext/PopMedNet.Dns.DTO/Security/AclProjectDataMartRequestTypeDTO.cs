using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMart Request Type ACL
    /// </summary>
    [DataContract]
    public class AclProjectDataMartRequestTypeDTO : AclDataMartRequestTypeDTO
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
    }
}
