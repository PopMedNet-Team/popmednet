using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Get change request DTO
    /// </summary>
    [DataContract]
    public class GetChangeRequestDTO
    {
        /// <summary>
        /// Last checked
        /// </summary>
        [DataMember]
        public DateTimeOffset LastChecked {get; set;}
        /// <summary>
        /// Provider id's
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> ProviderIDs { get; set; }
    }
}
