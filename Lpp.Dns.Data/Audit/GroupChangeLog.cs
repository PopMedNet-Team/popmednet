using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsGroupChange")]
    public class GroupChangeLog : ChangeLog
    {
        public GroupChangeLog()
        {
            this.EventID = EventIdentifiers.Group.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid GroupID { get; set; }
        public virtual Group Group { get; set; }
    }

    internal class GroupChangeLogDTOMapping : EntityMappingConfiguration<GroupChangeLog, NotificationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<GroupChangeLog, NotificationDTO>> MapExpression
        {
            get
            {
                return d => new NotificationDTO
                {
                    Timestamp = d.TimeStamp,
                    Event = "Group Changed",
                    Message = d.Description
                };
            }
        }
    }
}
