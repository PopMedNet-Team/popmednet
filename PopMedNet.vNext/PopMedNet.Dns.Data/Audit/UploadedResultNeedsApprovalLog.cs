using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUploadedResultNeedsApproval")]
    public class UploadedResultNeedsApprovalLog : AuditLog
    {
        public UploadedResultNeedsApprovalLog()
        {
            this.EventID = EventIdentifiers.Response.UploadedResultNeedsApproval.ID;
        }

        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class UploadedResultNeedsApprovalLogConfiguration : IEntityTypeConfiguration<UploadedResultNeedsApprovalLog>
    {
        public void Configure(EntityTypeBuilder<UploadedResultNeedsApprovalLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestDataMartID }).HasName("PK_LogsUploadedResultNeedsApproval");
        }
    }
}
