using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRoutingStatusChange")]
    public class RoutingStatusChangeLog : AuditLog
    {
        public RoutingStatusChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RoutingStatusChanged.ID;
        }

        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }
        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
        public DTO.Enums.RoutingStatus? OldStatus { get; set; }
        public DTO.Enums.RoutingStatus? NewStatus { get; set; }
        public Guid? ResponseID { get; set; }
        public virtual Response? Response { get; set; }
    }
    internal class RoutingStatusChangeLogConfiguration : IEntityTypeConfiguration<RoutingStatusChangeLog>
    {
        public void Configure(EntityTypeBuilder<RoutingStatusChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestDataMartID }).HasName("PK_LogsRoutingStatusChange");
            builder.Property(e => e.OldStatus).HasConversion<int>();
            builder.Property(e => e.NewStatus).HasConversion<int>();
        }
    }
}
