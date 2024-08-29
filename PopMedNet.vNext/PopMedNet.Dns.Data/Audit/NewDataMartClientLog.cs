using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Logging;
using PopMedNet.Dns.DTO.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsNewDataMartClient")]
    public class NewDataMartClientLog : AuditLog
    {
        public NewDataMartClientLog()
        {
            this.EventID = EventIdentifiers.DataMart.NewDataMartAvailable.ID;
        }

        public DateTime LastModified { get; set; }
    }
    internal class NewDataMartClientLogConfiguration : IEntityTypeConfiguration<NewDataMartClientLog>
    {
        public void Configure(EntityTypeBuilder<NewDataMartClientLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.LastModified }).HasName("PK_LogsNewDataMartClient");
        }
    }
}
