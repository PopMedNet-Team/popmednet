using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsPasswordExpiration")]
    public class PasswordExpirationLog : AuditLog
    {
        public PasswordExpirationLog()
        {
            this.EventID = EventIdentifiers.User.PasswordExpirationReminder.ID;
        }

        [Key, Column(Order = 3)]
        public Guid ExpiringUserID { get; set; }
        public virtual User ExpiringUser { get; set; }
    }

    internal class PasswordExpirationLogDTOMapping : EntityMappingConfiguration<PasswordExpirationLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<PasswordExpirationLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Password Expired",
                    Message = d.Description
                };
            }
        }
    }
}
