using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.DTO.Subscriptions
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
