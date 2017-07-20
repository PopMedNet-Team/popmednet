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
    /// Notification
    /// </summary>
    [DataContract]
    public class NotificationDTO
    {
        /// <summary>
        /// Gets or set the date time
        /// </summary>
        [DataMember]
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [DataMember]
        public string Event { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
