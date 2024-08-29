using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestMetadataChange")]
    public class RequestMetadataChangeLog : AuditLog
    {
        public RequestMetadataChangeLog()
        {
            this.EventID = EventIdentifiers.Request.MetadataChange.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }

        /// <summary>
        /// Contains a json object defining the individual property changes done.
        /// </summary>
        [MaxLength, Required]
        public string ChangeDetail { get; set; } = string.Empty;
    }
    internal class RequestMetadataChangeLogConfiguration : IEntityTypeConfiguration<RequestMetadataChangeLog>
    {
        public void Configure(EntityTypeBuilder<RequestMetadataChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID }).HasName("PK_LogsRequestMetadataChange");
        }
    }
}
