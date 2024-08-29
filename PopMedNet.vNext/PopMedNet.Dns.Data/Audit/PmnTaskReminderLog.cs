using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsTaskReminder")]
    public class PmnTaskReminderLog : AuditLog
    {
        public PmnTaskReminderLog()
        {
            this.EventID = EventIdentifiers.Task.WorkflowTaskReminder.ID;
        }
        public Guid TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class PmnTaskReminderLogConfiguration : IEntityTypeConfiguration<PmnTaskReminderLog>
    {
        public void Configure(EntityTypeBuilder<PmnTaskReminderLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.TaskID }).HasName("PK_LogsTaskReminder");
        }
    }
}
