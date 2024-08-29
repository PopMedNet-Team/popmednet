using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsExternalCommunication")]
    public class ExternalCommunicationLog : ChangeLog
    {
        public ExternalCommunicationLog()
        {
            this.EventID = EventIdentifiers.ExternalCommunication.CommunicationFailed.ID;
            this.ID = DatabaseEx.NewGuid();
        }

        public Guid ID { get; set; }
    }

    internal class ExternalCommunicationLogConfiguration : IEntityTypeConfiguration<ExternalCommunicationLog>
    {
        public void Configure(EntityTypeBuilder<ExternalCommunicationLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.ID }).HasName("PK_LogsExternalCommunication");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
