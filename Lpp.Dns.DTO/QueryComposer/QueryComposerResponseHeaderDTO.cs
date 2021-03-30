using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    [DataContract]
    public class QueryComposerResponseHeaderDTO
    {
        /// <summary>
        /// Gets or sets the ID of the response.
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the Request the response belongs to.
        /// </summary>
        [DataMember]
        public Guid? RequestID { get; set; }
        /// <summary>
        /// The id of the document the response is serialized to.
        /// </summary>
        [DataMember]
        public Guid? DocumentID { get; set; }
        /// <summary>
        /// Gets or sets the datetime the first query started executing.
        /// QueryingStart and QueryingEnd represent the entire duration of request execution.
        /// </summary>
        [DataMember]
        public DateTimeOffset? QueryingStart { get; set; }
        /// <summary>
        /// Gets or sets the datetime the last query finished executing or the queries post-processing finished, whichever is latest.
        /// QueryingStart and QueryingEnd represent the entire duration of request execution.
        /// </summary>
        [DataMember]
        public DateTimeOffset? QueryingEnd { get; set; }
        /// <summary>
        /// Gets or sets the name of the DataMart that generated the response.
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
    }
}
