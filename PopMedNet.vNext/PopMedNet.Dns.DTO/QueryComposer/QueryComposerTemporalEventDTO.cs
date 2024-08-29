using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Temporal Events
    /// </summary>
    [DataContract]
    public class QueryComposerTemporalEventDTO
    {
        /// <summary>
        /// The Identifier that the Index Event should use.
        /// </summary>
        [DataMember]
        public string IndexEventDateIdentifier { get; set; }
        /// <summary>
        /// The number of days before the Index Event.
        /// </summary>
        [DataMember]
        public int DaysBefore { get; set; }
        /// <summary>
        /// The Number of days after the Index Event.
        /// </summary>
        [DataMember]
        public int DaysAfter { get; set; }
        [DataMember]
        public IEnumerable<QueryComposerCriteriaDTO> Criteria { get; set; }
    }
}
