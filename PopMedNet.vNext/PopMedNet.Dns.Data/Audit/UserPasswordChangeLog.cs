using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data.Audit
{
    [Table("LogsUserPasswordChange")]
    public class UserPasswordChangeLog
    {
        public UserPasswordChangeLog()
        {
        }

        /// <summary>
        /// Gets or sets the ID of the user doing the password change.
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// Gets or sets the user doing the password change.
        /// </summary>
        public User? User { get; set; }
        /// <summary>
        /// Gets or sets the timestamp for the log item.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
        /// <summary>
        /// Gets or sets the ID of the user who's password is getting changed.
        /// </summary>
        public Guid UserChangedID { get; set; }
        /// <summary>
        /// Gets or sets the user who's password is getting changed.
        /// </summary>
        public User? UserChanged { get; set; }
        /// <summary>
        /// Gets or sets a hashed value of the prior password.
        /// </summary>
        [MaxLength(100)]
        public string? OriginalPassword { get; set; }
        /// <summary>
        /// Gets or sets the reason/method the password was changed.
        /// </summary>
        public UserPasswordChange Method { get; set; }
    }
    internal class UserPasswordChangeLogConfiguration : IEntityTypeConfiguration<UserPasswordChangeLog>
    {
        public void Configure(EntityTypeBuilder<UserPasswordChangeLog> builder)
        {
            builder.HasKey(e => new { e.UserID, e.TimeStamp, e.UserChangedID }).HasName("PK_LogsUserPasswordChange");
            builder.HasIndex(i => new { i.TimeStamp, i.UserChangedID, i.OriginalPassword }, "IX_TimeStamp_UserChangedID_OriginalPassword").IsClustered(false).IsUnique(false);
            builder.Property(e => e.Method).HasConversion<int>();

        }
    }
}


