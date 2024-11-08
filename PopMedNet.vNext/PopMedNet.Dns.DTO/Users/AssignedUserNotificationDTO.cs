﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    [DataContract]
    public class AssignedUserNotificationDTO
    {
        /// <summary>
        /// The name of the event/notification
        /// </summary>
        [DataMember]
        public string? Event { get; set; }

        /// <summary>
        /// The event ID
        /// </summary>
        [DataMember]
        public Guid EventID { get; set; }

        /// <summary>
        /// The level at which this notification is set.
        /// </summary>
        [DataMember]
        public string? Level { get; set; }

        /// <summary>
        /// Th project, organization, datamart, or datamart within project details.
        /// </summary>
        [DataMember]
        public string? Description { get; set; }
    }
}
