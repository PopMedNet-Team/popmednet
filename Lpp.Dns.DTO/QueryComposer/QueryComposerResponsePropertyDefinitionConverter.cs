using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
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
