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
    public class ObserverEventDTO
    {
        /// <summary>
        /// The Event ID
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// The name of the event
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
