using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Events;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsDocumentChange")]
    public class DocumentChangeLog : ChangeLog
    {
        public DocumentChangeLog()
        {
            this.EventID = EventIdentifiers.Document.Change.ID;
        }

        public Guid DocumentID { get; set; }
        public virtual Document? Document { get; set; }
        public Guid ItemID { get; set; }

    }

    internal class DocumentChangeLogConfiguration : IEntityTypeConfiguration<DocumentChangeLog>
    {
        public void Configure(EntityTypeBuilder<DocumentChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.DocumentID, e.ItemID }).HasName("PK_LogsDocumentChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
