using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DTO.Subscriptions
{
    /// <summary>
    /// Scheduled Subscription
    /// </summary>
    public interface ISimpleScheduledSubscription
    {
        /// <summary>
        /// Schedule kind
        /// </summary>
        Frequencies? ScheduleKind { get; }
    }
}
