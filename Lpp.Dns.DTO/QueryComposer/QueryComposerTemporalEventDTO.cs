using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
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
