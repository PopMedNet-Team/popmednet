using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// The results of a query.
    /// </summary>
    [DataContract]
    public class QueryComposerResponseQueryResultDTO
    {
        /// <summary>
        /// Gets or set the ID
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the query.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the time the query was started in UTC.
        /// </summary>
        [DataMember]
        public DateTimeOffset? QueryStart { get; set; }
        /// <summary>
        /// Gets or sets the time the query finished executing in UTC.
        /// </summary>
        [DataMember]
        public DateTimeOffset? QueryEnd { get; set; }
        /// <summary>
        /// Gets or sets the time the post-processing of the query result was started in UTC.
        /// </summary>
        [DataMember]
        public DateTimeOffset? PostProcessStart { get; set; }
        /// <summary>
        /// Gets or sets the time the post-processing of the query result was finished in UTC.
        /// </summary>
        [DataMember]
        public DateTimeOffset? PostProcessEnd { get; set; }
        /// <summary>
        /// Errors
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerResponseErrorDTO> Errors { get; set; }
        /// <summary>
        /// A collection of a collection of results (ie a collection of tables).
        /// </summary>
        [DataMember]
        public IEnumerable<IEnumerable<Dictionary<string, object>>> Results { get; set; }
        /// <summary>
        /// Lowcell Threshold
        /// </summary>
        [DataMember]
        public double? LowCellThrehold { get; set; }
        /// <summary>
        /// Gets the collection of property definitions that define the shape of the response.
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerResponsePropertyDefinitionDTO> Properties { get; set; }
        /// <summary>
        /// Gets the information about how to aggregrate the results.
        /// </summary>
        [DataMember]
        public QueryComposerResponseAggregationDefinitionDTO Aggregation { get; set; }
    }
}
