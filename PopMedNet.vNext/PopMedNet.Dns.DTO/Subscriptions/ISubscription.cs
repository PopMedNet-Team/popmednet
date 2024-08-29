namespace PopMedNet.Dns.DTO.Subscriptions
{
    /// <summary>
    /// Subscription
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Last run time
        /// </summary>
        DateTimeOffset? LastRunTime { get; set; }
        /// <summary>
        /// nect due time
        /// </summary>
        DateTimeOffset? NextDueTime { get; set; }
        /// <summary>
        /// Filters Definition xml
        /// </summary>
        string FiltersDefinitionXml { get; }
    }
}
