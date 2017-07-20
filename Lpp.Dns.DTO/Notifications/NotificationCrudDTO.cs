using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Notification crud
    /// </summary>
    [DataContract]
    public class NotificationCrudDTO
    {
        /// <summary>
        /// Object id
        /// </summary>
        [DataMember]
        public Guid ObjectID { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [DataMember]
        public ObjectStates State { get; set; }
    }
}
