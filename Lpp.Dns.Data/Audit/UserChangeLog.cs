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
    [Table("LogsUserChange")]
    public class UserChangeLog : ChangeLog
    {
        public UserChangeLog()
        {
            this.EventID = EventIdentifiers.User.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid UserChangedID { get; set; }
        public virtual User UserChanged { get; set; }
    }

    internal class UserChangeLogDTOMapping : EntityMappingConfiguration<UserChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "User Changed",
                    Message = d.Description
                };
            }
        }
    }
}
