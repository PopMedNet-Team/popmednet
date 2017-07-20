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
    [Table("LogsUserRegistrationChanged")]
    public class UserRegistrationChangedLog : AuditLog
    {
        public UserRegistrationChangedLog()
        {
            this.EventID = EventIdentifiers.User.RegistrationStatusChanged.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RegisteredUserID { get; set; }
        public virtual User RegisteredUser { get; set; }

    }

    internal class UserRegistrationChangedLogDTOMapping : EntityMappingConfiguration<UserRegistrationChangedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserRegistrationChangedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "User Registration Changed",
                    Message = d.Description
                };
            }
        }
    }
}
