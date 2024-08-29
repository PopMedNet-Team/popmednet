using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsSubmittedRequestNeedsApproval")]
    public class SubmittedRequestNeedsApprovalLog : AuditLog
    {
        public SubmittedRequestNeedsApprovalLog()
        {
            this.EventID = EventIdentifiers.Request.SubmittedRequestNeedsApproval.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class SubmittedRequestNeedsApprovalLogConfiguration : IEntityTypeConfiguration<SubmittedRequestNeedsApprovalLog>
    {
        public void Configure(EntityTypeBuilder<SubmittedRequestNeedsApprovalLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsSubmittedRequestNeedsApproval");
        }
    }
}
