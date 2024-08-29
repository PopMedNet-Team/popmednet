using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("Events")]
    public class Event : EntityWithID
    {
        [Required, MaxLength(255), Index]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool SupportsMyNotifications { get; set; }

        public virtual ICollection<EventLocation> Locations { get; set; }

        public virtual ICollection<DataMartEvent> DataMartEvents { get; set; }
        public virtual ICollection<GlobalEvent> Events { get; set; }
        public virtual ICollection<GroupEvent> GroupEvents { get; set; }
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual ICollection<ProjectDataMartEvent> ProjectDataMartEvents { get; set; }
        public virtual ICollection<ProjectEvent> ProjectEvents { get; set; }
        public virtual ICollection<ProjectOrganizationEvent> ProjectOrganizationEvents { get; set; }
        public virtual ICollection<RegistryEvent> RegistryEvents { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<UserEventSubscription> Subscriptions { get; set; }
    }

    internal class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration()
        {
            HasMany(t => t.DataMartEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.Events).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.GroupEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.OrganizationEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectDataMartEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.ProjectOrganizationEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.RegistryEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.UserEvents).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);

            HasMany(t => t.Locations).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
            HasMany(t => t.Subscriptions).WithRequired(t => t.Event).HasForeignKey(t => t.EventID).WillCascadeOnDelete(true);
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
    internal class EventDTOMappingConfiguration : EntityMappingConfiguration<Event, EventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<Event, EventDTO>> MapExpression
        {
            get
            {
                return (e) => new EventDTO
                {
                    ID = e.ID,
                    Description = e.Description,
                    Name = e.Name,
                    Timestamp = e.Timestamp,
                    Locations = e.Locations.Select(l => l.Location),
                    SupportsMyNotifications = e.SupportsMyNotifications
                };
            }
        }
    }
}
