using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("vLogsNewRequestsSubmitted")]
    public class vNewRequestSubmittedLog : LogView
    {
        [ReadOnly(true)]
        public Guid RequestID { get; set; }

    }
    internal class vNewRequestSubmittedLogConfiguration : IEntityTypeConfiguration<vNewRequestSubmittedLog>
    {
        public void Configure(EntityTypeBuilder<vNewRequestSubmittedLog> builder)
        {
            builder.HasKey(e => new { e.TimeStamp, e.UserID, e.RequestID }).HasName("PK_vLogsNewRequestsSubmitted");
        }
    }
}
