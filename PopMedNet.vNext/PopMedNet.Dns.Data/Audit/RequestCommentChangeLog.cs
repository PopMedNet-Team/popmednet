using PopMedNet.Dns.DTO.Events;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestCommentChange")]
    public class RequestCommentChangeLog : ChangeLog
    {
        public RequestCommentChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RequestCommentChange.ID;
        }

        public Guid CommentID { get; set; }
        public virtual Comment? Comment { get; set; }
    }
    internal class RequestCommentChangeLogConfiguration : IEntityTypeConfiguration<RequestCommentChangeLog>
    {
        public void Configure(EntityTypeBuilder<RequestCommentChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.CommentID }).HasName("PK_LogsRequestCommentChange");
            builder.Property(e => e.Reason).HasConversion<int>();
        }
    }
}
