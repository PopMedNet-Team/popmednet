using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Querycomposer Response DTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponseDTO
    {
        /// <summary>
        /// Gets or set the ID
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Gets or sets the response time
         /// </summary>
        [DataMember]
        public DateTimeOffset ResponseDateTime { get; set; }
        /// <summary>
        /// Gets or sets the ID of request
         /// </summary>
        [DataMember]
         public Guid RequestID { get; set; }
        /// <summary>
        /// Errors
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerResponseErrorDTO> Errors { get; set; }
        /// <summary>
        /// A collection of a collection of results (ie a collection of tables).
        /// </summary>
        [DataMember]
        public IEnumerable<IEnumerable<Dictionary<string,object>>> Results { get; set; }
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
    /// <summary>
    /// QueryComposer Response AggregationDefinitionDTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponseAggregationDefinitionDTO : Lpp.Objects.Dynamic.IAggregationDefinition
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
        public IEnumerable<Lpp.Objects.Dynamic.IPropertyDefinition> Select { get; set; }
        /// <summary>
        /// Stores the group name if any.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
    /// <summary>
    /// QueryComposer Response PropertyDefinitionDTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponsePropertyDefinitionDTO : Lpp.Objects.Dynamic.IPropertyDefinition
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// The name of the type of the property. This should be a defined type.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        string _as = null;
        /// <summary>
        /// The name of the property to be used in a select or aggregation.
        /// </summary>
        [DataMember]
        public string As
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_as))
                    return this.Name;

                return _as;
            }
            set
            {
                _as = value;
            }
        }
        /// <summary>
        /// The name of an applicable aggregation to apply when aggregating the results. (Sum, Average, Count, etc.)
        /// </summary>
        [DataMember]
        public string Aggregate { get; set; }

        /// <summary>
        /// Returns the Type property as a System.Type.
        /// </summary>
        /// <returns></returns>
        public Type AsType()
        {
            return System.Type.GetType(this.Type);
        }
    }

    /// <summary>
    /// Custom json converter to handle converting IPropertyDefinition interface to concrete type.
    /// </summary>
    public class QueryComposerResponsePropertyDefinitionConverter : Newtonsoft.Json.JsonConverter
    {
        /// <summary>
        /// retuns object type
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Lpp.Objects.Dynamic.IPropertyDefinition);
        }
        /// <summary>
        /// reads the json object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return serializer.Deserialize<QueryComposerResponsePropertyDefinitionDTO>(reader);
        }
        /// <summary>
        /// create a json object
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
