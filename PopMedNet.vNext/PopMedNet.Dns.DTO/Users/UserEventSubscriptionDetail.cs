namespace PopMedNet.Dns.DTO.Users
{
    /// <summary>
    /// User Event Subscription Detail
    /// </summary>
    public class UserEventSubscriptionDetail
    {
        /// <summary>
        /// Gets or sets the ID of the user
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the Event
        /// </summary>
        public Guid EventID { get; set; }
        /// <summary>
        /// Event last run time
        /// </summary>
        public DateTimeOffset? LastRunTime { get; set; }
        /// <summary>
        /// Event next due time
        /// </summary>
        public DateTimeOffset? NextDueTime { get; set; }
        ///<summary>
        /// My Event next due time
        /// </summary>
        public DateTimeOffset? NextDueTimeForMy { get; set; }
        /// <summary>
        /// Gets or sets the frequency
        /// </summary>
        public int? Frequency { get; set; }
        /// <summary>
        /// Gets or sets the frequency for My notifications
        /// </summary>
        public int? FrequencyForMy { get; set; }
        /// <summary>
        /// Name of the User
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// user Phone number
        /// </summary>
        public string Phone { get; set; }
    }
}
