using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("Events")]
    public class Event : EntityWithID
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool SupportsMyNotifications { get; set; }

        public virtual ICollection<EventLocation> Locations { get; set; } = new HashSet<EventLocation>();

        public virtual ICollection<DataMartEvent> DataMartEvents { get; set; } = new HashSet<DataMartEvent>();
        public virtual ICollection<GlobalEvent> Events { get; set; } = new HashSet<GlobalEvent>();
        public virtual ICollection<GroupEvent> GroupEvents { get; set; } = new HashSet<GroupEvent>();
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; } = new HashSet<OrganizationEvent>();
        public virtual ICollection<ProjectDataMartEvent> ProjectDataMartEvents { get; set; } = new HashSet<ProjectDataMartEvent>();
        public virtual ICollection<ProjectEvent> ProjectEvents { get; set; } = new HashSet<ProjectEvent>();
        public virtual ICollection<ProjectOrganizationEvent> ProjectOrganizationEvents { get; set; } = new HashSet<ProjectOrganizationEvent>();
        public virtual ICollection<RegistryEvent> RegistryEvents { get; set; } = new HashSet<RegistryEvent>();
        public virtual ICollection<UserEvent> UserEvents { get; set; } = new HashSet<UserEvent>();
        public virtual ICollection<UserEventSubscription> Subscriptions { get; set; } = new HashSet<UserEventSubscription>();
    }

    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasIndex(e => e.Name).IsClustered(false).IsUnique(false);

            builder.HasMany(t => t.DataMartEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Events)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.GroupEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.OrganizationEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectDataMartEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectOrganizationEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RegistryEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.UserEvents)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Locations)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Subscriptions)
                .WithOne(t => t.Event)
                .IsRequired(true)
                .HasForeignKey(t => t.EventID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class EventSecurityConfiguration : DnsEntitySecurityConfiguration<Event>
    {
        public override IQueryable<Event> SecureList(DataContext db, IQueryable<Event> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return query; //This is a special case because they are always allowed to be listed.
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Event[] objs)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return Task.Run(() => false);
        }
    }

    public class EventMappingProfile : AutoMapper.Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, DTO.EventDTO>()
                .ForMember(d => d.Locations, opt => opt.MapFrom(src => src.Locations.Select(l => l.Location).AsEnumerable()));
        }
    }
}
