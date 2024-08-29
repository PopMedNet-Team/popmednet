using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("vLogsRoutingStatusChanged")]
    public class vRoutingStatusChangedLog : LogView
    {
        [ReadOnly(true)]
        public Guid RequestID { get; set; }

        [ReadOnly(true)]
        public Guid RequestDataMartID { get; set; }
    }
    internal class vRoutingStatusChangedLogConfiguration : IEntityTypeConfiguration<vRoutingStatusChangedLog>
    {
        public void Configure(EntityTypeBuilder<vRoutingStatusChangedLog> builder)
        {
            builder.HasKey(e => new { e.TimeStamp, e.UserID, e.RequestID, e.RequestDataMartID }).HasName("PK_vLogsRoutingStatusChanged");
        }
    }
}
