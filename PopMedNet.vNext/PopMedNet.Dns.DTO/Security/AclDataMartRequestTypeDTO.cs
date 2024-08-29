using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart Request type ACL
    /// </summary>
    [DataContract]
    public class AclDataMartRequestTypeDTO : BaseAclRequestTypeDTO
    {
        /// <summary>
        /// ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
