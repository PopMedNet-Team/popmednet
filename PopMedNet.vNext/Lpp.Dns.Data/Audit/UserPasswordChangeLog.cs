using Lpp.Dns.DTO.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsUserPasswordChange")]
    public class UserPasswordChangeLog
    {
        public UserPasswordChangeLog()
        {
            this.TimeStamp = DateTimeOffset.UtcNow;
        }

        [Key, Column(Order = 1)]
        //This is the Acting User who did the change.
        public Guid UserID { get; set; }
        public User User { get; set; }
        [Key, Column(Order = 2)]
        public DateTimeOffset TimeStamp { get; set; }
        [Key, Column(Order = 3)]
        //This is the User who was changed.
        public Guid UserChangedID { get; set; }
        public User UserChanged { get; set; }
        [MaxLength(100)]
        public string OriginalPassword { get; set; }
        public UserPasswordChange Method { get; set; }
    }

    internal class UserPasswordChangeLogConfiguration : EntityTypeConfiguration<UserPasswordChangeLog>
    {
        public UserPasswordChangeLogConfiguration()
        {
            HasIndex(x => new { x.TimeStamp, x.UserChangedID, x.OriginalPassword });
        }
    }
}
