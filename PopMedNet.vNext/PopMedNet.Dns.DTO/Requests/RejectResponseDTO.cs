using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Reject the Response
    /// </summary>
    [DataContract]
    public class RejectResponseDTO
    {
        /// <summary>
        /// Message
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
