using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("EventLocations")]
    public class EventLocation : Entity
    {
        public EventLocation() { }

        public Guid EventID { get; set; }
        public Event? Event { get; set; }
        public PermissionAclTypes Location { get; set; }
    }
    internal class EventLocationConfiguration : IEntityTypeConfiguration<EventLocation>
    {
        public void Configure(EntityTypeBuilder<EventLocation> builder)
        {
            builder.HasKey(e => new { e.EventID, e.Location }).HasName("PK_dbo.EventLocations");
            builder.Property(e => e.Location).HasConversion<int>();

        }
    }
}
