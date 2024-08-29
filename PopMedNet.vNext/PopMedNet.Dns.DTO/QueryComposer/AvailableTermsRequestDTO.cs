using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    [DataContract]
    public class AvailableTermsRequestDTO
    {
        /// <summary>
        /// Gets or sets the collection of Adapter ID's the terms must belong to.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Adapters { get; set; }
        /// <summary>
        /// Gets or sets the adapter detail value.
        /// </summary>
        [DataMember]
        public DTO.Enums.QueryComposerQueryTypes? QueryType { get; set; }
    }
}
