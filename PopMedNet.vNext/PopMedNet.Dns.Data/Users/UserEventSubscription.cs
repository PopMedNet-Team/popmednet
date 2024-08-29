using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using PopMedNet.Utilities.Logging;

namespace PopMedNet.Dns.Data
{
    [Table("UserEventSubscriptions")]
    public class UserEventSubscription : Entity, DTO.Subscriptions.ISubscription, DTO.Subscriptions.ISimpleScheduledSubscription, ISubscription
    {
        public DateTimeOffset? LastRunTime { get; set; }

        public DateTimeOffset? NextDueTime { get; set; }

        public DateTimeOffset? NextDueTimeForMy { get; set; }

        public Frequencies? Frequency { get; set; }

        public Frequencies? FrequencyForMy { get; set; }
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
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
                return (int?)Frequency;
            }
        }

        [NotMapped]
        int? ISubscription.FrequencyForMy
        {
            get
            {
                return (int?)FrequencyForMy;
            }
        }

    }

    internal class UserEventSubscriptionConfiguration : IEntityTypeConfiguration<UserEventSubscription>
    {
        public void Configure(EntityTypeBuilder<UserEventSubscription> builder)
        {
            builder.HasKey(e => new { e.UserID, e.EventID }).HasName("PK_dbo.UserEventSubscriptions").IsClustered(true);
            builder.HasIndex(evt => evt.LastRunTime, "IX_LastRunTime").IsUnique(false).IsClustered(false);
            builder.HasIndex(evt => evt.NextDueTime, "IX_NextDueTime").IsUnique(false).IsClustered(false);
            builder.HasIndex(evt => evt.NextDueTimeForMy, "IX_NextDueTimeForMy").IsUnique(false).IsClustered(false);
            builder.Property(p => p.Frequency).HasConversion<int>();
            builder.Property(p => p.FrequencyForMy).HasConversion<int>();
        }
    }
    public class UserEventSubscriptionMappingProfile : AutoMapper.Profile
    {
        public UserEventSubscriptionMappingProfile()
        {
            CreateMap<UserEventSubscription, DTO.UserEventSubscriptionDTO>();
        }
    }

}
