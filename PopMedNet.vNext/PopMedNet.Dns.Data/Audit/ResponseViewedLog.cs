using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsResponseViewed")]
    public class ResponseViewedLog : AuditLog
    {
        public ResponseViewedLog()
        {
            this.EventID = EventIdentifiers.Response.ResultsViewed.ID;
        }

        public Guid ResponseID { get; set; }
        public virtual Response? Response { get; set; }
    }
    internal class ResponseViewedLogConfiguration : IEntityTypeConfiguration<ResponseViewedLog>
    {
        public void Configure(EntityTypeBuilder<ResponseViewedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.ResponseID }).HasName("PK_LogsResponseViewed");
        }
    }
}
