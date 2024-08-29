using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Request Completion Request
    /// </summary>
    [DataContract]
    public class RequestCompletionRequestDTO : ICompletionRequest<RequestDTO>
    {
        /// <summary>
        /// Gets or sets the ID of Demand Activity result
        /// </summary>
        [DataMember]
        public Guid? DemandActivityResultID { get; set; }
        /// <summary>
        /// Gets or sets the Dto
        /// </summary>
        [DataMember]
        public RequestDTO Dto { get; set; }
        /// <summary>
        /// Available Datamarts
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDataMartDTO> DataMarts { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        [DataMember]
        public string Data { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
    }
}
