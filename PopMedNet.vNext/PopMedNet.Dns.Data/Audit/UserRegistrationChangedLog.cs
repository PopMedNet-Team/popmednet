using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUserRegistrationChanged")]
    public class UserRegistrationChangedLog : AuditLog
    {
        public UserRegistrationChangedLog()
        {
            this.EventID = EventIdentifiers.User.RegistrationStatusChanged.ID;
        }

        public Guid RegisteredUserID { get; set; }
        public virtual User? RegisteredUser { get; set; }
    }
    internal class UserRegistrationChangedLogConfiguration : IEntityTypeConfiguration<UserRegistrationChangedLog>
    {
        public void Configure(EntityTypeBuilder<UserRegistrationChangedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RegisteredUserID }).HasName("PK_LogsUserRegistrationChanged");
        }
    }
}
