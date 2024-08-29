using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsTaskChange")]
    public class PmnTaskChangeLog : ChangeLog
    {
        public PmnTaskChangeLog()
        {
            EventID = EventIdentifiers.Task.Change.ID;
        }

        public Guid TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class PmnTaskChangeLogConfiguration : IEntityTypeConfiguration<PmnTaskChangeLog>
    {
        public void Configure(EntityTypeBuilder<PmnTaskChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.TaskID }).HasName("PK_LogsTaskChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
