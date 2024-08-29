using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestStatusChange")]
    public class RequestStatusChangedLog : ChangeLog
    {
        public RequestStatusChangedLog()
        {
            this.EventID = EventIdentifiers.Request.RequestStatusChanged.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public RequestStatuses OldStatus { get; set; }
        public RequestStatuses NewStatus { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
        public string? EmailBody { get; set; }
        public string? MyEmailBody { get; set; }
        public string? Subject { get; set; }
    }
    internal class RequestStatusChangedLogConfiguration : IEntityTypeConfiguration<RequestStatusChangedLog>
    {
        public void Configure(EntityTypeBuilder<RequestStatusChangedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_RequestStatusChangedLog");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
