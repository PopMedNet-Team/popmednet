using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestDocumentChange")]
    public class RequestDocumentChangeLog : ChangeLog
    {
        public RequestDocumentChangeLog()
        {
            this.EventID = EventIdentifiers.Document.Change.ID;
        }

        public Guid DocumentID { get; set; }
        public virtual Document? Document { get; set; }
        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
    }
    internal class RequestDocumentChangeLogConfiguration : IEntityTypeConfiguration<RequestDocumentChangeLog>
    {
        public void Configure(EntityTypeBuilder<RequestDocumentChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.DocumentID, e.RequestID }).HasName("PK_LogsRequestDocumentChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
