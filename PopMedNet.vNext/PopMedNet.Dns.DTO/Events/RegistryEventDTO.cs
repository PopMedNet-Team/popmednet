using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Registry Events
    /// </summary>
    [DataContract]
    public class RegistryEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// ID of the Registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
    }
}
