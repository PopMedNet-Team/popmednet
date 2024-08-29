using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRegistryChange")]
    public class RegistryChangeLog : ChangeLog
    {
        public RegistryChangeLog()
        {
            this.EventID = EventIdentifiers.Registry.Change.ID;
        }

        public Guid RegistryID { get; set; }
        public virtual Registry? Registry { get; set; }
    }
    internal class RegistryChangeLogConfiguration : IEntityTypeConfiguration<RegistryChangeLog>
    {
        public void Configure(EntityTypeBuilder<RegistryChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RegistryID }).HasName("PK_LogsRegistryChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
