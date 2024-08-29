using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsResultsReminder")]
    public class ResultsReminderLog : AuditLog
    {
        public ResultsReminderLog()
        {
            this.EventID = EventIdentifiers.Request.ResultsReminder.ID;
        }
        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
    }
    internal class ResultsReminderLogConfiguration : IEntityTypeConfiguration<ResultsReminderLog>
    {
        public void Configure(EntityTypeBuilder<ResultsReminderLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsResultsReminder");
        }
    }
}
