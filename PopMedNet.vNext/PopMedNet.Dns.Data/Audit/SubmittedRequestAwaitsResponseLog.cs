using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsSubmittedRequestAwaitsResponse")]
    public class SubmittedRequestAwaitsResponseLog : AuditLog
    {
        public SubmittedRequestAwaitsResponseLog()
        {
            this.EventID = EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class SubmittedRequestAwaitsResponseLogConfiguration : IEntityTypeConfiguration<SubmittedRequestAwaitsResponseLog>
    {
        public void Configure(EntityTypeBuilder<SubmittedRequestAwaitsResponseLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsSubmittedRequestAwaitsResponse");
        }
    }
}
