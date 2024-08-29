using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Header
    /// </summary>
    [DataContract]
    public class QueryComposerQueryHeaderDTO : QueryComposerHeaderDTO
    {
        /// <summary>
        /// Gets or sets the sub type of the request.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerQueryTypes? QueryType { get; set; }
        /// <summary>
        /// Gets or sets the type of composition interface to use for the query.
        /// </summary>
        [DataMember]
        public Enums.QueryComposerInterface? ComposerInterface { get; set; }
    }
}
