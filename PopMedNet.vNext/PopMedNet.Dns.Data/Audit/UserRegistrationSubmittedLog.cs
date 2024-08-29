using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUserRegistrationSubmitted")]
    public class UserRegistrationSubmittedLog : AuditLog
    {
        public UserRegistrationSubmittedLog()
        {
            this.EventID = EventIdentifiers.User.RegistrationSubmitted.ID;
        }

        public Guid RegisteredUserID { get; set; }
        public virtual User? RegisteredUser { get; set; }
    }
    internal class UserRegistrationSubmittedLogConfiguration : IEntityTypeConfiguration<UserRegistrationSubmittedLog>
    {
        public void Configure(EntityTypeBuilder<UserRegistrationSubmittedLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.RegisteredUserID }).HasName("PK_LogsUserRegistrationSubmitted");
        }
    }
}
