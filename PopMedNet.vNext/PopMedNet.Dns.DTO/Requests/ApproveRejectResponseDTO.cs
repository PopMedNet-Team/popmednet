using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Approve Reject Response
    /// </summary>
    [DataContract]
    public class ApproveRejectResponseDTO
    {
        /// <summary>
        /// Gets or sets the ID of Response
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
    }
}
