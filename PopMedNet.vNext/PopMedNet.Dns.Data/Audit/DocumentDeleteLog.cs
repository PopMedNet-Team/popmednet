using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsDeletedDocumentArchive")]
    public class DocumentDeleteLog : AuditLog
    {
        public Guid DocumentID { get; set; }
        public Guid ItemID { get; set; }
    }
    
    internal class DocumentDeleteLogConfiguration : IEntityTypeConfiguration<DocumentDeleteLog>
    {
        public void Configure(EntityTypeBuilder<DocumentDeleteLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp }).HasName("PK_LogsDeletedDocumentArchive");
        }
    }
}
