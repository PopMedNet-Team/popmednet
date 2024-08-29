using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// This class supports the where clause for Query composer
    /// </summary>
    [DataContract]
    public class QueryComposerWhereDTO
    {
        /// <summary>
        /// Query composer criteria
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerCriteriaDTO> Criteria { get; set; }
    }
}
