using System.ComponentModel.DataAnnotations;

namespace PopMedNet.Utilities.Logging
{
    //Remember that all log tables must have their PKs and indexes created manually instead of by auto-code-first-migrations to specify the filegroup see: \Migrations\201407101708289_AddRegistryChangeLogs.cs for details.
    [Microsoft.EntityFrameworkCore.Index(nameof(EventID))]
    public abstract class AuditLog
    {
        public AuditLog()
        {
        }

        public Guid UserID { get; set; }
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
        public Guid EventID { get; set; }
        [MaxLength]
        public string? Description { get; set; }
    }
}
