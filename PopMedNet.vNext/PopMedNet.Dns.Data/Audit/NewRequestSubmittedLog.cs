using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsNewRequestSubmitted")]
    public class NewRequestSubmittedLog : AuditLog
    {
        public NewRequestSubmittedLog()
        {
            this.EventID = EventIdentifiers.Request.NewRequestSubmitted.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }

        public Guid? RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }
    }
    internal class NewRequestSubmittedLogConfiguration : IEntityTypeConfiguration<NewRequestSubmittedLog>
    {
        public void Configure(EntityTypeBuilder<NewRequestSubmittedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsNewRequestSubmitted");
        }
    }
}
