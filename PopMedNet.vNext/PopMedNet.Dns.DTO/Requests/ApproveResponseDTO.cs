using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Approve Response
    /// </summary>
    [DataContract]
    public class ApproveResponseDTO
    {
        /// <summary>
        /// Response Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// Response ID's
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> ResponseIDs { get; set; }
    }
}
