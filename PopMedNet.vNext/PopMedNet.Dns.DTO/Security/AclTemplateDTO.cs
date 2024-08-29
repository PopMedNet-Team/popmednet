using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Template ACL
    /// </summary>
    [DataContract]
    public class AclTemplateDTO : AclDTO
    {
        /// <summary>
        /// Gets or set the ID of template
        /// </summary>
        [DataMember]
        public Guid TemplateID { get; set; }
    }
}
