using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Logging
{
    //Remember that all log tables must have their PKs and indexes created manually instead of by auto-code-first-migrations to specify the filegroup see: \Migrations\201407101708289_AddRegistryChangeLogs.cs for details.
    public abstract class AuditLog
    {
        public AuditLog()
        {
            this.TimeStamp = DateTimeOffset.UtcNow;
        }

        [Key, Column(Order = 1)]
        public Guid UserID { get; set; }
        [Key, Column(Order = 2)]
        public DateTimeOffset TimeStamp { get; set; }
        [Index]
        public Guid EventID { get; set; }

        [MaxLength]
        public string Description { get; set; }
    }
}
