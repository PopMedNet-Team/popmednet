using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Network Message
    /// </summary>
    [DataContract]
    public class NetworkMessageDTO : EntityDtoWithID
    {
        /// <summary>
        /// Subject
        /// </summary>
        [DataMember]
        [Required(AllowEmptyStrings = true)]
        public string? Subject { get; set; }
        /// <summary>
        /// Message Text
        /// </summary>
         [DataMember]
        [Required]
        public string? MessageText { get; set; }
        /// <summary>
        /// The Date the network message was created on
        /// </summary>
         [DataMember]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Available Targets
        /// </summary>
        [DataMember]
        public IEnumerable<Guid>? Targets { get; set; }

    }
}
