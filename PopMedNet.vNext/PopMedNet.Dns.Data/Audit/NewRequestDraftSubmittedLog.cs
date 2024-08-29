using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsNewRequestDraftSubmitted")]
    public class NewRequestDraftSubmittedLog : AuditLog
    {
        public NewRequestDraftSubmittedLog()
        {
            this.EventID = EventIdentifiers.Request.NewRequestDraftSubmitted.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

    }
    internal class NewRequestDraftSubmittedLogConfiguration : IEntityTypeConfiguration<NewRequestDraftSubmittedLog>
    {
        public void Configure(EntityTypeBuilder<NewRequestDraftSubmittedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsNewRequestDraftSubmitted");
        }
    }
}
