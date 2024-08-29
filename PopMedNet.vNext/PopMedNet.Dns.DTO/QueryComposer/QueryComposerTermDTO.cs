using Newtonsoft.Json;
using PopMedNet.Dns.DTO.Enums;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Term
    /// </summary>
    [DataContract]
    public class QueryComposerTermDTO
    {
        /// <summary>
        /// Querycomposer Operators
        /// </summary>
        [DataMember]
        public QueryComposerOperators Operator { get; set; }
        /// <summary>
        /// The name of the type of the term. This should be a defined type.
        /// </summary>
        [DataMember]
        public Guid Type { get; set; }
        /// <summary>
        /// Term values
        /// </summary>
        [DataMember, JsonExtensionData]
        public Dictionary<string, object> Values { get; set; }

        [DataMember]
        public IEnumerable<QueryComposerCriteriaDTO> Criteria { get; set; }

        /// <summary>
        /// The key design elements for the term
        /// </summary>
        [DataMember]
        public DesignDTO Design { get; set; }
    }
}
