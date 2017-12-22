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
    [Table("LogsUserRegistrationSubmitted")]
    public class UserRegistrationSubmittedLog : AuditLog
    {
        public UserRegistrationSubmittedLog()
        {
            this.EventID = EventIdentifiers.User.RegistrationSubmitted.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RegisteredUserID { get; set; }
        public virtual User RegisteredUser { get; set; }
    }

    internal class UserRegistrationSubmittedLogDTOMapping : EntityMappingConfiguration<UserRegistrationSubmittedLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserRegistrationSubmittedLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "User Registration Submitted",
                    Message = d.Description
                };
            }
        }
    }
}
