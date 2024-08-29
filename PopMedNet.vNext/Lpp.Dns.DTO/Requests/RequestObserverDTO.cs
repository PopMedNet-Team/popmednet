﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Lpp.Objects;
using Lpp.Dns.DTO.Enums;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User Request Observer Event Subscription
    /// </summary>
    [DataContract]
    public class RequestObserverDTO : EntityDtoWithID
    {
        /// <summary>
        /// RequestID
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [DataMember]
        public Guid? UserID { get; set; }

        /// <summary>
        /// SecurityGroupID
        /// </summary>
        [DataMember]
        public Guid? SecurityGroupID { get; set; }

        /// <summary>
        /// DisplayName
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        [MaxLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// The event subscriptions for this Observer
        /// </summary>
        [DataMember]
        public IEnumerable<RequestObserverEventSubscriptionDTO> EventSubscriptions { get; set; }
    }
}
