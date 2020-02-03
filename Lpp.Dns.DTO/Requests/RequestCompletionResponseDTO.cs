using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request completion Response
    /// </summary>
    [DataContract]
    public class RequestCompletionResponseDTO : ICompletionResponse<RequestDTO>
    {
        /// <summary>
        /// Url
        /// </summary>
        [DataMember]
        public string Uri { get; set; }
        /// <summary>
        /// Gets or sets the entities
        /// </summary>
        [DataMember]
        public RequestDTO Entity { get; set; }
        /// <summary>
        /// Available DataMarts
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDataMartDTO> DataMarts { get; set; }
    }
}
