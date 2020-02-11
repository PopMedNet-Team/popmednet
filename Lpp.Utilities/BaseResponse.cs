using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lpp.Utilities
{
    [DataContract]
    public class BaseResponse<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public ResponseError[] errors { get; set; }

        /// <summary>
        /// Returns all errors as a string
        /// </summary>
        /// <returns></returns>
        public string ReturnErrorsAsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var err in errors)
            {
                var ErrorMessage = err.Description;
                if (err.ErrorNumber != null)
                    ErrorMessage = +err.ErrorNumber + ": ";
                if (!string.IsNullOrWhiteSpace(err.ErrorType))
                    ErrorMessage += "; Type: " + err.ErrorType;

                sb.AppendLine(ErrorMessage);
            }

            return sb.ToString();
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public T[] results { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), DataMember]
        public Int64? InlineCount { get; set; }

        [XmlIgnore]
        public string RawResult { get; set; }
        [XmlIgnore]
        public string RequestUrl { get; set; }
    }

    [DataContract]
    public class ResponseError
    {
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorNumber { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorType { get; set; }
        [DataMember, JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Property { get; set; }
    }
}
