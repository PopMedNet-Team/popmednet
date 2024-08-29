using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
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
