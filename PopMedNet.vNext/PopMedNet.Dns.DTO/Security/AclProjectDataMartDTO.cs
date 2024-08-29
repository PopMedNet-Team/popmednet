using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Project DataMart ACL
    /// </summary>
    [DataContract]
    public class AclProjectDataMartDTO : AclDTO
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        [DataMember]
        public Guid ProjectID { get; set; }
        /// <summary>
        /// ID of the DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
