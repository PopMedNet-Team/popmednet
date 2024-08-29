using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Utilities.Logging;

namespace PopMedNet.Dns.Data
{
    [Table("RequestObserverEventSubscriptions")]
    public class RequestObserverEventSubscription : Entity, PopMedNet.Dns.DTO.Subscriptions.ISubscription, PopMedNet.Dns.DTO.Subscriptions.ISimpleScheduledSubscription, PopMedNet.Utilities.Logging.ISubscription
    {
        public DateTimeOffset? LastRunTime { get; set; }

        public DateTimeOffset? NextDueTime { get; set; }

        public Frequencies Frequency { get; set; }
        public Guid RequestObserverID { get; set; }
        public virtual RequestObserver? RequestObserver { get; set; }
        [NotMapped]
        Guid ISubscription.UserID { get { return RequestObserverID; } }
        public Guid EventID { get; set; }
        public virtual Event? Event { get; set; }

        [NotMapped]
        Frequencies? DTO.Subscriptions.ISimpleScheduledSubscription.ScheduleKind
        {
            get { return Frequency; }
        }

        [NotMapped]
        string DTO.Subscriptions.ISubscription.FiltersDefinitionXml
        {
            get { return string.Format(@"<Q><Kind Id=""{0}"" /></Q>", this.EventID); }
        }

        [NotMapped]
        int? ISubscription.Frequency
        {
            get
            {
                return (int)Frequency;
            }
        }

        [NotMapped]
        DateTimeOffset? ISubscription.NextDueTimeForMy
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        [NotMapped]
        int? ISubscription.FrequencyForMy
        {
            get
            {
                return null;
            }
        }
    }

    internal class RequestObserverEventSubscriptionConfiguration : IEntityTypeConfiguration<RequestObserverEventSubscription>
    {
        public void Configure(EntityTypeBuilder<RequestObserverEventSubscription> builder)
        {
            builder.HasKey(e => new { e.RequestObserverID, e.EventID }).HasName("PK_dbo.RequestObserverEventSubscriptions").IsClustered(true);
            builder.HasIndex(e => e.LastRunTime, "IX_LastRunTime").IsClustered(false).IsUnique(false);
            builder.HasIndex(e => e.NextDueTime, "IX_NextDueTime").IsClustered(false).IsUnique(false);

            builder.Property(e => e.Frequency).HasConversion<int>();
        }
    }
}
