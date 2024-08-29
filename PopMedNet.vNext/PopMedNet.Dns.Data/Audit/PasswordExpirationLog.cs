using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsPasswordExpiration")]
    public class PasswordExpirationLog : AuditLog
    {
        public PasswordExpirationLog()
        {
            this.EventID = EventIdentifiers.User.PasswordExpirationReminder.ID;
        }

        public Guid ExpiringUserID { get; set; }
        public virtual User? ExpiringUser { get; set; }
    }
    internal class PasswordExpirationLogConfiguration : IEntityTypeConfiguration<PasswordExpirationLog>
    {
        public void Configure(EntityTypeBuilder<PasswordExpirationLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.ExpiringUserID }).HasName("PK_LogsPasswordExpiration");
        }
    }
}
