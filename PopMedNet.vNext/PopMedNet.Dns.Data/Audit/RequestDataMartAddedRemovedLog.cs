using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestDataMartAddedRemoved")]
    public class RequestDataMartAddedRemovedLog : AuditLog
    {
        public RequestDataMartAddedRemovedLog()
        {
            this.EventID = EventIdentifiers.Request.RequestDataMartAddedRemoved.ID;
        }

        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }
        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
        public EntityState Reason { get; set; }

    }
    internal class RequestDataMartAddedRemovedLogConfiguration : IEntityTypeConfiguration<RequestDataMartAddedRemovedLog>
    {
        public void Configure(EntityTypeBuilder<RequestDataMartAddedRemovedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestDataMartID }).HasName("PK_dbo.LogsRequestDataMartAddedRemoved");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
