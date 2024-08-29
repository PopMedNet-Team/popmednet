using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart ACL 
    /// </summary>
    [DataContract]
    public class AclDataMartDTO : AclDTO
    {
        /// <summary>
        /// ID of DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
