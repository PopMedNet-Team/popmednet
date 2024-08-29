using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Events;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsDataMartChange")]
    public class DataMartChangeLog : ChangeLog
    {
        public DataMartChangeLog()
        {
            this.EventID = EventIdentifiers.DataMart.Change.ID;
        }

        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
    }
    internal class DataMartChangeLogConfiguration : IEntityTypeConfiguration<DataMartChangeLog>
    {
        public void Configure(EntityTypeBuilder<DataMartChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.DataMartID }).HasName("PK_LogsDataMartChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
