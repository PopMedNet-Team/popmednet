using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("RequestObservers")]
    public class RequestObserver : EntityWithID
    {
        public RequestObserver()
        {
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public Guid? UserID { get; set; }
        public virtual User? User { get; set; }

        public Guid? SecurityGroupID { get; set; }
        public virtual SecurityGroup? SecurityGroup { get; set; }

        [MaxLength(150)]
        public string? DisplayName { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        public virtual ICollection<RequestObserverEventSubscription> EventSubscriptions { get; set; } = new HashSet<RequestObserverEventSubscription>();
    }

    internal class RequestObserverConfiguration : IEntityTypeConfiguration<RequestObserver>
    {
        public void Configure(EntityTypeBuilder<RequestObserver> builder)
        {
            builder.HasMany(t => t.EventSubscriptions)
                .WithOne(t => t.RequestObserver)
                .IsRequired(true)
                .HasForeignKey(t => t.RequestObserverID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
