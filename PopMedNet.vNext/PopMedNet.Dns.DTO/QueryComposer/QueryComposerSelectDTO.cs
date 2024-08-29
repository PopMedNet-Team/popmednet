using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Select DTO
    /// </summary>
    [DataContract]
    public class QueryComposerSelectDTO {
        /// <summary>
        /// Available Fields
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerFieldDTO> Fields { get; set; }
    }
}