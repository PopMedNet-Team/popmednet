using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities;
using PopMedNet.Utilities.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUserAuthentication")]
    public class UserAuthenticationLogs : AuditLog
    {
        public UserAuthenticationLogs()
        {
            this.EventID = EventIdentifiers.User.Authentication.ID;
            this.ID = DatabaseEx.NewGuid();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public bool Success { get; set; }
        [MaxLength(40)]
        public string? IPAddress { get; set; }
        [MaxLength(50)]
        public string? Environment { get; set; }
        [MaxLength(500)]
        public string? Details { get; set; }
        [MaxLength(200)]
        public string? Source { get; set; }
        [MaxLength(20)]
        public string? DMCVersion { get; set; }
    }
    internal class UserAuthenticationLogsConfiguration : IEntityTypeConfiguration<UserAuthenticationLogs>
    {
        public void Configure(EntityTypeBuilder<UserAuthenticationLogs> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.ID }).HasName("PK_LogsUserAuthentication");
        }
    }
}
