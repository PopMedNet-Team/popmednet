using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// QueryComposer Response AggregationDefinitionDTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponseAggregationDefinitionDTO : PopMedNet.Objects.Dynamic.IAggregationDefinition
    {
        /// <summary>
        /// Gets or sets the collection of property names to group by.
        /// </summary>
        [DataMember]
        public IEnumerable<string> GroupBy { get; set; }
        /// <summary>
        /// Gets or sets the collection of PropertyDefinitions defining the properties to select for the aggregation result.
        /// The definition will indicate if any aggregate action should be applied.
        /// </summary>
        [DataMember]
        public IEnumerable<PopMedNet.Objects.Dynamic.IPropertyDefinition> Select { get; set; }
        /// <summary>
        /// Stores the group name if any.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
