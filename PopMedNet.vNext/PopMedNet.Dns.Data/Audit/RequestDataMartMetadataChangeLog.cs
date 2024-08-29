using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsRequestDataMartMetadataChange")]
    public class RequestDataMartMetadataChangeLog : AuditLog
    {
        public RequestDataMartMetadataChangeLog()
        {
            this.EventID = EventIdentifiers.Request.DataMartMetadataChange.ID;
        }

        public Guid RequestID { get; set; }
        public virtual Request? Request { get; set; }
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart? RequestDataMart { get; set; }
        public Guid? TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }

        /// <summary>
        /// Contains a json object defining the individual property changes done.
        /// </summary>
        [MaxLength, Required]
        public string? ChangeDetail { get; set; }
    }
    internal class RequestDataMartMetadataChangeLogConfiguration : IEntityTypeConfiguration<RequestDataMartMetadataChangeLog>
    {
        public void Configure(EntityTypeBuilder<RequestDataMartMetadataChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RequestID, e.RequestDataMartID }).HasName("PK_LogsRequestDataMartMetadataChange");
        }
    }
}
