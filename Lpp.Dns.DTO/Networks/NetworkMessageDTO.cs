using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
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
        public string Subject { get; set; }
        /// <summary>
        /// Message Text
        /// </summary>
         [DataMember]
        [Required]
        public string MessageText { get; set; }
        /// <summary>
        /// The Date the network message was created on
        /// </summary>
         [DataMember]
        public DateTimeOffset CreatedOn { get; set; }
        /// <summary>
        /// Available Targets
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> Targets { get; set; }

    }
}
